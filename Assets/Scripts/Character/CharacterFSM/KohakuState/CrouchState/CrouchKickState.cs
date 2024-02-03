using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class CrouchKickState : BehaviorStateInterface
    {
        public CrouchKickState(GameObject characterRoot) 
            : base(BehaviorEnumSet.State.CrouchKick, characterRoot, 
                BehaviorEnumSet.AttackLevel.BasicAttack, PassiveStateEnumSet.CharacterPositionState.Crouch) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterAnimator.PlayAnimation("CrouchKick", CharacterAnimator.Layer.LowerLayer,true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if(CharacterAnimator.IsEndCurrentAnimation("CrouchKick", CharacterAnimator.Layer.LowerLayer))
                return BehaviorEnumSet.State.CrouchIdle;

            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.CrouchKick);
        }
    }
}