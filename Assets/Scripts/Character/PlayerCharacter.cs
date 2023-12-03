using System;
using System.Collections.Generic;
using Character;
using Character.CharacterFSM;
using Character.CharacterPassiveState;
using UnityEngine;

public class PlayerCharacter : MonoPublisherInterface
{
    [SerializeField] public GameObject EnemyObject;

    private BehaviorStateSimulator _enemyStateManager;
    
    private Rigidbody _rigidbody;
    private bool _isLookingRight;
    
    private CharacterInputManager _inputManager;
    private CommandProcessor _commandProcessor;
    private PassiveStateManager _passiveStateManager;
    private CharacterAnimator _characterAnimator;
    
    public BehaviorStateManager StateManager { get; private set; }
    private BehaviorStateSimulator _stateSimulatorInStoppedFrame;
    public ComboManager ComboManagerInstance { get; private set; }
    
    public float PositionYOffsetForLand { get; private set; } = -0.6f;

    private bool isPause = false;
    
    public PassiveStateEnumSet.CharacterPositionState CharacterPositionState { get; set; }
        = PassiveStateEnumSet.CharacterPositionState.Size;
    
    public int Hp { get; private set; } = 100;

    public bool IsHitContinuous { get; set; } = false;

    public bool IsAcceptArtificialInput = false;

    public List<BehaviorEnumSet.Button> ArtificialButtons = new List<BehaviorEnumSet.Button>();

    private float _addingGravityMultiple = 1.3f;

    public int PlayerUniqueIndex { get; set; }

    private List<MonoObserverInterface> _observers = new List<MonoObserverInterface>();
    
    private void Awake()
    {
        ComboManagerInstance = new ComboManager(this);
        StateManager = new BehaviorStateManager(this.gameObject, ComboManagerInstance);
        _stateSimulatorInStoppedFrame = new BehaviorStateSimulator(this.gameObject, ComboManagerInstance);
    }

    void Start()
    {
        RegisterObserver(GameObject.FindObjectOfType<GameRoundManager>());
        _rigidbody = this.GetComponent<Rigidbody>();
        _inputManager = this.GetComponent<CharacterInputManager>();
        _commandProcessor = this.GetComponent<CommandProcessor>();
        _passiveStateManager = this.GetComponent<PassiveStateManager>();
        _characterAnimator = this.GetComponent<CharacterAnimator>();

        _enemyStateManager = EnemyObject.GetComponent<PlayerCharacter>().StateManager;
        
        ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
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
        
        DecideBehaviorByInput();
        _passiveStateManager.UpdatePassiveState();
        if(isPause) return;
        StateManager.UpdateState();
        ComboManagerInstance.UpdateComboManager();
    }

    private void FixedUpdate()
    {
        if(_rigidbody.useGravity)
            _rigidbody.AddForce(0.0f, -10.0f * _addingGravityMultiple, 0.0f);
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
    public void DecideBehaviorByInput()
    {
        if (IsAcceptArtificialInput)
            foreach (var button in ArtificialButtons)
            {
                _inputManager.EnqueueInputQueue(button);
            }
        while (!_inputManager.isEmptyInputQueue())
        {
            BehaviorEnumSet.Button input = _inputManager.DequeueInputQueue();
            // 나중에 삭제해도 되는 코드
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
                    if ((int)_enemyStateManager.CurrentState.AttackLevel >= (int)BehaviorEnumSet.AttackLevel.BasicAttack &&
                        (int)_enemyStateManager.CurrentState.AttackLevel < (int)BehaviorEnumSet.AttackLevel.Hit)
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
                case BehaviorEnumSet.Button.Guard:
                    nextBehavior = BehaviorEnumSet.Behavior.Guard;
                    break;
                default:
                    Debug.Log("No Input Bug");
                    break;
            }
            BehaviorEnumSet.Behavior commandBehavior 
                = _commandProcessor.JudgeCommand(nextBehavior, CharacterPositionState);
            nextBehavior = (commandBehavior == BehaviorEnumSet.Behavior.Null) ? nextBehavior : commandBehavior;
            if (!isPause) StateManager.HandleInput(nextBehavior);
            else if (isPause) _stateSimulatorInStoppedFrame.HandleInput(nextBehavior);
        }
        // Debug.Log(inputCount);
        
        //Debug.Log(_rigidbody.velocity);
    }
    
    public void DecreaseHp(int damage)
    {
        Hp -= damage;
        Notify();
    }

    // this function provide auto setting position and rigidbody
    public void ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState state, bool force = false)
    {
        //if (state == CharacterPositionState && !force) return;
        
        Vector3 characterPosition = Vector3.zero;
        switch (state)
        {
            case PassiveStateEnumSet.CharacterPositionState.OnGround:
                _rigidbody.useGravity = false;
                _rigidbody.constraints =
                    RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                characterPosition = this.transform.position;
                characterPosition.y = 0.05f;
                this.transform.position = characterPosition;

                CharacterPositionState = PassiveStateEnumSet.CharacterPositionState.OnGround;
                break;
            case PassiveStateEnumSet.CharacterPositionState.Crouch:
                _rigidbody.constraints =
                    RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;
                characterPosition = this.transform.position;
                characterPosition.y = -0.4f;
                this.transform.position = characterPosition;

                break;
            case PassiveStateEnumSet.CharacterPositionState.InAir:
                _rigidbody.useGravity = true;
                _rigidbody.constraints =
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotation;

                CharacterPositionState = PassiveStateEnumSet.CharacterPositionState.InAir;
                break;
        }
    }

    public void Stop()
    {
        isPause = true;
        if(CharacterPositionState == PassiveStateEnumSet.CharacterPositionState.InAir)
            _stateSimulatorInStoppedFrame.ChangeState(BehaviorEnumSet.State.InAirIdle);
        else
            _stateSimulatorInStoppedFrame.ChangeState(BehaviorEnumSet.State.StandingIdle);
    }

    public void Resume()
    {
        isPause = false;
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

    public void ResetHp()
    {
        Hp = 100;
    }
    
    public void LookAtEnemy()
    {
        float forwardDirection = this.transform.forward.x;
        float positionDirection = EnemyObject.transform.position.x - this.transform.position.x;
        
        if(forwardDirection * positionDirection < 0.0f) this.transform.Rotate(Vector3.up, 180.0f);
    }
    
}