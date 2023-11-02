using UnityEngine;

namespace Character.CharacterFSM
{
    public class WalkingBackwardState : BehaviorStateInterface
    {
        private float _walkingVelocity = 2.5f;

        private Vector3 _finalVelocity;
        
        public WalkingBackwardState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.Backward, characterRoot, BehaviorEnumSet.AttackLevel.Move) {}
        
        public override void Enter()
        {
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
            _finalVelocity = (CharacterTransform.transform.forward.x < 0.0f)
                    ? (Vector3.right * _walkingVelocity)
                    : (Vector3.left * _walkingVelocity);
            CharacterRigidBody.velocity = _finalVelocity;
            CharacterAnimator.PlayAnimationSmoothly("WalkBackward", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("WalkBackward", CharacterAnimator.Layer.LowerLayer);
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
                case BehaviorEnumSet.Behavior.Forward:
                    StateManager.ChangeState(BehaviorEnumSet.State.Forward);
                    break;
                case BehaviorEnumSet.Behavior.StandingPunchSkill:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunchSkill);
                    break;
                case BehaviorEnumSet.Behavior.Guard:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingGuard);
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