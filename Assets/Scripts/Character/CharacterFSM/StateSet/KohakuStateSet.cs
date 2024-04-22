using System.Collections.Generic;
using Character.CharacterFSM.KohakuState.SkillState;
using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class KohakuStateSet : BehaviorStateSetInterface
    {
        protected override List<BehaviorStateInterface> StateSet { get; set; }

        protected override GameObject CharacterRoot { get; set; }

        public KohakuStateSet(GameObject characterRoot, GameObject wall) : base(characterRoot)
        {
            InitClass();
            BindState(BehaviorEnumSet.State.StandingHit, new StandingHitState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingIdle, new StandingIdleState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingPunch, new StandingPunchState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingKick, new StandingKickState(characterRoot));
            BindState(BehaviorEnumSet.State.Forward, new WalkingForwardState(characterRoot));
            BindState(BehaviorEnumSet.State.Backward, new WalkingBackwardState(characterRoot));
            BindState(BehaviorEnumSet.State.Jump, new JumpState(characterRoot));
            BindState(BehaviorEnumSet.State.InAirIdle, new AiringState(characterRoot));
            BindState(BehaviorEnumSet.State.Land, new LandState(characterRoot));
            BindState(BehaviorEnumSet.State.CrouchIdle, new CrouchIdleState(characterRoot));
            BindState(BehaviorEnumSet.State.CrouchPunch, new CrouchPunchState(characterRoot));
            BindState(BehaviorEnumSet.State.CrouchKick, new CrouchKickState(characterRoot));
            BindState(BehaviorEnumSet.State.AiringPunch, new AiringPunchState(characterRoot));
            BindState(BehaviorEnumSet.State.AiringKick, new AiringKickState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingPunch236Skill, new StandingPunch236SkillState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingKick236Skill, new StandingKick236SkillState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingPunch623Skill, new StandingPunch623SkillState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingKick623Skill, new StandingKick623SkillState(characterRoot));
            BindState(BehaviorEnumSet.State.DashOnGround, new DashOnGroundState(characterRoot));
            BindState(BehaviorEnumSet.State.BackStepOnGroundState, new BackStepOnGroundState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingGuard, new StandingGuardState(characterRoot, wall));
            BindState(BehaviorEnumSet.State.CrouchGuard, new CrouchGuardState(characterRoot, wall));
            BindState(BehaviorEnumSet.State.CrouchHit, new CrouchHitState(characterRoot));
            BindState(BehaviorEnumSet.State.InAirHit, new InAirHitState(characterRoot));
            BindState(BehaviorEnumSet.State.FallDown, new FallDownState(characterRoot));
            BindState(BehaviorEnumSet.State.GetUp, new GetUpState(characterRoot));
            BindState(BehaviorEnumSet.State.IntroPose, new IntroPoseState(characterRoot));
            BindState(BehaviorEnumSet.State.OutroPose, new OutroPoseState(characterRoot));
            BindState(BehaviorEnumSet.State.GrabStart, new GrabStartState(characterRoot));
            BindState(BehaviorEnumSet.State.GrabWait, new GrabWaitState(characterRoot));
            BindState(BehaviorEnumSet.State.GrabAttack, new GrabAttackState(characterRoot));
            BindState(BehaviorEnumSet.State.BeCaught, new BeCaughtState(characterRoot));
            BindState(BehaviorEnumSet.State.GrabEscape, new EscapeGrabState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingStopHit, new StandingStopHitState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingPunch6246SpecialSkillEnter, new StandingPunch6246SpecialSkillEnterState(characterRoot));
            BindState(BehaviorEnumSet.State.StandingPunch6246SpecialSkillAttack, new StandingPunch6246SpecialSkillAttackState(characterRoot));
        }
    }
}