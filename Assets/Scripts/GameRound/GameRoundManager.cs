using System.Collections;
using System.Collections.Generic;
using Character.PlayerMode;
using DefaultNamespace;
using GameRound;
using TMPro;
using UnityEngine;

public class GameRoundManager : MonoObserverInterface
{
    // monoObserver 대신 delegate 함수 사용
    public enum GameState
    {
        SingleNormalPlay = 0,
        MultiNormalPlay,
        Replay,
        Start,
        Pause,
        Wait,
        End,
        Result,
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

    private LeaveDataForGame _data;

    private AudioSource[] _audioSources;
    
    //private List<PlayerCharacter> _players = new List<PlayerCharacter>();
    //private CharacterInputManager[] _inputManagers;

    public float RoundRemainTime { get; private set; }

    public float InitRemainTime { get; set; } = 100.0f;
    
    public bool IsGamePaused { get; set; } = false;

    public bool IsGameEnded { get; set; } = false;

    // case : NormalPlaying, Replaying
    //private GameState _initGameRoundState;

    
    private RoundRecordManager _gameRoundRecordManager;


    public GameState GetSelectedGameState()
    {
        if (_data == null) return GameState.SingleNormalPlay;
        return _data.SelectedGameState;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.FindObjectOfType<LeaveDataForGame>();
        _frameManager = GameObject.FindObjectOfType<FrameManager>();
        _playersControlManager = new PlayersInRoundControlManager();
        
        _gameRoundStateManager = new GameRoundStateManager(this, _playersControlManager, _frameManager);
        _gameRoundRecordManager = new RoundRecordManager();
        _audioSources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        
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
        //_resultPanel = Instantiate(ResultTextPrefab, Vector3.zero, Quaternion.identity, _canvas);
        _resultPanel = GameObject.FindWithTag("Result");
        _resultPanel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        _text = _resultPanel.GetComponentInChildren<TextMeshProUGUI>();
        _resultPanel.SetActive(false);
        
        if(_playersControlManager.GetIsInitializedPlayers())
            _playersControlManager.InitializePlayersInStartingGame();
        else
        {
            Debug.Log("Initialize Failure");
        }
        
        RoundRemainTime = InitRemainTime;
        
        switch (GetSelectedGameState())
        {
            case GameState.SingleNormalPlay:
                StartRoundFromIntro();
                break;
            
            case GameState.MultiNormalPlay:
                StartRoundFromIntro();
                break;
            
            case GameState.Replay:
                Replay();
                break;
            
            default:
                StartRoundFromIntro();
                break;
        }

        foreach (var source in _audioSources)
        {
            if (_data == null)
            {
                source.volume = 0.5f;
                continue;
            }
            source.volume = _data.Volume;
        }
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
        
        //_gameRoundRecordManager.Update();
        
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

    private bool _previousIsGamePaused = true;
    public void ApplySettingInStateByPausing(bool isPaused)
    {
        if (isPaused)
        {
            _previousIsGamePaused = true;
            Time.timeScale = 0.0f;
            
            _playersControlManager.ChangeModeToStopOfAllPlayers();
        }
        else
        {
            if (_previousIsGamePaused == false) return;
            Time.timeScale = 1.0f;

            _playersControlManager.ChangePreviousModeOfAllPlayers();
            _previousIsGamePaused = false;
        }
    
    }
    
    public void PauseGame()
    {
        _gameRoundStateManager.ChangeState(GameState.Pause);
    }
    
    public void ResumeGame()
    {
        _gameRoundStateManager.ChangeState(GameState.SingleNormalPlay);
    }
    
    public void Replay()
    {
        StartRoundFromIntro();
        _playersControlManager.BlockAllPlayersInput();
        _gameRoundRecordManager.IsGameRecorded = false;
        _gameRoundRecordManager.LoadPreviousRoundInfoFromJsonToPlayers();
        
        _playersControlManager.InitializePlayersInRound(GameState.Replay);
        _gameRoundStateManager.ChangeState(GameState.Replay);
        _playersControlManager.ChangeModeToStopOfAllPlayers();
        _playersControlManager.ChangePreviousModeOfAllPlayers();
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
        _gameRoundRecordManager.IsGameRecorded = true;
        IsGameEnded = false;
        
        _gameRoundRecordManager.Clear();
        _resultPanel.SetActive(false);
        _playersControlManager.AcceptAllPlayersInput();
        
        RoundRemainTime = InitRemainTime;
        _frameManager.ResetFrame();
        
        _playersControlManager.InitializePlayersInRound(GameState.Start);
        _gameRoundStateManager.ChangeState(GameState.Start);
        _playersControlManager.ChangeModeToStopOfAllPlayers();
        _playersControlManager.ChangePreviousModeOfAllPlayers();
    }
    
    /*
    private void InitializeRound(GameState state)
    {
        _gameRoundStateManager.ChangeState(state);
        //_playersControlManager.ChangeModeToStopOfAllPlayers();
        //_playersControlManager.ChangePreviousModeOfAllPlayers();
    }
    */
    
    public void EndRound(PlayerCharacter.CharacterIndex winCharacterIndex)
    {
        IsGameEnded = true;
        _playersControlManager.BlockAllPlayersInput();
        _text.text = "Player " + (int)winCharacterIndex + " Win";
        _resultPanel.SetActive(true);
        
        _gameRoundRecordManager.SaveRoundInfoToJson();
        
        //_gameRoundStateManager.ChangeState(GameState.Wait);
    }

    public void DrawRound()
    {
        IsGameEnded = true;
        _playersControlManager.BlockAllPlayersInput();
        _text.text = "Draw";
        _resultPanel.SetActive(true);
        
        _gameRoundRecordManager.SaveRoundInfoToJson();
    }

    public override void Notify()
    {
        IsGameEnded = true;
        
        /*
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
        */
    }
    
    
}
