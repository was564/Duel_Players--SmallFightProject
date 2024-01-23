using UnityEngine;
using UnityEngine.Assertions;

namespace Character.CharacterFSM.KohakuState
{
    public abstract class PunchState : BehaviorStateInterface
    {
        public PunchState(BehaviorEnumSet.State state, GameObject characterRoot, 
            BehaviorEnumSet.State nextState, PassiveStateEnumSet.CharacterPositionState positionState) :
            base(state, characterRoot, BehaviorEnumSet.AttackLevel.BasicAttack, positionState)
        {
            _nextState = nextState;
        }

        protected BehaviorEnumSet.State _nextState;
        
        public override void Enter()
        {
            //CharacterAnimator.PlayAnimation("StandingPunch", CharacterAnimator.Layer.UpperLayer,true);
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
            if (CharacterAnimator.IsEndCurrentAnimation("StandingPunch", CharacterAnimator.Layer.UpperLayer))
                return _nextState;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}