using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class StandingHitState : HitState
    {
        public StandingHitState(GameObject characterRoot) : 
            base(characterRoot, BehaviorEnumSet.State.StandingHit, PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        
        public override void Enter()
        {
            base.Enter();
            
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            //CharacterRigidBody.velocity = Vector3.left * ((CharacterTransform.forward.x < 0.0f ? -1.0f : 1.0f) * _backMoveSpeedByAttack);

            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.LowerLayer, true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            return base.GetResultStateByHandleInput(behavior);
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if (CharacterAnimator.IsEndCurrentAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingIdle;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}