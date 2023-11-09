using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingGuardState: GuardState
    {
        public StandingGuardState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(characterRoot, stateManager, BehaviorEnumSet.State.StandingGuard, BehaviorEnumSet.State.StandingIdle) {}
        
        public override void Enter()
        {
            base.Enter();
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
            
            CharacterAnimator.PlayAnimation("StandingStop", CharacterAnimator.Layer.LowerLayer, true);
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