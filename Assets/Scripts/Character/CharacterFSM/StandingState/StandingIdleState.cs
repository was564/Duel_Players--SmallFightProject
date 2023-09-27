using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingIdleState : BehaviorStateInterface
    {
        public StandingIdleState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingIdle, characterRoot) {}
        
        public override void Enter()
        {
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle");
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Idle:
                    break;
                case BehaviorEnumSet.Behavior.Punch:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunch);
                    break;
                case BehaviorEnumSet.Behavior.Crouch:
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    StateManager.ChangeState(BehaviorEnumSet.State.Jump);
                    break;
                case BehaviorEnumSet.Behavior.Backward:
                    StateManager.ChangeState(BehaviorEnumSet.State.Backward);
                    break;
                case BehaviorEnumSet.Behavior.Forward:
                    StateManager.ChangeState(BehaviorEnumSet.State.Forward);
                    break;
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            
        }

        public override void Quit()
        {
            
        }
    } 
}