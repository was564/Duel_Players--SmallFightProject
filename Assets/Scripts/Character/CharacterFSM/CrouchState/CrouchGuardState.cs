using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchGuardState: GuardState
    {
        public CrouchGuardState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(characterRoot, stateManager, BehaviorEnumSet.State.StandingGuard, BehaviorEnumSet.State.CrouchIdle) {}
        
        public override void Enter()
        {
            base.Enter();
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.Crouch);
            
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
            base.UpdateState();
        }

        public override void Quit()
        {
            
        }
    }
}