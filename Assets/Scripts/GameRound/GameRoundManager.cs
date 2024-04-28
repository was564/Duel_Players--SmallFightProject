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
    
    //private List<PlayerCharacter> _players = new List<PlayerCharacter>();
    //private CharacterInputManager[] _inputManagers;

    public float RoundRemainTime { get; private set; }

    public float InitRemainTime { get; set; } = 100.0f;
    
    public bool IsGamePaused { get; set; } = false;

    public bool IsGameEnded { get; set; } = false;

    // case : NormalPlaying, Replaying
    //private GameState _initGameRoundState;
    
    private bool _isGameRecorded = false;

    
    private RoundRecordManager _gameRoundRecordManager;
    
    
    // Start is called before the first frame update
    void Start()
    {

        _frameManager = GameObject.FindObjectOfType<FrameManager>();
        _playersControlManager = new PlayersInRoundControlManager();
        
        _gameRoundStateManager = new GameRoundStateManager(this, _playersControlManager, _frameManager);
        _gameRoundRecordManager = new RoundRecordManager();
        
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
        
        /*
        if(_playersControlManager.GetIsInitializedPlayers())
            _playersControlManager.InitializePlayersInStartingGame();
        else
        {
            Debug.Log("Initialize Failure");
        }
        */
        
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

    public void ApplySettingInStateByPausing(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0.0f;
            
            _playersControlManager.ChangeModeToStopOfAllPlayers();
        }
        else
        {
            Time.timeScale = 1.0f;

            _playersControlManager.ChangePreviousModeOfAllPlayers();
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
        StartRoundFromIntro();
        _playersControlManager.BlockAllPlayersInput();
        _playersControlManager.InitializePlayersInRound(GameState.Replay);
        _playersControlManager.ChangeModeToStopOfAllPlayers();
        _playersControlManager.ChangePreviousModeOfAllPlayers();
        _isGameRecorded = false;
        _gameRoundRecordManager.LoadPreviousRoundInfoFromJsonToPlayers();
        
        InitializeRound(GameState.Replay);
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
        _isGameRecorded = true;
        IsGameEnded = false;
        
        _gameRoundRecordManager.Clear();
        _resultPanel.SetActive(false);
        _playersControlManager.AcceptAllPlayersInput();
        
        RoundRemainTime = InitRemainTime;
        _frameManager.ResetFrame();
        
        InitializeRound(GameState.Start);
    }
    
    private void InitializeRound(GameState state)
    {
        _gameRoundStateManager.ChangeState(state);
        _playersControlManager.ChangeModeToStopOfAllPlayers();
        _playersControlManager.ChangePreviousModeOfAllPlayers();
    }
    
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
