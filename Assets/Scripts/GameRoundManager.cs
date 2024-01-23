using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameRoundManager : MonoObserverInterface
{
    public enum GameState
    {
        NormalRound = 0,
        Replay,
        Size
    }

    public enum CharacterIndex
    {
        Player = 0,
        Enemy,
        Size
    }
    
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

    private GameState _gameState = GameState.NormalRound;
    
    private bool _isGameRecorded = false;
    
    private List<int> _downPlayers = new List<int>();

    private RoundInfoManager _roundInfoManager;

    
    
    // Start is called before the first frame update
    void Start()
    {
        _roundInfoManager = new RoundInfoManager();
        
        PlayerCharacter player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerCharacter>();
        PlayerCharacter enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.GetComponent<PlayerCharacter>();
        
        
        _players.Insert((int)CharacterIndex.Player, player);
        player.PlayerUniqueIndex = _players.Count;
       
        _players.Insert((int)CharacterIndex.Enemy, enemy);
        enemy.PlayerUniqueIndex = _players.Count;

        _inputManagers = GameObject.FindObjectsOfType<CharacterInputManager>();

        _canvas = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        _resultPanel = Instantiate(ResultTextPrefab, Vector3.zero, Quaternion.identity, _canvas);
        _resultPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _text = _resultPanel.GetComponentInChildren<TextMeshProUGUI>();
        _resultPanel.SetActive(false);
        
        RoundRemainTime = InitRemainTime;
        StartRound();
        
        /*
        // -- for Replay By Input -- //
        _playerInput = player.GetComponent<CharacterInputManager>();
        _enemyInput = enemy.GetComponent<CharacterInputManager>();
        */
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameStopped || IsGameEnded) return;
        RoundRemainTime -= Time.deltaTime;

        if (RoundRemainTime <= 0.0f)
            DrawRound();

        switch (_gameState)
        {
            case GameState.Replay:
                break;
            case GameState.NormalRound:
                if(_isGameRecorded) RecordGameByState();
                break;
        }
    }

    public void Replay()
    {
        _gameState = GameState.Replay;
        StartRound();
        BlockAllPlayersInput();
        _isGameRecorded = false;
        _roundInfoManager.LoadPreviousRoundInfoFromJson(_playersInputQueueForReplay, _enemyInputQueueForReplay);
    }

    public void SaveRoundUntilNow()
    {
        DrawRound();
        StartRound();
    }
    
    public void StartRound()
    {
        _gameState = GameState.NormalRound;
        _isGameRecorded = true;
        
        _resultPanel.SetActive(false);
        AcceptAllPlayersInput();
        
        RoundRemainTime = InitRemainTime;
        _players[(int)CharacterIndex.Player].transform.position = Vector3.left;
        _players[(int)CharacterIndex.Enemy].transform.position = Vector3.right;
        
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
        _roundInfoManager.EnqueueEntryInput(tagName, button);
    }


    private BehaviorEnumSet.State _previousPlayerState = BehaviorEnumSet.State.Null;
    private BehaviorEnumSet.State _previousEnemyState = BehaviorEnumSet.State.Null;
    
    public void EnqueueRoundState(CharacterIndex index, BehaviorEnumSet.State state, int frame, Vector2 position, Vector2 velocity)
    {
        bool doEnqueue = true;
        switch (index)
        {
            case CharacterIndex.Player:
                if (state == _previousPlayerState) doEnqueue = false;
                break;
            case CharacterIndex.Enemy:
                if (state == _previousEnemyState) doEnqueue = false;
                break;
            default:
                return;
        }
        if(doEnqueue) _roundInfoManager.EnqueueEntryState(index, state, position, velocity);
    }
    
    private void RecordGameByState()
    {
        for (CharacterIndex index = 0; index < CharacterIndex.Size; index++)
        {
            PlayerCharacter player = _players[(int)index];
            
        }
    }

    private Queue<int> _playersInputQueueForReplay = new Queue<int>();
    private Queue<int> _enemyInputQueueForReplay = new Queue<int>();
    private CharacterInputManager _playerInput;
    private CharacterInputManager _enemyInput;
    private void ReplayGameByInput()
    {
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
                    BehaviorEnumSet.Button enemyInput = (BehaviorEnumSet.Button)_enemyInputQueueForReplay.Dequeue();
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
                    BehaviorEnumSet.Button playerInput = (BehaviorEnumSet.Button)_playersInputQueueForReplay.Dequeue();
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
}
