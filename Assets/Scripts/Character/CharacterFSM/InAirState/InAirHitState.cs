using UnityEngine;

namespace Character.CharacterFSM
{
    public class InAirHitState : BehaviorStateInterface
    {
        public InAirHitState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.InAirHit, characterRoot, BehaviorEnumSet.AttackLevel.SpecialMove) {}

        private Vector3 _hittedAwayDirection = new Vector3(1.5f, 5.0f, 0);
        
        public override void Enter()
        {
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.InAir);

            CharacterRigidBody.velocity = _hittedAwayDirection;
            
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
            if (this.CharacterTransform.position.y <= this.Character.PositionYOffsetForLand)
                StateManager.ChangeState(BehaviorEnumSet.State.FallDown);
        }

        public override void Quit()
        {
            
        }
    }
}