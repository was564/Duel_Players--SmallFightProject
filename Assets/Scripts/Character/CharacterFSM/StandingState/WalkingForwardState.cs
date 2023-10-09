using UnityEngine;

namespace Character.CharacterFSM
{
    public class WalkingForwardState : BehaviorStateInterface
    {
        private float _walkingVelocity = 2.5f;
        
        public WalkingForwardState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.Forward, characterRoot) {}
        
        public override void Enter()
        {
            CharacterRigidBody.velocity =
                (CharacterTransform.transform.forward.x > 0.0f)
                    ? (Vector3.right * _walkingVelocity)
                    : (Vector3.left * _walkingVelocity);
            // CharacterAnimator.Play("WalkForward");
            CharacterAnimator.PlayAnimationSmoothly("WalkForward");
        }
    
        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Idle:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
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