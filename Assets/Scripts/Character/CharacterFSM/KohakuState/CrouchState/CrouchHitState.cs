using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class CrouchHitState : HitState
    {
        public CrouchHitState(GameObject characterRoot) : 
            base(characterRoot, BehaviorEnumSet.State.CrouchHit, PassiveStateEnumSet.CharacterPositionState.Crouch) {}
        
        private float _backMoveSpeedByAttack = 2.0f;
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterRigidBody.velocity = Vector3.left * ((CharacterTransform.forward.x < 0.0f ? -1.0f : 1.0f) * _backMoveSpeedByAttack);

            
            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("CrouchStop", CharacterAnimator.Layer.LowerLayer, true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            return base.GetResultStateByHandleInput(behavior);
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if(CharacterAnimator.IsEndCurrentAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.CrouchIdle;
            
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}