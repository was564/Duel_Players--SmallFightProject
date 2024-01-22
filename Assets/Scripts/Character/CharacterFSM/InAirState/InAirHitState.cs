using UnityEngine;

namespace Character.CharacterFSM
{
    public class InAirHitState : BehaviorStateInterface
    {
        public InAirHitState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.InAirHit, characterRoot, 
                BehaviorEnumSet.AttackLevel.Hit, PassiveStateEnumSet.CharacterPositionState.InAir) {}
        
        private Vector3 _hittedAwayDirection = new Vector3(1.5f, 5.0f, 0);
        
        public override void Enter()
        {
            
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
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
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
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