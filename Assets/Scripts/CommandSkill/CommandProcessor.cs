using System.Collections.Generic;
using CommandSkill;
using UnityEngine;


// MonoBehaviour 굳이 필요한가 나중에 구현할 때 판단하기
public class CommandProcessor : MonoBehaviour
{
    // Keyvaluepair는 Allocation 속도는 빠르나 동작 속도가 느리다.
    // 반면 Tuple은 Allocation 속도가 느리나 동작 속도가 빠르다.
    // 참고 : https://codingcoding.tistory.com/206

    public int SkillInputAcknowledgeFrame = 12;
    public int MoveInputAcknowledgeFrame = 18;
    
    private Queue<BehaviorEnumSet.Button> _inputQueue = new Queue<BehaviorEnumSet.Button>();

    
    // 배열로 bool 타입을 넣어 만드는게 빠를까? HashSet으로 만드는게 빠를까?
    // HashSet : 가변적인 데이터 크기, List : 데이터 지역성
    // 빠른 거는 잘 모르겠지만 List로 만드는 것이 코드가 직관적이기 때문에 List로 구현
    /*
    private HashSet<CommandStructure> _unAvailableCommandSet = new HashSet<CommandStructure>();
    private HashSet<CommandStructure> _availableCommandSet = new HashSet<CommandStructure>();
    
    private Stack<CommandStructure> _commandSetForSwitchingAvailable = new Stack<CommandStructure>();
    */
    
    private List<CommandStructure> _characterCommandSet = new List<CommandStructure>();
    
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
        
        
        
        foreach (var command in _characterCommandSet)
        {
            if (command.IsAvailable == true) continue;
            
            if(command.Depth >= command.Command.Count)
            {
                command.IsAvailable = true;
            }
            else if (command.Command[command.Depth].Condition == resultInput)
            {
                if (command.Depth == 0) command.InputStartingFrame = FrameManager.CurrentFrame;
                command.Depth += 1;
            }
            
            int inputAcknowledgeFrame = (command.AttackTrigger == BehaviorEnumSet.Behavior.Null)
                ? MoveInputAcknowledgeFrame
                : SkillInputAcknowledgeFrame;
            if (FrameManager.CurrentFrame - command.InputStartingFrame >= inputAcknowledgeFrame)
            {
                command.Depth = 0;
                command.IsAvailable = false;
            }
        }
        
        foreach (var command in _characterCommandSet)
        {
            if (command.IsAvailable == false) continue;
            
            float inputAcknowledgeTime = (command.AttackTrigger == BehaviorEnumSet.Behavior.Null)
                ? MoveInputAcknowledgeFrame
                : SkillInputAcknowledgeFrame;
            if (FrameManager.CurrentFrame - command.InputStartingFrame > inputAcknowledgeTime)
            {
                command.Depth = 0;
                command.IsAvailable = false;
            }
        }

        /*
        foreach (var command in _commandForMovingState)
        {
            _availableCommandSet.Remove(command);
        }
        _commandForMovingState.Clear();
        */
    }

    public BehaviorEnumSet.Behavior JudgeCommand(
        BehaviorEnumSet.Behavior attack, 
        PassiveStateEnumSet.CharacterPositionState positionState)
    {
        CommandStructure result = null;
        foreach (var command in _characterCommandSet)
        {
            if (command.IsAvailable == false) continue;
            
            if (command.AvailableCommandPositionState.Contains(positionState) == false)
                continue;
            if (command.AttackTrigger == BehaviorEnumSet.Behavior.Null)
            {
                if(result == null || result.InputStartingFrame > command.InputStartingFrame)
                    result = command;
            }
            if (command.AttackTrigger == attack)
            {
                if(result == null || result.InputStartingFrame > command.InputStartingFrame)
                    result = command;
            }
        }
        
        if (result == null)
            return attack;
        else
        {
            ResetCommandToUnAvailable();
            return result.CommandBehavior;
        }
    }

    public void EnqueueInput(BehaviorEnumSet.Button button)
    {
        _inputQueue.Enqueue(button);
    }

    public void AddCommand(
        List<BehaviorEnumSet.InputSet> command,
        BehaviorEnumSet.Behavior attackTrigger,
        List<PassiveStateEnumSet.CharacterPositionState> availableCommandPositionStates,
        BehaviorEnumSet.Behavior behavior, BehaviorEnumSet.AttackLevel attackLevel = BehaviorEnumSet.AttackLevel.Technique)
    {
        CommandStructure newCommand = new CommandStructure(command, attackTrigger, availableCommandPositionStates, behavior);
        
        if(attackLevel == BehaviorEnumSet.AttackLevel.SpecialMove)
            _characterCommandSet.Insert(0, newCommand);
        else if (attackLevel == BehaviorEnumSet.AttackLevel.Technique)
            _characterCommandSet.Add(newCommand);
    }
    
    public void ResetCommandToUnAvailable()
    {
        foreach (var command in _characterCommandSet)
        {
            command.IsAvailable = false;
        }
    }
    /*
    
    public BehaviorEnumSet.Behavior JudgeCommand()
    {
        return _commandTree.JudgeCommand(_inputUntilAcknowledgeTimeList);
    }
    */
}

