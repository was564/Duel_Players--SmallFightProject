using System;
using System.Collections.Generic;
using BehaviorTree;
using Character;
using Character.CharacterFSM;
using Character.CharacterPassiveState;
using Character.PlayerMode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacter : MonoPublisherInterface, ControlPlayerInterface
{
    [SerializeField] public GameObject EnemyObject;

    private GameObject _wall;

    private PlayerCharacter _enemyCharacter;
    
    public Rigidbody RigidBody { get; private set; }
    public BoxCollider CharacterCollider { get; private set; }
    private bool _isLookingRight;

    public GameObject SpecialSkillBall;
    
    public GameObject SpecialSkillBallInstance { get; private set; }
    
    private List<GameObject> _modelObjectsForRender = new List<GameObject>();
    
    private CharacterInputManager _inputManager;
    private CommandProcessor _commandProcessor;
    private PassiveStateManager _passiveStateManager;

    private PlayerModeManager _playerModeManager;
    public BehaviorStateManager StateManager { get; private set; }
    private BehaviorStateSimulator _stateSimulatorInStoppedFrame;
    private BehaviorStateSimulator _currentStateManager;
    
    public ComboManager ComboManagerInstance { get; private set; }
    
    public float PositionYOffsetForLand { get; private set; } = -0.6f;

    //private bool isPaused = false;
    
    public PassiveStateEnumSet.CharacterPositionState CurrentCharacterPositionState { get; set; }
        = PassiveStateEnumSet.CharacterPositionState.Size;
    
    public int Hp { get; private set; } = 100;

    public bool IsHitContinuous { get; set; } = false;
    public bool IsGuarded { get; set; } = false;

    public bool IsAcceptArtificialInput = false;

    public List<BehaviorEnumSet.Button> ArtificialButtons = new List<BehaviorEnumSet.Button>();
    
    public List<BehaviorEnumSet.Button> ArtificialButtonsInSameTime = new List<BehaviorEnumSet.Button>();

    private float _addingGravityMultiple = 1.3f;
    
    // for checking end point of intro animation and outro animation
    public bool IsEndedPoseAnimation { get; set; } = false;
    
    public int PlayerUniqueIndex { get; set; }

    private List<MonoObserverInterface> _observers = new List<MonoObserverInterface>();
    
    

    private void OnCollisionEnter(Collision other)
    {   // 플레이어와 적이 머리 위와 다리 아래로 충돌 했을 때 공중에 멈추지 않게 위치를 조정하는 함수
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Enemy")) return;
        // 공중에 있는 플레이어가 해당 코드를 처리한다. (this가 공중에 있는 플레이어)
        if (this.transform.position.y < other.transform.position.y) return;

        BoxCollider otherCollider = this._enemyCharacter.CharacterCollider;
        // 플레이어가 적의 머리 위의 포지션이 아니라면 실행하지 않음
        if(this.transform.position.y + this.CharacterCollider.center.y - this.CharacterCollider.size.y * 0.5f 
           < other.transform.position.y + otherCollider.center.y + otherCollider.size.y * 0.5f - 0.05f) return;
        
        // 공중에 있는 플레이어가 땅에 있는 적과의 포지션을 보고 위치를 이동
        if (this.transform.position.x > _enemyCharacter.transform.position.x)
        {
            Vector3 playerPosition = this.transform.position;
            playerPosition.x = _enemyCharacter.transform.position.x + (CharacterCollider.size.x + otherCollider.size.x) * 0.5f;
            this.transform.position = playerPosition;
        }
        else
        {
            Vector3 playerPosition = this.transform.position;
            playerPosition.x = _enemyCharacter.transform.position.x - (CharacterCollider.size.x + otherCollider.size.x) * 0.5f;
            this.transform.position = playerPosition;
        }
    }

    public bool IsInitializedStartMethod { get; private set; } = false;
    void Start()
    {
        SpecialSkillBallInstance = Instantiate(SpecialSkillBall, this.transform);
        SpecialSkillBallInstance.tag = this.tag;
        SpecialSkillBallInstance.GetComponent<ChasingBall>().Target = EnemyObject.transform;

        foreach (var go in this.transform.GetComponentsInChildren<Transform>())
        {
            if(go.CompareTag("ModelRenderer")) _modelObjectsForRender.Add(go.gameObject);
        }
        
        _wall = GameObject.FindWithTag("Wall");
        CharacterCollider = this.GetComponent<BoxCollider>();
        RegisterObserver(GameObject.FindObjectOfType<GameRoundManager>());
        RigidBody = this.GetComponent<Rigidbody>();
        _inputManager = this.GetComponent<CharacterInputManager>();
        _commandProcessor = this.GetComponent<CommandProcessor>();
        _passiveStateManager = this.GetComponent<PassiveStateManager>();
        _enemyCharacter = EnemyObject.GetComponent<PlayerCharacter>();
        
        ComboManagerInstance = new ComboManager(this);

        StateManager = new BehaviorStateManager(this.gameObject, _wall, ComboManagerInstance);
        _stateSimulatorInStoppedFrame = new BehaviorStateSimulator(this.gameObject, _wall, ComboManagerInstance);
        
        IsInitializedStartMethod = true;
        
        Initialize();
    }

    public void SetModelObjectsVisible(bool isVisible)
    {
        foreach (var modelObject in _modelObjectsForRender)
        {
            modelObject.SetActive(isVisible);
        }
    }
    
    public void Initialize()
    {
        BehaviorStateSetInterface stateSet = BehaviorStateSetManager.GetStateSet(
            BehaviorStateSetManager.BehaviorStateSetIndex.Kohaku,
            transform.root.gameObject);
        
        ComboManagerInstance.Initialize(stateSet);
        
        StateManager.Initialize(stateSet);
        _stateSimulatorInStoppedFrame.Initialize(stateSet);
        
        _currentStateManager = StateManager;
        _playerModeManager = new PlayerModeManager(this);
        
        IsEndedPoseAnimation = false;
        
        ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
        _playerModeManager.SetMode(PlayerModeManager.PlayerMode.NormalPlaying);
    }
    
    // Update is called once per frame
    void Update()
    {
        /* PassiveState 구현 후 구현하기
        // parallel 고려
        while (_activatedPassiveStateSet.Count > 0)
        {
            if(!_activatedPassiveStateSet.Remove(0));
        }
        */
        /*
        if (_roundManager.IsGameStopped) return;
        
        DecideBehaviorByInput();
        UpdatePassiveState();
        if(isPaused) return;
        StateManager.UpdateState();
        ComboManagerInstance.UpdateComboManager();
        */
        
        _playerModeManager.Update();
        
        
        // Debug.Log(name + ":" + GetPlayerMode());
        
         //Debug.Log(StateManager.CurrentState.StateName);
         //Debug.Log(gameObject.name + CharacterPositionState);
    }
    
    

    private void FixedUpdate()
    {
        if(RigidBody.useGravity)
            RigidBody.AddForce(0.0f, -10.0f * _addingGravityMultiple, 0.0f);
    }

    // Switch가 아닌 Command Pattern을 써도 좋으나 필요가 있는가
    // 답 : 런타임에서 Unity의 InputManager의 입력을 바꿀 수 있다면 이미 적용된 사항
    
    // 조작 키의 추가를 고려하여 Input만 만들고 Process를 밖으로 빼야 하는가
    // 답 : 솔직히 Input을 적당한 Process를 넣는 방안 중 Enum 말고 적당한 방법이 있는지 모르겠음
    // 참고 : 이렇게 할거면 빠른 응답을 위해 O(1) 정도의 코드를 생각해야 함
    // 추가 답 : 각각의 고유 번호를 가지고 Process에 접근하도록 하면 됨
    // 이의 : 고유 번호를 찾기 위한 속도는 O(LogN) 안됨
    // 대안 : Process에 해시테이블(혹은 배열)처럼 연결하면 된다 (가능)
    // 답 2 : Input 클래스를 따로 만들고 Process를 포함 시키면 된다. 좋을지도..?
    // 상담해보기 (굳이 안해도 될 듯)
    // 나중에 나온 답 : FSM에 직접 전달하자 (사실상 State가 Process를 진행한다)
    private int _indexForReadInArtificialInput = 0;
    
    public void DecideBehaviorByInput()
    {
        if (IsAcceptArtificialInput && ArtificialButtons.Count > 0)
        {
            BehaviorEnumSet.Button artficialInput = ArtificialButtons[_indexForReadInArtificialInput++];
            _inputManager.EnqueueInput(artficialInput);
            if (_indexForReadInArtificialInput == ArtificialButtons.Count) _indexForReadInArtificialInput = 0;
        }
        
        if (IsAcceptArtificialInput && ArtificialButtonsInSameTime.Count > 0)
        {
            for(int index = 0; index < ArtificialButtonsInSameTime.Count; index++)
                _inputManager.EnqueueInput(ArtificialButtonsInSameTime[index]);
        }

        while (!_inputManager.isEmptyInputQueue())
        {
            BehaviorEnumSet.Button input = _inputManager.DequeueInputQueue();
            //_roundManager.EnqueueRoundInput(gameObject.tag, input, FrameManager.CurrentFrame);
            _commandProcessor.EnqueueInput(input);

            BehaviorEnumSet.Behavior nextBehavior = BehaviorEnumSet.Behavior.Null;
            switch (input)
            {
                case BehaviorEnumSet.Button.Stand:
                    nextBehavior = BehaviorEnumSet.Behavior.Stand;
                    break;
                case BehaviorEnumSet.Button.Crouch:
                    nextBehavior = BehaviorEnumSet.Behavior.Crouch;
                    break;
                case BehaviorEnumSet.Button.Jump:
                    nextBehavior = BehaviorEnumSet.Behavior.Jump;
                    break;
                case BehaviorEnumSet.Button.Forward:
                    nextBehavior = BehaviorEnumSet.Behavior.Forward;
                    break;
                case BehaviorEnumSet.Button.Backward:
                    // backward State에 일임하기
                    if (PlayerStateCheckingMethodSet.IsAttackState(_enemyCharacter.StateManager.CurrentState.StateName))
                        nextBehavior = BehaviorEnumSet.Behavior.Guard;
                    else
                        nextBehavior = BehaviorEnumSet.Behavior.Backward;
                    break;
                case BehaviorEnumSet.Button.Stop:
                    nextBehavior = BehaviorEnumSet.Behavior.Stop;
                    break;
                case BehaviorEnumSet.Button.Punch:
                    nextBehavior = BehaviorEnumSet.Behavior.Punch;
                    break;
                case BehaviorEnumSet.Button.Kick:
                    nextBehavior = BehaviorEnumSet.Behavior.Kick;
                    break;
                case BehaviorEnumSet.Button.Grab:
                    nextBehavior = BehaviorEnumSet.Behavior.Grab;
                    break;
                case BehaviorEnumSet.Button.Guard:
                    nextBehavior = BehaviorEnumSet.Behavior.Guard;
                    break;
                default:
                    Debug.Log("No Input Bug");
                    break;
            }
            BehaviorEnumSet.Behavior commandBehavior 
                = _commandProcessor.JudgeCommand(nextBehavior, CurrentCharacterPositionState);
            nextBehavior = (commandBehavior == BehaviorEnumSet.Behavior.Null) ? nextBehavior : commandBehavior;
            _currentStateManager.HandleInput(nextBehavior);
            /*
            if (!isPaused) StateManager.HandleInput(nextBehavior);
            else if (isPaused) _stateSimulatorInStoppedFrame.HandleInput(nextBehavior);
            */
        }
        // Debug.Log(inputCount);
        
        //Debug.Log(_rigidbody.velocity);
    }
    
    public void DecreaseHp(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            IsEndedPoseAnimation = true;
            StateManager.ChangeState(BehaviorEnumSet.State.InAirHit);
            Notify();
        }
    }

    // this function provide auto setting position and rigidbody
    public void ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState state, bool force = false)
    {
        //if (state == CharacterPositionState && !force) return;
        
        Vector3 characterPosition = Vector3.zero;
        switch (state)
        {
            case PassiveStateEnumSet.CharacterPositionState.OnGround:
                RigidBody.useGravity = false;
                RigidBody.constraints =
                    RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                characterPosition = this.transform.position;
                characterPosition.y = 0.05f;
                this.transform.position = characterPosition;

                CurrentCharacterPositionState = PassiveStateEnumSet.CharacterPositionState.OnGround;
                break;
            case PassiveStateEnumSet.CharacterPositionState.Crouch:
                RigidBody.useGravity = false;
                RigidBody.constraints =
                    RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                characterPosition = this.transform.position;
                characterPosition.y = -0.4f;
                this.transform.position = characterPosition;
                
                CurrentCharacterPositionState = PassiveStateEnumSet.CharacterPositionState.Crouch;
                break;
            case PassiveStateEnumSet.CharacterPositionState.InAir:
                RigidBody.useGravity = true;
                RigidBody.constraints =
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;

                CurrentCharacterPositionState = PassiveStateEnumSet.CharacterPositionState.InAir;
                break;
        }
    }

    private PlayerModeManager.PlayerMode _previousMode;
    public void StopFrame()
    {
        _previousMode = _playerModeManager.GetCurrentModeName();
        _playerModeManager.SetMode(PlayerModeManager.PlayerMode.FramePause);
        _currentStateManager = _stateSimulatorInStoppedFrame;
        if(CurrentCharacterPositionState == PassiveStateEnumSet.CharacterPositionState.InAir)
            _stateSimulatorInStoppedFrame.ChangeState(BehaviorEnumSet.State.InAirIdle);
        else
            _stateSimulatorInStoppedFrame.ChangeState(BehaviorEnumSet.State.StandingIdle);
    }
    
    public void ResumeFrame()
    {
        SetPlayerMode(_previousMode);
        _currentStateManager = StateManager;
        BehaviorEnumSet.State previousInputState = _stateSimulatorInStoppedFrame.CurrentState.StateName;
        if (previousInputState == BehaviorEnumSet.State.CrouchGuard || previousInputState == BehaviorEnumSet.State.StandingGuard)
            return;
        
        if (previousInputState != BehaviorEnumSet.State.StandingIdle &&
            previousInputState != BehaviorEnumSet.State.InAirIdle)
            if (ComboManagerInstance.CheckStateTransition(StateManager.CurrentState,
                    _stateSimulatorInStoppedFrame.CurrentState))
            {
                ComboManagerInstance.CountStateCancel(_stateSimulatorInStoppedFrame.CurrentState.StateName);
                StateManager.ChangeState(_stateSimulatorInStoppedFrame.CurrentState.StateName);
            }
    }
    
    public void SetPlayerMode(PlayerModeManager.PlayerMode mode)
    {
        _playerModeManager.SetMode(mode);
    }

    public PlayerModeManager.PlayerMode GetPlayerMode()
    {
        return _playerModeManager.GetCurrentModeName();
    }
    
    public void ResetHp()
    {
        Hp = 250;
    }
    
    public void LookAtEnemy()
    {
        float forwardDirection = this.transform.forward.x;
        float positionDirection = EnemyObject.transform.position.x - this.transform.position.x;
        
        if(forwardDirection * positionDirection < 0.0f) this.transform.Rotate(Vector3.up, 180.0f);
    }

    public void UpdatePassiveState()
    {
        _passiveStateManager.UpdatePassiveState();
    }
}