using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.SkillState
{
    public class StandingKick623SkillState : SkillStateInterface
    {
        public StandingKick623SkillState(GameObject characterRoot, BehaviorStateSimulator stateManager)
            : base(BehaviorEnumSet.State.StandingKick236Skill, stateManager, characterRoot, 
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
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            CharacterAnimator.PlayAnimation("StandingKick623Skill", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingKick623Skill", CharacterAnimator.Layer.LowerLayer,true);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            if(CharacterAnimator.IsEndCurrentAnimation("StandingKick623Skill", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.StandingKick623Skill);
        }
    }
}