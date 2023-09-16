using System;
using System.Collections;
using System.Collections.Generic;
using Character.CharacterFSM;
using Character.CharacterPassive;
using UnityEngine;

public class CharacterStructure : MonoBehaviour
{
    private CharacterInputManager _inputManager;
    // 여기서 animator를 조종하지 않도록 하자
    // character가 조작할 수 있는 것은 FSM
    private CharacterAnimator _animator;
    private CommandProcessor _commandProcessor;

    private BehaviorStateManager _BehaviorstateManager;

    /* 상담 후 구현하기
    // priorityQueue 대체 할만한 자료구조 (Key값을 바꿀 수 있고 정렬해주는 자료구조) 
    // key-read-only
    // 버프 디버프같은 것을 가지게 하고 남는 시간에 따라 정렬할 수 있는 큐
    // 남는 시간이 끝나면 Dequeue, 버프나 디버프가 추가되면 Enqueue
    // 제안1 : PriorityQueue를 구현
    // 제안2 : 버프나 디버프 마다 Enum으로 요소 위치를 지정 후 HashTable 처럼 관리
    // 문제 : 새로운 효과가 추가되면 Enum을 추가하며 캐릭터는 시작할 때 모든 효과를 가지고 있는다. (Disable, Enable)
    private SortedDictionary<float, PassiveStateInterface> _activatedPassiveStateSet
        = new SortedDictionary<float, PassiveStateInterface>();
        
    public void ActivatePassiveState(PassiveStateInterface state)
    {
        _activatedPassiveStateSet.Add(state.RemainTime, state);
    }
    */
    
    // Start is called before the first frame update
    void Start()
    {
        _inputManager = this.GetComponent<CharacterInputManager>();
        _animator = this.GetComponent<CharacterAnimator>();
        _commandProcessor = this.GetComponent<CommandProcessor>();
        
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
        BehaviorByInput();
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
    public void BehaviorByInput() 
    {
        while (true)
        {
            if (_inputManager.isEmptyQueue()) return;
            Behavior.Button input = _inputManager.DequeueInputQueue();
            _commandProcessor.EnqueueInput(input, Time.time);
            // _commandProcessor.JudgeCommand();
            
            
            switch (input)
            {
                case Behavior.Button.Idle:
                    break;
                case Behavior.Button.Crouch:
                    break;
                case Behavior.Button.Jump:
                    break;
                case Behavior.Button.Right:
                    break;
                case Behavior.Button.Left:
                    break;
                case Behavior.Button.Punch:
                    Behavior.AttackName attackName = JudgeAttackNameOnlyPunch();
                    // animator는 FSM을 통해 움직이게 하기 (여기는 FSM 구현하기)
                    _animator.animateByAttackNameInBehavior(attackName);
                    break;
            }
        }
    }
    
    // 해당 메소드는 차후 CommandProcessor로 옮길 예정
    private Behavior.AttackName JudgeAttackNameOnlyPunch()
    {
        return Behavior.AttackName.Punch;
    }
}

public class Behavior
{
    public enum Button{
        Idle = 0,
        Punch,
        Right,
        Left,
        Jump,
        Crouch,
        Size
    }

    public enum AttackName
    {
        Punch = 0,
        Size
    }
}