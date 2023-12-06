using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchHitState : BehaviorStateInterface
    {
        public CrouchHitState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.CrouchHit, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.Hit, PassiveStateEnumSet.CharacterPositionState.Crouch) {}
        
        private float _backMoveSpeedByAttack = 2.0f;
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            CharacterRigidBody.velocity = Vector3.left * ((CharacterTransform.forward.x < 0.0f ? -1.0f : 1.0f) * _backMoveSpeedByAttack);

            
            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("CrouchStop", CharacterAnimator.Layer.LowerLayer, true);
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
            if(CharacterAnimator.IsEndCurrentAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.CrouchIdle);
        }

        public override void Quit()
        {
            
        }
    }
}