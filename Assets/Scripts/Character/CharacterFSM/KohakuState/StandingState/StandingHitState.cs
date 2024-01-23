using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class StandingHitState : BehaviorStateInterface
    {
        public StandingHitState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingHit, characterRoot, 
                BehaviorEnumSet.AttackLevel.Hit, PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            //CharacterRigidBody.velocity = Vector3.left * ((CharacterTransform.forward.x < 0.0f ? -1.0f : 1.0f) * _backMoveSpeedByAttack);

            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("StandingIdle", CharacterAnimator.Layer.LowerLayer, true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
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