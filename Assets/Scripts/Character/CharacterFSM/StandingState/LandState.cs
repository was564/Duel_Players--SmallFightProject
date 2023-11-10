using UnityEngine;

namespace Character.CharacterFSM
{
    public class LandState : BehaviorStateInterface
    {
        public LandState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.Land, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.Move) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
            
            CharacterRigidBody.velocity = Vector3.zero;
            
            CharacterAnimator.PlayAnimationSmoothly("Land", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("Land", CharacterAnimator.Layer.LowerLayer);
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
            if(CharacterAnimator.IsEndCurrentAnimation("Land", CharacterAnimator.Layer.LowerLayer))
                StateManager.ForceChangeState(BehaviorEnumSet.State.StandingIdle);
            else
            {
                Vector3 characterPosition = this.CharacterTransform.position;
                
                float positionY = Mathf.Lerp(
                    this.PlayerCharacter.PositionYOffsetForLand, 
                    0.0f, 
                    CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.LowerLayer));
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