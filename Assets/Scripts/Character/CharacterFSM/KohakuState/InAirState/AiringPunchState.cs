using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class AiringPunchState : BehaviorStateInterface
    {
        public AiringPunchState(GameObject characterRoot) 
            : base(BehaviorEnumSet.State.AiringPunch, characterRoot, 
                BehaviorEnumSet.AttackLevel.BasicAttack, PassiveStateEnumSet.CharacterPositionState.InAir) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterAnimator.PlayAnimation("AiringPunch", CharacterAnimator.Layer.UpperLayer,true);
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
            if (this.CharacterTransform.position.y <= this.PlayerCharacter.PositionYOffsetForLand)
                return BehaviorEnumSet.State.Land;

            if (CharacterAnimator.IsEndCurrentAnimation("AiringPunch", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.InAirIdle;
            
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.State.AiringPunch);
        }
    }
}