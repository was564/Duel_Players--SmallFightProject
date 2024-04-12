using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class BeCaughtState : BehaviorStateInterface
    {
        public BeCaughtState(GameObject characterRoot) : base(BehaviorEnumSet.State.BeCaught, characterRoot,
            BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
        }

        public override void Enter()
        {
            CharacterAnimator.SetPoseInAnimation("StandingHit", CharacterAnimator.Layer.UpperLayer, 0.5f);
            CharacterAnimator.SetPoseInAnimation("StandingHit", CharacterAnimator.Layer.LowerLayer, 0.5f);
            CharacterAnimator.PauseAnimation();
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Grab:
                    return BehaviorEnumSet.State.GrabEscape;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterAnimator.ResumeAnimation();
        }
    }
}