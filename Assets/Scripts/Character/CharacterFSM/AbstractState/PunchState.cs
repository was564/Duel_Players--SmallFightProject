using UnityEngine;
using UnityEngine.Assertions;

namespace Character.CharacterFSM
{
    public abstract class PunchState : BehaviorStateInterface
    {
        public PunchState(BehaviorEnumSet.State state, BehaviorStateSimulator stateManager, GameObject characterRoot, BehaviorEnumSet.State nextState) :
            base(state, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.BasicAttack)
        {
            _nextState = nextState;
        }

        protected BehaviorEnumSet.State _nextState;
        
        public override void Enter()
        {
            CharacterAnimator.PlayAnimation("StandingPunch", CharacterAnimator.Layer.UpperLayer,true);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            if(CharacterAnimator.IsEndCurrentAnimation("StandingPunch", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(_nextState);
        }

        public override void Quit()
        {
            
        }
    }
}