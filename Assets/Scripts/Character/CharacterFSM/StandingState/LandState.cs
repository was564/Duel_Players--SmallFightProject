using UnityEngine;

namespace Character.CharacterFSM
{
    public class LandState : BehaviorStateInterface
    {
        public LandState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.Land, characterRoot) {}

        public override void Enter()
        {

            CharacterRigidBody.useGravity = false;
            CharacterRigidBody.velocity = Vector3.zero;
            Character.InAir = false;
            CharacterAnimator.PlayAnimationSmoothly("Land");
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
            if(CharacterAnimator.IsEndCurrentAnimation("Land"))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
            else
            {
                Vector3 characterPosition = this.CharacterTransform.position;
                
                float positionY = Mathf.Lerp(
                    this.Character.PositionYOffsetForLand, 
                    0.0f, 
                    CharacterAnimator.GetCurrentAnimationDuration());
                characterPosition.y = positionY;
                this.CharacterTransform.position = characterPosition;
            }
        }

        public override void Quit()
        {
            Vector3 characterPosition = this.CharacterTransform.position;
            characterPosition.y = 0;
            this.CharacterTransform.position = characterPosition;
        }
    }
}