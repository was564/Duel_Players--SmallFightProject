using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class InAirHitState : HitState
    {
        public InAirHitState(GameObject characterRoot) : 
            base(characterRoot, BehaviorEnumSet.State.InAirHit, PassiveStateEnumSet.CharacterPositionState.InAir) {}
        
        //private Vector3 _hittedAwayDirection = new Vector3(1.5f, 5.0f, 0);
        
        public override void Enter()
        {
            
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            /*
            Vector3 resultDirection = _hittedAwayDirection;
            resultDirection.x *= (CharacterTransform.position - PlayerCharacter.EnemyObject.transform.position).x > 0.0f ? 1.0f : -1.0f;
            CharacterRigidBody.velocity = resultDirection;
            */
            
            CharacterAnimator.PlayAnimation("InAirHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("InAirHit", CharacterAnimator.Layer.LowerLayer, true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            return base.GetResultStateByHandleInput(behavior);
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if (this.CharacterTransform.position.y <= this.PlayerCharacter.PositionYOffsetForLand)
                return BehaviorEnumSet.State.FallDown;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}