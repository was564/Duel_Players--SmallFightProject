using UnityEngine;

namespace Character.CharacterFSM
{
    public class WalkingForwardState : BehaviorStateInterface
    {
        private float _walkingVelocity = 2.5f;

        private Vector3 _finalVelocity;
        
        public WalkingForwardState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.Forward, characterRoot, BehaviorEnumSet.AttackLevel.Move) {}
        
        public override void Enter()
        {
            _finalVelocity = (CharacterTransform.transform.forward.x > 0.0f)
                    ? (Vector3.right * _walkingVelocity)
                    : (Vector3.left * _walkingVelocity);
            CharacterRigidBody.velocity = _finalVelocity;
            CharacterAnimator.PlayAnimationSmoothly("WalkForward", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("WalkForward", CharacterAnimator.Layer.LowerLayer);
        }
    
        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stop:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
                    break;
                case BehaviorEnumSet.Behavior.Punch:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunch);
                    break;
                case BehaviorEnumSet.Behavior.Kick:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingKick);
                    break;
                case BehaviorEnumSet.Behavior.Crouch:
                    StateManager.ChangeState(BehaviorEnumSet.State.CrouchIdle);
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    StateManager.ChangeState(BehaviorEnumSet.State.Jump);
                    break;
                case BehaviorEnumSet.Behavior.Backward:
                    StateManager.ChangeState(BehaviorEnumSet.State.Backward);
                    break;
                case BehaviorEnumSet.Behavior.StandingPunchSkill:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunchSkill);
                    break;
                default:
                    break;
            }
        }
    
        public override void UpdateState()
        {
            CharacterRigidBody.velocity = _finalVelocity;
        }

        public override void Quit()
        {
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
        }
    }
}