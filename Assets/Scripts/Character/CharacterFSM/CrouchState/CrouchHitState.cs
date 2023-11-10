using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchHitState : BehaviorStateInterface
    {
        public CrouchHitState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.CrouchHit, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.SpecialMove) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.Crouch);
            
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
                StateManager.ForceChangeState(BehaviorEnumSet.State.CrouchIdle);
        }

        public override void Quit()
        {
            
        }
    }
}