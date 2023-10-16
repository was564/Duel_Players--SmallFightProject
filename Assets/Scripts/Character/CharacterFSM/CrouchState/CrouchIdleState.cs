using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchIdleState : BehaviorStateInterface
    {
        public CrouchIdleState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.CrouchIdle, characterRoot) {}
        
        public override void Enter()
        {
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("Crouch", CharacterAnimator.Layer.LowerLayer);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Idle:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    StateManager.ChangeState(BehaviorEnumSet.State.Jump);
                    break;
                case BehaviorEnumSet.Behavior.Punch:
                    break;
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            Vector3 characterPosition = this.CharacterTransform.position;
                
            float positionY = Mathf.Lerp(
                0, 
                -2f,
                0.2f);
            characterPosition.y = positionY;
            this.CharacterTransform.position = characterPosition;
        }

        public override void Quit()
        {
            Vector3 characterPosition = this.CharacterTransform.position;
            characterPosition.y = 0;
            this.CharacterTransform.position = characterPosition;
        }
    }
}