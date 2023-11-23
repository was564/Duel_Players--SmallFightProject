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
    
    private List<int> _downPlayers = new List<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter player = GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerCharacter>();
        PlayerCharacter enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.GetComponent<PlayerCharacter>();
        
        _players.Add(player);
        player.PlayerUniqueIndex = _players.Count;
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
        
    }

    public void StartRound()
    {
        _resultPanel.SetActive(false);
        AcceptAllPlayersInput();

        RoundRemainTime = InitRemainTime;
        _players[0].transform.position = Vector3.left;
        _players[1].transform.position = Vector3.right;
        

        foreach (var player in _players)
        {
            player.ResetHp();
            player.CharacterPositionState = PassiveStateEnumSet.CharacterPositionState.Size;
            player.StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        
        _downPlayers.Clear();
    }
    
    private void EndRound(int playerUnique)
    {
        BlockAllPlayersInput();
        _text.text = "Player " + playerUnique + " Win";
        _resultPanel.SetActive(true);
    }

    private void DrawRound()
    {
        BlockAllPlayersInput();
        _text.text = "Draw";
        _resultPanel.SetActive(true);
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
}
