using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchPunchState : PunchState
    {
        public CrouchPunchState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.CrouchPunch, stateManager, characterRoot, BehaviorEnumSet.State.CrouchIdle, 
                PassiveStateEnumSet.CharacterPositionState.Crouch) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            this.CharacterAnimator.PlayAnimation("CrouchPunch", CharacterAnimator.Layer.UpperLayer,true);
            this.CharacterAnimator.PlayAnimation("CrouchStop", CharacterAnimator.Layer.LowerLayer, true);
            Vector3 characterPosition = this.CharacterTransform.position;
            characterPosition.y = -0.4f;
            this.CharacterTransform.position = characterPosition;
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
            base.Quit();
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.CrouchPunch);
        }
    }
}