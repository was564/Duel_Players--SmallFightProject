using System.Collections.Generic;
using CommandSkill;
using UnityEngine;


// MonoBehaviour 굳이 필요한가 나중에 구현할 때 판단하기
public class CommandProcessor : MonoBehaviour
{
    // Keyvaluepair는 Allocation 속도는 빠르나 동작 속도가 느리다.
    // 반면 Tuple은 Allocation 속도가 느리나 동작 속도가 빠르다.
    // 참고 : https://codingcoding.tistory.com/206

    public float SkillInputAcknowledgeTime = 1.0f;
    public float MoveInputAcknowledgeTime = 0.5f;
    // Queue -> LinkedList (중간에 값을 알지 못함)
    private Queue<BehaviorEnumSet.Button> _inputQueue = new Queue<BehaviorEnumSet.Button>();

    private HashSet<CommandStructure> _unAvailableCommandSet = new HashSet<CommandStructure>();
    private HashSet<CommandStructure> _availableCommandSet = new HashSet<CommandStructure>();

    private List<CommandStructure> _commandForMovingState = new List<CommandStructure>();
    
    // Start is called before the first frame update
    // queue parallel 가능??
    // 예상 : peek 노드가 바뀔 수 있음 (어떻게 구현했는지 따라) (의도에서는 문제가 안됨)
    // 궁금한 점 : Allocation할 때 운영체제에서는 스레드락을 거는가?
    // 보통 그럴 일은 없다고 함
    void Start()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        int resultInputAsInt = (int)BehaviorEnumSet.InputSet.Idle;
        while (_inputQueue.Count > 0)
        {
            BehaviorEnumSet.Button input = _inputQueue.Dequeue();
            if ((int)input >= (int)BehaviorEnumSet.Button.Punch)
                continue;
            switch (input)
            {
                case BehaviorEnumSet.Button.Forward:
                    resultInputAsInt += (int)BehaviorEnumSet.InputSet.Forward;
                    break;
                case BehaviorEnumSet.Button.Backward:
                    resultInputAsInt += (int)BehaviorEnumSet.InputSet.Backward;
                    break;
                case BehaviorEnumSet.Button.Jump:
                    resultInputAsInt += (int)BehaviorEnumSet.InputSet.Up;
                    break;
                case BehaviorEnumSet.Button.Crouch:
                    resultInputAsInt += (int)BehaviorEnumSet.InputSet.Down;
                    break;
            }
        }
        BehaviorEnumSet.InputSet resultInput = (BehaviorEnumSet.InputSet)resultInputAsInt;
        //Debug.Log(resultInput.ToString() + resultInputAsInt.ToString());
        
        foreach (var command in _unAvailableCommandSet)
        {
            if(command.Depth >= command.Command.Count)
            {
                _availableCommandSet.Add(command);
                /*
                if (result == null || result.InputStartingTime > command.InputStartingTime)
                {
                    result = command;
                }
                */
            }
            else if (command.Command[command.Depth].Condition == resultInput)
            {
                if (command.Depth == 0) command.InputStartingTime = Time.time;
                command.Depth += 1;
            }
            
            float inputAcknowledgeTime = (command.AttackTrigger == BehaviorEnumSet.Behavior.Null)
                ? MoveInputAcknowledgeTime
                : SkillInputAcknowledgeTime;
            if (Time.time - command.InputStartingTime > inputAcknowledgeTime)
            {
                command.Depth = 0;
            }
        }
        
        foreach (var command in _availableCommandSet)
        {
            float inputAcknowledgeTime = (command.AttackTrigger == BehaviorEnumSet.Behavior.Null)
                ? MoveInputAcknowledgeTime
                : SkillInputAcknowledgeTime;
            if (Time.time - command.InputStartingTime > inputAcknowledgeTime)
            {
                command.Depth = 0;
                _unAvailableCommandSet.Add(command);
                _commandForMovingState.Add(command);
            }
        }

        foreach (var command in _commandForMovingState)
        {
            _availableCommandSet.Remove(command);
        }
        _commandForMovingState.Clear();
    }

    public BehaviorEnumSet.Behavior JudgeCommand(
        BehaviorEnumSet.Behavior attack, 
        PassiveStateEnumSet.CharacterPositionState positionState)
    {
        CommandStructure result = null;
        foreach (var command in _availableCommandSet)
        {
            if (command.AvailableCommandPositionState.Contains(positionState) == false)
                continue;
            if (command.AttackTrigger == BehaviorEnumSet.Behavior.Null)
            {
                result = command;
            }
            if (command.AttackTrigger == attack)
            {
                if(result == null || result.InputStartingTime > command.InputStartingTime)
                    result = command;
            }
        }
        
        if (result == null)
            return attack;
        
        _unAvailableCommandSet.Add(result);
        result.Depth = 0;
        _availableCommandSet.Remove(result);
        
        return result.CommandBehavior;
    }

    public void EnqueueInput(BehaviorEnumSet.Button button)
    {
        _inputQueue.Enqueue(button);
    }

    public void AddCommand(
        List<BehaviorEnumSet.InputSet> command,
        BehaviorEnumSet.Behavior attackTrigger,
        List<PassiveStateEnumSet.CharacterPositionState> availableCommandPositionStates,
        BehaviorEnumSet.Behavior behavior)
    {
        _unAvailableCommandSet.Add(
            new CommandStructure(command, attackTrigger, availableCommandPositionStates, behavior));
    }
    /*
    
    public BehaviorEnumSet.Behavior JudgeCommand()
    {
        return _commandTree.JudgeCommand(_inputUntilAcknowledgeTimeList);
    }
    */
}

