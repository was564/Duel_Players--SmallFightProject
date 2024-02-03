using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class GetUpState : BehaviorStateInterface
    {
        public GetUpState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.GetUp, characterRoot, 
                BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
             
            CharacterAnimator.PlayAnimation("GetUp", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("GetUp", CharacterAnimator.Layer.LowerLayer);
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
            if (CharacterAnimator.IsEndCurrentAnimation("GetUp", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingIdle;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.EnableHitBox();
        }
    }
}