using UnityEngine;

namespace Character.CharacterFSM
{
    public class InAirHitState : BehaviorStateInterface
    {
        public InAirHitState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.InAirHit, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.Hit) {}
        
        private Vector3 _hittedAwayDirection = new Vector3(1.5f, 5.0f, 0);
        
        public override void Enter()
        {
            
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.InAir);
            
            Vector3 resultDirection = _hittedAwayDirection;
            resultDirection.x *= (CharacterTransform.position - PlayerCharacter.EnemyObject.transform.position).x > 0.0f ? 1.0f : -1.0f;
            CharacterRigidBody.velocity = resultDirection;
            
            CharacterAnimator.PlayAnimation("InAirHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("InAirHit", CharacterAnimator.Layer.LowerLayer, true);
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
            if (this.CharacterTransform.position.y <= this.PlayerCharacter.PositionYOffsetForLand)
                StateManager.ChangeState(BehaviorEnumSet.State.FallDown);
        }

        public override void Quit()
        {
            
        }
    }
}