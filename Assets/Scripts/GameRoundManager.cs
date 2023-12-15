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

    private Queue<BehaviorEnumSet.Button> _playersInputQueueForReplay = new Queue<BehaviorEnumSet.Button>();
    private Queue<BehaviorEnumSet.Button> _enemyInputQueueForReplay = new Queue<BehaviorEnumSet.Button>();

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
                    BehaviorEnumSet.Button enemyInput = _enemyInputQueueForReplay.Dequeue();
                    if (enemyInput == BehaviorEnumSet.Button.Null)
                    {
                        endedEnemyInputEnqueue = true;
                    }
                    else _enemyInput.EnqueueInputQueue(enemyInput);
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
                    BehaviorEnumSet.Button playerInput = _playersInputQueueForReplay.Dequeue();
                    if (playerInput == BehaviorEnumSet.Button.Null)
                    {
                        endedPlayerInputEnqueue = true;
                    }
                    else _playerInput.EnqueueInputQueue(playerInput);
                }
            }
            
            if (endedPlayerInputEnqueue && endedEnemyInputEnqueue)
                break;
        }
    }

    public void Replay()
    {
        StartRound();
        BlockAllPlayersInput();
        _isGameReplayed = true;
        _roundInfoManager.LoadPreviousRoundInfoFromJson(_playersInputQueueForReplay, _enemyInputQueueForReplay);
    }

    public void SaveRoundUntilNow()
    {
        DrawRound();
        StartRound();
    }
    
    public void StartRound()
    {
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
        _roundInfoManager.EnqueueEntryInput(tagName, button, frame);
    }
}
