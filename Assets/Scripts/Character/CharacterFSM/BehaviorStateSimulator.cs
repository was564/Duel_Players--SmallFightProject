using System.Collections.Generic;
using Character.CharacterFSM.SkillState;
using UnityEngine;

namespace Character.CharacterFSM
{
    public class BehaviorStateSimulator
    {
        protected Dictionary<BehaviorEnumSet.State, BehaviorStateInterface> BehaviorStateSet
            = new Dictionary<BehaviorEnumSet.State, BehaviorStateInterface>();
        
        protected GameObject RootCharacterObject;
        
        public BehaviorStateInterface CurrentState { get; protected set; }
    
        protected ComboManager ComboManagerInstance;
        
        public BehaviorStateSimulator(GameObject characterObject, ComboManager comboManager)
        {
            RootCharacterObject = characterObject.transform.root.gameObject;
            ComboManagerInstance = comboManager;
        
            InitStateSet();
        
            CurrentState = BehaviorStateSet[BehaviorEnumSet.State.StandingIdle];
            //_currentState.Enter();
        }
        
        public virtual void ChangeState(BehaviorEnumSet.State nextState)
        {
            BehaviorStateInterface nextStateInfo = BehaviorStateSet[nextState];
            if(ComboManagerInstance.CheckStateTransition(CurrentState, nextStateInfo))
                CurrentState = nextStateInfo;
        }
        
        public virtual void UpdateState() { }

        public virtual void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            CurrentState.HandleInput(behavior);
        }
        
        public int GetAttackLevel(BehaviorEnumSet.State state)
        {
            return BehaviorStateSet[state].AttackLevel;
        }

        public virtual void ForceChangeState(BehaviorEnumSet.State state)
        {
            CurrentState = BehaviorStateSet[state];
        }
        
        private void InitStateSet()
        {
            BehaviorStateSet.Add(BehaviorEnumSet.State.StandingHit, new StandingHitState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.StandingIdle, new StandingIdleState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.StandingPunch, new StandingPunchState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.StandingKick, new StandingKickState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.Forward, new WalkingForwardState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.Backward, new WalkingBackwardState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.Jump, new JumpState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.InAirIdle, new AiringState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.Land, new LandState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchIdle, new CrouchIdleState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchPunch, new CrouchPunchState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchKick, new CrouchKickState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.AiringPunch, new AiringPunchState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.AiringKick, new AiringKickState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.StandingPunchSkill, new StandingPunchSkillState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.DashOnGround, new DashOnGroundState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.StandingGuard, new StandingGuardState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchGuard, new CrouchGuardState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchHit, new CrouchHitState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.InAirHit, new InAirHitState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.FallDown, new FallDownState(RootCharacterObject, this));
            BehaviorStateSet.Add(BehaviorEnumSet.State.GetUp, new GetUpState(RootCharacterObject, this));
        }
    }
}