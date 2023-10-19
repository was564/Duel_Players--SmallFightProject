using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


// MonoBehaviour 굳이 필요한가 나중에 구현할 때 판단하기
public class CommandProcessor : MonoBehaviour
{
    // Keyvaluepair는 Allocation 속도는 빠르나 동작 속도가 느리다.
    // 반면 Tuple은 Allocation 속도가 느리나 동작 속도가 빠르다.
    // 참고 : https://codingcoding.tistory.com/206

    public float InputAcknowledgeTime = 1.0f;
    // Queue -> LinkedList (중간에 값을 알지 못함)
    private LinkedList<InputAndTimeTuple> _inputUntilAcknowledgeTimeList;
    private Queue<InputAndTimeTuple> _unusedTupleSet;

    private List<KeyValuePair<BehaviorEnumSet.Behavior, List<BehaviorEnumSet.Button>>> _commandList
        = new List<KeyValuePair<BehaviorEnumSet.Behavior, List<BehaviorEnumSet.Button>>>();
    
    // private CommandTree _commandTree = new CommandTree();
    
    // Start is called before the first frame update
    // queue parallel 가능??
    // 예상 : peek 노드가 바뀔 수 있음 (어떻게 구현했는지 따라) (의도에서는 문제가 안됨)
    // 궁금한 점 : Allocation할 때 운영체제에서는 스레드락을 거는가?
    // 보통 그럴 일은 없다고 함
    void Start()
    { 
        int maxInputCountPerSecond = 60 * (int)BehaviorEnumSet.Button.Size;
        
        _unusedTupleSet = new Queue<InputAndTimeTuple>();
        for (int i = 0; i < InputAcknowledgeTime * maxInputCountPerSecond; i++) 
            _unusedTupleSet.Enqueue(new InputAndTimeTuple(BehaviorEnumSet.Button.Stop, 0.0f));
        _inputUntilAcknowledgeTimeList = new LinkedList<InputAndTimeTuple>();

        /*
        // List<Behavior>에서 공격(Trigger)이 마지막 요소
        foreach (KeyValuePair<BehaviorEnumSet.Behavior, List<BehaviorEnumSet.Button>> commandInfo in _commandList)
        {
            _commandTree.AddCommand(commandInfo.Value, BehaviorEnumSet.Behavior.StandingPunchSkill);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        while (_inputUntilAcknowledgeTimeList.Count > 0)
        {
            var peekNode = _inputUntilAcknowledgeTimeList.Last();
            if (Time.time - peekNode.Time >= InputAcknowledgeTime)
            {
                _unusedTupleSet.Enqueue(_inputUntilAcknowledgeTimeList.Last());
                _inputUntilAcknowledgeTimeList.RemoveFirst();
            }
            else break;
        }
    }

    public void EnqueueInput(BehaviorEnumSet.Button button, float time)
    {
        if (_inputUntilAcknowledgeTimeList.Count > 0 && button == _inputUntilAcknowledgeTimeList.Last.Value.Input) return;
        InputAndTimeTuple input = _unusedTupleSet.Dequeue();
        input.ReInitValues(button, time);
        _inputUntilAcknowledgeTimeList.AddLast(input);
    }

    public void AddCommand(List<BehaviorEnumSet.Button> command, BehaviorEnumSet.Behavior behavior)
    {
        _commandList.Add(new KeyValuePair<BehaviorEnumSet.Behavior, List<BehaviorEnumSet.Button>>(behavior, command));
    }
    /*
    
    public BehaviorEnumSet.Behavior JudgeCommand()
    {
        return _commandTree.JudgeCommand(_inputUntilAcknowledgeTimeList);
    }
    */
    // struct는 데이터가 스택에 할당되기 때문에 더 빠르다.
    // Reference : https://mdfarragher.medium.com/whats-faster-in-c-a-struct-or-a-class-99e4761a7b76
    // 해당 구조체 속도는 모르겠음..
    // 추후 개선할 수도 있는 사항은 time대신 Frame수를 이용해보기 (float -> int)
    // Generic + Class WritablePair 고려하기 상담
    public class InputAndTimeTuple {
    
        public BehaviorEnumSet.Button Input { get; private set; }
        public float Time { get; private set; }
        public InputAndTimeTuple(BehaviorEnumSet.Button button, float time)
        {
            this.Input = button;
            this.Time = time;
        }

        public void ReInitValues(BehaviorEnumSet.Button button, float time)
        {
            this.Input = button;
            this.Time = time;
        }
    }
}

