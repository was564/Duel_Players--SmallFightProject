using UnityEngine;
using UnityEngine.Assertions;

namespace Character.CharacterFSM
{
    public class StandingPunchState : PunchState
    {
        public StandingPunchState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingPunch, characterRoot, BehaviorEnumSet.State.StandingIdle) {}
        
        public override void Enter()
        {
            base.Enter();
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
            
            CharacterRigidBody.velocity = Vector3.zero;
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
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
            base.UpdateState();
        }

        public override void Quit()
        {
            
        }
    }
}