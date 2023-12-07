using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameRoundManager : MonoObserverInterface
{
    public GameObject ResultTextPrefab;

    private GameObject _resultPanel;
    private TextMeshProUGUI _text;
    private RectTransform _canvas;
    
    private List<PlayerCharacter> _players = new List<PlayerCharacter>();
    private CharacterInputManager[] _inputManagers;

    public float RoundRemainTime { get; private set; }

    public float InitRemainTime = 100.0f;
    
    public bool IsGameStopped { get; set; } = false;

    public bool IsGameEnded { get; set; } = false;

    private bool _isGameReplayed = false;
    
    private List<int> _downPlayers = new List<int>();

    private RoundInfoManager _roundInfoManager;

    private Queue<UnionInfo> _playersInputQueueForReplay = new Queue<UnionInfo>();
    private Queue<UnionInfo> _enemyInputQueueForReplay = new Queue<UnionInfo>();
    
    private Queue<UnionInfo> _playersPositionQueueForReplay = new Queue<UnionInfo>();
    private Queue<UnionInfo> _enemyPositionQueueForReplay = new Queue<UnionInfo>();

    private CharacterInputManager _playerInput;
    private CharacterInputManager _enemyInput;
    
    // Start is called before the first frame update
    void Start()
    {
        _roundInfoManager = new RoundInfoManager();
        
        PlayerCharacter player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerCharacter>();
        PlayerCharacter enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.GetComponent<PlayerCharacter>();

        _playerInput = player.GetComponent<CharacterInputManager>();
        _players.Add(player);
        player.PlayerUniqueIndex = _players.Count;
        _enemyInput = enemy.GetComponent<CharacterInputManager>();
        _players.Add(enemy);
        enemy.PlayerUniqueIndex = _players.Count;

        _inputManagers = GameObject.FindObjectsOfType<CharacterInputManager>();

        _canvas = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        _resultPanel = Instantiate(ResultTextPrefab, Vector3.zero, Quaternion.identity, _canvas);
        _resultPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _text = _resultPanel.GetComponentInChildren<TextMeshProUGUI>();
        _resultPanel.SetActive(false);
        
        RoundRemainTime = InitRemainTime;
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameStopped || IsGameEnded) return;
        RoundRemainTime -= Time.deltaTime;

        if (RoundRemainTime <= 0.0f)
            DrawRound();

        if (!_isGameReplayed) return;
        bool endedPlayerInputEnqueue = false;
        bool endedEnemyInputEnqueue = false;
        while (true)
        {
            if (!endedEnemyInputEnqueue)
            {
                if (_enemyInputQueueForReplay.Count == 0)
                {
                    endedEnemyInputEnqueue = true;
                }
                else
                {
                    UnionInfo enemyInput = _enemyInputQueueForReplay.Peek();
                    if (enemyInput.Frame > FrameManager.CurrentFrame)
                    {
                        endedEnemyInputEnqueue = true;
                    }
                    else _enemyInput.EnqueueInputQueue((BehaviorEnumSet.Button)_enemyInputQueueForReplay.Dequeue().Button);
                }
            }
            if (!endedPlayerInputEnqueue)
            {
                if (_playersInputQueueForReplay.Count == 0)
                {
                    endedPlayerInputEnqueue = true;
                }
                else
                {
                    UnionInfo playerInput = _playersInputQueueForReplay.Peek();
                    if (playerInput.Frame > FrameManager.CurrentFrame)
                    {
                        endedPlayerInputEnqueue = true;
                    }
                    else _playerInput.EnqueueInputQueue((BehaviorEnumSet.Button)_playersInputQueueForReplay.Dequeue().Button);
                }
            }
            
            if (endedPlayerInputEnqueue && endedEnemyInputEnqueue)
                break;
        }

        while (_enemyPositionQueueForReplay.Count > 0)
        {
            if (_enemyPositionQueueForReplay.Peek().Frame == FrameManager.CurrentFrame)
            {
                UnionInfo positionInfo = _enemyPositionQueueForReplay.Dequeue();
                Vector3 enemyPosition = _enemyInput.transform.position;
                enemyPosition.x = positionInfo.X;
                enemyPosition.y = positionInfo.Y;
                _enemyInput.transform.position = enemyPosition;
                break;
            }
            else if (_enemyPositionQueueForReplay.Peek().Frame < FrameManager.CurrentFrame)
            {
                _enemyPositionQueueForReplay.Dequeue();
            }
        }
        
        while (_playersPositionQueueForReplay.Count > 0)
        {
            if (_playersPositionQueueForReplay.Peek().Frame == FrameManager.CurrentFrame)
            {
                UnionInfo positionInfo = _playersPositionQueueForReplay.Dequeue();
                Vector3 playerPosition = _playerInput.transform.position;
                playerPosition.x = positionInfo.X;
                playerPosition.y = positionInfo.Y;
                _playerInput.transform.position = playerPosition;
                break;
            }
            else if (_playersPositionQueueForReplay.Peek().Frame < FrameManager.CurrentFrame)
            {
                _playersPositionQueueForReplay.Dequeue();
            }
        }
    }

    public void Replay()
    {
        StartRound();
        BlockAllPlayersInput();
        _isGameReplayed = true;
        _roundInfoManager.LoadPreviousRoundInfoFromJson(
            _playersInputQueueForReplay, _enemyInputQueueForReplay,
            _playersPositionQueueForReplay, _enemyPositionQueueForReplay,
            _playerInput.transform, _enemyInput.transform);
    }

    public void SaveRoundUntilNow()
    {
        DrawRound();
        StartRound();
    }
    
    public void StartRound()
    {
        FrameManager.Reset();
        _isGameReplayed = false;
        
        _resultPanel.SetActive(false);
        AcceptAllPlayersInput();
        
        RoundRemainTime = InitRemainTime;
        _players[0].transform.position = Vector3.left;
        _players[1].transform.position = Vector3.right;
        
        foreach (var player in _players)
        {
            player.IsAcceptArtificialInput = false;
            player.ComboManagerInstance.IsCanceled = true;
            player.IsGuarded = false;
            player.ResetHp();
            player.StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
            player.GetComponent<CharacterJudgeBoxController>().EnableHitBox();
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        
        _downPlayers.Clear();
        _roundInfoManager.Clear();
        IsGameEnded = false;
    }
    
    private void EndRound(int playerUnique)
    {
        IsGameEnded = true;
        BlockAllPlayersInput();
        _text.text = "Player " + playerUnique + " Win";
        _resultPanel.SetActive(true);
        
        _roundInfoManager.SaveRoundInfoToJson();
        _roundInfoManager.Clear();
    }

    private void DrawRound()
    {
        IsGameEnded = true;
        BlockAllPlayersInput();
        _text.text = "Draw";
        _resultPanel.SetActive(true);
        
        _roundInfoManager.SaveRoundInfoToJson();
        _roundInfoManager.Clear();
    }
    
    public void BlockAllPlayersInput()
    {
        foreach (var manager in _inputManagers)
        {
            manager.IsAvailableInput = false;
        }
    }

    public void AcceptAllPlayersInput()
    {
        foreach (var manager in _inputManagers)
        {
            manager.IsAvailableInput = true;
        }
    }

    public override void Notify()
    {
        foreach (var player in _players)
        {
            if (player.Hp <= 0)
            {
                _downPlayers.Add(player.PlayerUniqueIndex);
                player.StateManager.ChangeState(BehaviorEnumSet.State.InAirHit);
            }
        }

        switch (_downPlayers.Count)
        {
            case 1:
                EndRound(_downPlayers[0]);
                break;
            case 2:
                DrawRound();
                break;
        }
    }

    public void EnqueueRoundInput(string tagName, BehaviorEnumSet.Button button, int frame)
    {
        _roundInfoManager.EnqueueInfoInput(tagName, button, frame);
    }
    
    public void EnqueueRoundInput(string tagName, float x, float y, int frame)
    {
        _roundInfoManager.EnqueueInfoPosition(tagName, x, y, frame);
    }
}
