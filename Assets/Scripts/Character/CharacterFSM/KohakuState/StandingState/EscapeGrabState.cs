using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class EscapeGrabState : BehaviorStateInterface
    {
        public EscapeGrabState(GameObject characterRoot) : base(BehaviorEnumSet.State.GrabEscape, characterRoot,
            BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
        }

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("StandingHit", CharacterAnimator.Layer.LowerLayer, true);
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
            if (CharacterAnimator.IsEndCurrentAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingIdle;

            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}