using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingGuardState: GuardState
    {
        public StandingGuardState(GameObject characterRoot, GameObject wall, BehaviorStateSimulator stateManager) : 
            base(characterRoot, wall, stateManager, BehaviorEnumSet.State.StandingGuard, BehaviorEnumSet.State.StandingIdle, 
                PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        public override void Enter()
        {
            base.Enter();
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            CharacterAnimator.PlayAnimation("StandingStop", CharacterAnimator.Layer.LowerLayer, true);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            PressGuardKey(behavior);

            if (ContinuousTimeByBlockAttack > 0.0f) return;
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Jump:
                    CharacterRigidBody.velocity = (CharacterTransform.forward.x > 0 ? -1.0f : 1.0f) * 2.5f * Vector3.right;
                    StateManager.ChangeState(BehaviorEnumSet.State.Jump);
                    break;
                case BehaviorEnumSet.Behavior.Crouch:
                    StateManager.ChangeState(BehaviorEnumSet.State.CrouchGuard);
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