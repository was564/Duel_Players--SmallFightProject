using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.KohakuState.SkillState
{
    public abstract class SkillStateInterface : BehaviorStateInterface
    {
        public SkillStateInterface(
            BehaviorEnumSet.State stateName,
            GameObject characterRoot, 
            BehaviorEnumSet.AttackLevel attackLevel,
            PassiveStateEnumSet.CharacterPositionState positionInitialState)
            : base(stateName, characterRoot, attackLevel, positionInitialState)
        {
            CommandManager = characterRoot.GetComponent<CommandProcessor>();
        }

        protected CommandProcessor CommandManager;
    
        public abstract List<BehaviorEnumSet.InputSet> MoveCommand { get; protected set; }
    
        public abstract BehaviorEnumSet.Behavior AttackTrigger { get; protected set; }
    
        public abstract List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionCondition { get; protected set; }
    }
}