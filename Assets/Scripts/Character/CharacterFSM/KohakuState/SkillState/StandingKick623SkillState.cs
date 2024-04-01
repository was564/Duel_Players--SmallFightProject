using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.KohakuState.SkillState
{
    public class StandingKick623SkillState : SkillStateInterface
    {
        public StandingKick623SkillState(GameObject characterRoot)
            : base(BehaviorEnumSet.State.StandingKick236Skill, characterRoot, 
                BehaviorEnumSet.AttackLevel.Technique, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            AttackTrigger = BehaviorEnumSet.Behavior.Kick;
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.Crouch);
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.OnGround);
            MoveCommand = new List<BehaviorEnumSet.InputSet>()
            {
                BehaviorEnumSet.InputSet.Forward,
                BehaviorEnumSet.InputSet.Down,
                BehaviorEnumSet.InputSet.Forward
            };
            CommandManager.AddCommand(
                MoveCommand, 
                AttackTrigger, 
                AvailableCommandPositionCondition,
                BehaviorEnumSet.Behavior.StandingKick623Skill);
            
            MoveCommand = new List<BehaviorEnumSet.InputSet>()
            {
                BehaviorEnumSet.InputSet.Forward,
                BehaviorEnumSet.InputSet.Down,
                BehaviorEnumSet.InputSet.ForwardDown
            };
            CommandManager.AddCommand(
                MoveCommand, 
                AttackTrigger, 
                AvailableCommandPositionCondition,
                BehaviorEnumSet.Behavior.StandingKick623Skill);
        }

        public override List<BehaviorEnumSet.InputSet> MoveCommand { get; protected set; }
        public override BehaviorEnumSet.Behavior AttackTrigger { get; protected set; }
        public override List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionCondition { get; protected set; }
            = new List<PassiveStateEnumSet.CharacterPositionState>();

        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterAnimator.PlayAnimation("StandingKick623Skill", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingKick623Skill", CharacterAnimator.Layer.LowerLayer,true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if (CharacterAnimator.IsEndCurrentAnimation("StandingKick623Skill", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingIdle;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.State.StandingKick623Skill);
        }
    }
}