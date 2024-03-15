using System.Collections;
using System.Collections.Generic;
using Character.PlayerMode;
using GameRound;
using TMPro;
using UnityEngine;

public class GameRoundManager : MonoObserverInterface
{
    // monoObserver 대신 delegate 함수 사용
    public enum GameState
    {
        NormalPlay = 0,
        Replay,
        Start,
        Pause,
        Wait,
        End,
        Size
    }

    /*
    public enum CharacterIndex
    {
        Player = 0,
        Enemy,
        Size
    }
    */
    
    public GameObject ResultTextPrefab;

    private GameObject _resultPanel;
    private TextMeshProUGUI _text;
    private RectTransform _canvas;

    private MenuInputManager _menuInputManager;
    
    private FrameManager _frameManager;

    private GameRoundStateManager _gameRoundStateManager;

    
    private PlayersInRoundControlManager _playersControlManager;
    
    //private List<PlayerCharacter> _players = new List<PlayerCharacter>();
    //private CharacterInputManager[] _inputManagers;

    public float RoundRemainTime { get; private set; }

    public float InitRemainTime { get; set; } = 100.0f;
    
    public bool IsGamePaused { get; set; } = false;

    public bool IsGameEnded { get; set; } = false;

    // case : NormalPlaying, Replaying
    private GameState _initGameRoundState;
    
    private bool _isGameRecorded = false;

    private RoundInfoManager _roundInfoManager;

    
    
    // Start is called before the first frame update
    void Start()
    {
        _initGameRoundState = GameState.NormalPlay;
        _roundInfoManager = new RoundInfoManager();

        _frameManager = GameObject.FindObjectOfType<FrameManager>();
        _playersControlManager = new PlayersInRoundControlManager();
        
        _gameRoundStateManager = new GameRoundStateManager(this, _playersControlManager, _frameManager);
        
        /*
        PlayerCharacter player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerCharacter>();
        PlayerCharacter enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.GetComponent<PlayerCharacter>();

        _players.Insert((int)CharacterIndex.Player, player);
        player.PlayerUniqueIndex = _players.Count;
       
        _players.Insert((int)CharacterIndex.Enemy, enemy);
        enemy.PlayerUniqueIndex = _players.Count;

        _inputManagers = GameObject.FindObjectsOfType<CharacterInputManager>();
        */
        
        _canvas = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        _resultPanel = Instantiate(ResultTextPrefab, Vector3.zero, Quaternion.identity, _canvas);
        _resultPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _text = _resultPanel.GetComponentInChildren<TextMeshProUGUI>();
        _resultPanel.SetActive(false);
        
        RoundRemainTime = InitRemainTime;
        StartRoundFromIntro();
        
        /*
        // -- for Replay By Input -- //
        _playerInput = player.GetComponent<CharacterInputManager>();
        _enemyInput = enemy.GetComponent<CharacterInputManager>();
        */
    } 

    // Update is called once per frame
    void Update()
    {
        _gameRoundStateManager.Update();
        /*
        if(IsGameStopped || IsGameEnded) return;
        RoundRemainTime -= Time.deltaTime;
    
        if (RoundRemainTime <= 0.0f)
            DrawRound();

        switch (_gameState)                     
        {
            case GameState.Replay:
                break;
            case GameState.NormalPlay:
                if(_isGameRecorded) RecordGameByState();
                break;
        }
        */
    }

    public void ApplySettingInStateByPausing(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0.0f;
            
            _playersControlManager.ChangeModeOfAllPlayers(PlayerModeManager.PlayerMode.GamePause);
        }
        else
        {
            Time.timeScale = 1.0f;

            PlayerModeManager.PlayerMode mode;
            if (_initGameRoundState == GameState.Replay)
                _playersControlManager.ChangeModeOfAllPlayers(PlayerModeManager.PlayerMode.Replaying);
            else if (_initGameRoundState == GameState.NormalPlay)
                _playersControlManager.ChangeModeOfAllPlayers(PlayerModeManager.PlayerMode.NormalPlaying);
            
            else _playersControlManager.ChangeModeOfAllPlayers(PlayerModeManager.PlayerMode.NormalPlaying);
        }
    
    }
    
    public void PauseGame()
    {
        _gameRoundStateManager.ChangeState(GameState.Pause);
    }
    
    public void ResumeGame()
    {
        _gameRoundStateManager.ChangeState(GameState.NormalPlay);
    }
    
    public void Replay()
    {
        _initGameRoundState = GameState.Replay;
        StartRoundFromIntro();
        _playersControlManager.BlockAllPlayersInput();
        _isGameRecorded = false;
        //_roundInfoManager.LoadPreviousRoundInfoFromJson(_playersInputQueueForReplay, _enemyInputQueueForReplay);
    }

    public void DecreaseRemainTimePerFrame(int frame)
    {
        RoundRemainTime -= (float)frame / 60.0f;
    }
    
    public void SaveRoundUntilNow()
    {
        DrawRound();
        StartRoundFromIntro();
    }
    
    public void StartRoundFromIntro()
    {
        _initGameRoundState = GameState.NormalPlay;
        _isGameRecorded = true;
        IsGameEnded = false;
        
        _resultPanel.SetActive(false);
        _playersControlManager.AcceptAllPlayersInput();
        
        RoundRemainTime = InitRemainTime;
        
        _gameRoundStateManager.ChangeState(GameState.Start);
        _playersControlManager.ChangeInitializeRoundState(GameState.Start);
        
        _playersControlManager.InitializePlayersInRound();
    }
    
    private void EndRound(PlayersInRoundControlManager.CharacterIndex characterIndex)
    {
        IsGameEnded = true;
        _playersControlManager.BlockAllPlayersInput();
        _text.text = "Player " + (int)characterIndex + " Win";
        _resultPanel.SetActive(true);
        
        _roundInfoManager.SaveRoundInfoToJson();
        _roundInfoManager.Clear();
        
        _gameRoundStateManager.ChangeState(GameState.End);
    }

    private void DrawRound()
    {
        IsGameEnded = true;
        _playersControlManager.BlockAllPlayersInput();
        _text.text = "Draw";
        _resultPanel.SetActive(true);
        
        _roundInfoManager.SaveRoundInfoToJson();
        _roundInfoManager.Clear();
    }

    public override void Notify()
    {
        IsGameEnded = true;
        
        int countingDownPlayers = _playersControlManager.CountDownPlayers();
        switch (countingDownPlayers)
        {
            case 0:
                break;
            case 1:
                EndRound(_playersControlManager.GetDownPlayerIndex());
                break;
            case 2:
                DrawRound();
                break;
            default:
                break;
        }
    }
    
    /*
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
    */
}
