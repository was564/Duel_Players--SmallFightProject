using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CommandProcessor : MonoBehaviour
{
    // Keyvaluepair는 Allocation 속도는 빠르나 동작 속도가 느리다.
    // 반면 Tuple은 Allocation 속도가 느리나 동작 속도가 빠르다.
    // 참고 : https://codingcoding.tistory.com/206

    public float InputAcknowledgeTime = 1.0f;
    // Queue -> LinkedList (중간에 값을 알지 못함)
    private LinkedList<InputAndTimeTuple> _inputUntilAcknowledgeTimeList;
    private Queue<InputAndTimeTuple> _unusedTupleSet;
    
    // Start is called before the first frame update
    void Start()
    {
        int maxInputCountPerSecond = 60 * (int)BehaviorEnumSet.Button.Size;
        // queue parallel 가능??
        // 예상 : peek 노드가 바뀔 수 있음 (어떻게 구현했는지 따라) (의도에서는 문제가 안됨)
        // 궁금한 점 : Allocation할 때 운영체제에서는 스레드락을 거는가?
        _unusedTupleSet = new Queue<InputAndTimeTuple>();
        for (int i = 0; i < InputAcknowledgeTime * maxInputCountPerSecond; i++) 
            _unusedTupleSet.Enqueue(new InputAndTimeTuple(BehaviorEnumSet.Button.Idle, 0.0f));
        _inputUntilAcknowledgeTimeList = new LinkedList<InputAndTimeTuple>();
    }

    // Update is called once per frame
    void Update()
    {
        while (_inputUntilAcknowledgeTimeList.Count > 0)
        {
            _unusedTupleSet.Enqueue(_inputUntilAcknowledgeTimeList.Last());
            _inputUntilAcknowledgeTimeList.RemoveFirst();
        }
    }

    public void EnqueueInput(BehaviorEnumSet.Button button, float time)
    {
        InputAndTimeTuple input = _unusedTupleSet.Dequeue();
        input.ReInitValues(button, time);
        _inputUntilAcknowledgeTimeList.AddLast(input);
    }
}

// struct는 데이터가 스택에 할당되기 때문에 더 빠르다.
// Reference : https://mdfarragher.medium.com/whats-faster-in-c-a-struct-or-a-class-99e4761a7b76
// 해당 구조체 속도는 모르겠음..
// 추후 개선할 수도 있는 사항은 time대신 Frame수를 이용해보기 (float -> int)
// Generic + Class WritablePair 고려하기 상담
struct InputAndTimeTuple {
    
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