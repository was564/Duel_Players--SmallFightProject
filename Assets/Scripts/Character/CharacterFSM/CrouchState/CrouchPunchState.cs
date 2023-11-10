using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchPunchState : PunchState
    {
        public CrouchPunchState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.CrouchPunch, stateManager, characterRoot, BehaviorEnumSet.State.CrouchIdle) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.Crouch);
            
            Vector3 characterPosition = this.CharacterTransform.position;
            characterPosition.y = -0.4f;
            this.CharacterTransform.position = characterPosition;
            base.Enter();
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    // StateManager.ChangeState(BehaviorEnumSet.State.Punch);
                    break;
                case BehaviorEnumSet.Behavior.Jump:
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
            
        }
    }
}