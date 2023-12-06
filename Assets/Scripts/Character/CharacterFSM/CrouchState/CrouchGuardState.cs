using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchGuardState: GuardState
    {
        public CrouchGuardState(GameObject characterRoot, GameObject wall, BehaviorStateSimulator stateManager) : 
            base(characterRoot, wall, stateManager, BehaviorEnumSet.State.CrouchGuard, BehaviorEnumSet.State.CrouchIdle, 
                PassiveStateEnumSet.CharacterPositionState.Crouch) {}
        
        public override void Enter()
        {
            base.Enter();
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            CharacterAnimator.PlayAnimation("CrouchStop", CharacterAnimator.Layer.LowerLayer, true);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            PressGuardKey(behavior);
            
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stand:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingGuard);
                    break;
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
            base.Quit();
        }
    }
}