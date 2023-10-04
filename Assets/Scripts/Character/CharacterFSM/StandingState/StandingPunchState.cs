using UnityEngine;
using UnityEngine.Assertions;

namespace Character.CharacterFSM
{
    public class StandingPunchState : BehaviorStateInterface
    {
        public StandingPunchState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingPunch, characterRoot) {}
        
        public override void Enter()
        {
            CharacterRigidBody.velocity = Vector3.zero;
            CharacterAnimator.PlayAnimation("StandingPunch", true);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch: 
                    // StateManager.ChangeState(BehaviorEnumSet.State.Punch);
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    break;
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            if(CharacterAnimator.IsEndCurrentAnimation("StandingPunch"))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            
        }
    }
}