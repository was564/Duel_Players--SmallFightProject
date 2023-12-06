using System.Collections.Generic;
using Character.CharacterFSM;
using UnityEngine;

public abstract class SkillStateInterface : BehaviorStateInterface
{
    public SkillStateInterface(
        BehaviorEnumSet.State stateName, 
        BehaviorStateSimulator stateManager,
        GameObject characterRoot, 
        BehaviorEnumSet.AttackLevel attackLevel,
        PassiveStateEnumSet.CharacterPositionState positionState)
        : base(stateName, stateManager, characterRoot, attackLevel, positionState)
    {
        CommandManager = characterRoot.GetComponent<CommandProcessor>();
    }

    protected CommandProcessor CommandManager;
    
    public abstract List<BehaviorEnumSet.InputSet> MoveCommand { get; protected set; }
    
    public abstract BehaviorEnumSet.Behavior AttackTrigger { get; protected set; }
    
    public abstract List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionCondition { get; protected set; }
}