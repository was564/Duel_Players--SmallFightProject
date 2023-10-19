using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.SkillState
{
    public class StandingPunchSkill : SkillStateInterface
    {
        public StandingPunchSkill(GameObject characterRoot)
            : base(BehaviorEnumSet.State.StandingPunchSkill, characterRoot, BehaviorEnumSet.AttackLevel.Technique)
        {
            Command = new List<BehaviorEnumSet.Button>()
            {
                BehaviorEnumSet.Button.Crouch, 
                BehaviorEnumSet.Button.Forward, 
                BehaviorEnumSet.Button.Punch
            };
            CommandManager.AddCommand(Command, BehaviorEnumSet.Behavior.StandingPunchSkill);
        }

        public override List<BehaviorEnumSet.Button> Command { get; protected set; }
        
        public override void Enter()
        {
            CharacterAnimator.PlayAnimation("StandingPunchSkill", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingPunchSkill", CharacterAnimator.Layer.LowerLayer,true);
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
            if(CharacterAnimator.IsEndCurrentAnimation("StandingPunchSkill", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            
        }
    }
}