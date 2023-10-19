using System;
using System.Collections;
using System.Collections.Generic;
using Character.CharacterFSM;
using Character.CharacterPassiveState;
using UnityEngine;

public class CharacterStructure : MonoBehaviour
{
    private CharacterInputManager _inputManager;
    
    private CommandProcessor _commandProcessor;

    private BehaviorStateManager _behaviorStateManager;

    private PassiveStateManager _passiveStateManager;
    
    public float PositionYOffsetForLand { get; private set; } = -0.5f;

    public PassiveStateEnumSet.CharacterPositionState CharacterPositionState { get; set; }
        = PassiveStateEnumSet.CharacterPositionState.OnGround;
    
    public int Hp { get; private set; } = 100;

    public bool IsHitContinuous { get; set; } = false;
    
    public void ActivatePassiveState(PassiveStateInterface state)
    {
        state.EnterPassiveState();
    }
    
    void Start()
    {
        _inputManager = this.GetComponent<CharacterInputManager>();
        _commandProcessor = this.GetComponent<CommandProcessor>();
        _behaviorStateManager = this.GetComponent<BehaviorStateManager>();
        _passiveStateManager = this.GetComponent<PassiveStateManager>();
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
        _behaviorStateManager.UpdateState();
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
        while (!_inputManager.isEmptyInputQueue())
        {
            BehaviorEnumSet.Button input = _inputManager.DequeueInputQueue();
            // _commandProcessor.JudgeCommand();

            BehaviorEnumSet.Behavior nextBehavior = BehaviorEnumSet.Behavior.Null;
            switch (input)
            {
                case BehaviorEnumSet.Button.Stand:
                    nextBehavior = BehaviorEnumSet.Behavior.Stand;
                    break;
                case BehaviorEnumSet.Button.Crouch:
                    _commandProcessor.EnqueueInput(input, Time.time);
                    nextBehavior = BehaviorEnumSet.Behavior.Crouch;
                    break;
                case BehaviorEnumSet.Button.Jump:
                    nextBehavior = BehaviorEnumSet.Behavior.Jump;
                    break;
                case BehaviorEnumSet.Button.Forward:
                    _commandProcessor.EnqueueInput(input, Time.time);
                    nextBehavior = BehaviorEnumSet.Behavior.Forward;
                    break;
                case BehaviorEnumSet.Button.Backward:
                    _commandProcessor.EnqueueInput(input, Time.time);
                    nextBehavior = BehaviorEnumSet.Behavior.Backward;
                    break;
                case BehaviorEnumSet.Button.Stop:
                    nextBehavior = BehaviorEnumSet.Behavior.Stop;
                    break;
                case BehaviorEnumSet.Button.Punch:
                    _commandProcessor.EnqueueInput(input, Time.time);
                    nextBehavior = JudgeAttackNameOnlyPunch();
                    // animator는 FSM을 통해 움직이게 하기 (여기는 FSM 구현하기)
                    // _animator.animateByAttackNameInBehavior(attackName);
                    break;
                case BehaviorEnumSet.Button.Kick:
                    nextBehavior = BehaviorEnumSet.Behavior.Kick;
                    break;
                default:
                    Debug.Log("No Input Bug");
                    break;
            }
            _behaviorStateManager.HandleInput(nextBehavior);
        }
        // Debug.Log(inputCount);
    }
    
    // 해당 메소드는 차후 CommandProcessor로 옮길 예정
    private BehaviorEnumSet.Behavior JudgeAttackNameOnlyPunch()
    {
        return BehaviorEnumSet.Behavior.Punch;

        //return _commandProcessor.JudgeCommand();
    }

    public void DecreaseHp(int damage)
    {
        Hp -= damage;
    }
}