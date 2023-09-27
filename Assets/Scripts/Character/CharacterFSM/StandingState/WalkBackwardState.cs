using UnityEngine;

namespace Character.CharacterFSM
{
    public class WalkBackwardState : BehaviorStateInterface
    {
        private float _walkingVelocity = 2.5f;
        
        public WalkBackwardState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.Backward, characterRoot) {}
        
        public override void Enter()
        {
            CharacterRigidBody.velocity =
                (CharacterTransform.transform.forward.x < 0.0f)
                    ? (Vector3.right * _walkingVelocity)
                    : (Vector3.left * _walkingVelocity);
            CharacterAnimator.PlayAnimationSmoothly("WalkBackward");
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
            CharacterRigidBody.velocity = Vector3.zero;
        }
    }
}