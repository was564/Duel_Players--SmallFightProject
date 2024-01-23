using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class CrouchPunchState : PunchState
    {
        public CrouchPunchState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.CrouchPunch, characterRoot, BehaviorEnumSet.State.CrouchIdle, 
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

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    // return BehaviorEnumSet.State.Punch;
                    return BehaviorEnumSet.State.Null;
                
                case BehaviorEnumSet.Behavior.Jump:
                    return BehaviorEnumSet.State.Null;
                    
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            return base.UpdateState();
        }

        public override void Quit()
        {
            base.Quit();
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.CrouchPunch);
        }
    }
}