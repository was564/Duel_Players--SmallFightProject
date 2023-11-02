using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingHitState : BehaviorStateInterface
    {
        public StandingHitState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingHit, characterRoot, BehaviorEnumSet.AttackLevel.SpecialMove) {}
        
        public override void Enter()
        {
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
            
            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("StandingIdle", CharacterAnimator.Layer.LowerLayer, true);
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
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            
        }
    }
}