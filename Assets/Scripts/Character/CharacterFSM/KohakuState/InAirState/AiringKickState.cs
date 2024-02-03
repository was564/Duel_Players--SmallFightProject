using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class AiringKickState : BehaviorStateInterface
    {
        public AiringKickState(GameObject characterRoot) 
            : base(BehaviorEnumSet.State.AiringKick, characterRoot, 
                BehaviorEnumSet.AttackLevel.BasicAttack, PassiveStateEnumSet.CharacterPositionState.InAir) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterAnimator.PlayAnimation("AiringKick", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("AiringKick", CharacterAnimator.Layer.LowerLayer,true);
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
            
            if(CharacterAnimator.IsEndCurrentAnimation("AiringKick", CharacterAnimator.Layer.LowerLayer))
                return BehaviorEnumSet.State.InAirIdle;

            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.AiringKick);
        }
    }
}