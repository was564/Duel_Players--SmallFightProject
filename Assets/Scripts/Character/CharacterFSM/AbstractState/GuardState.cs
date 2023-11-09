using UnityEngine;

namespace Character.CharacterFSM
{
    public abstract class GuardState : BehaviorStateInterface
    {
        public GuardState(GameObject characterRoot, BehaviorStateSimulator stateManager, BehaviorEnumSet.State guardStateName,
            BehaviorEnumSet.State nextState) :
            base(guardStateName, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.SpecialMove)
        {
            _nextState = nextState;
        }

        private BehaviorEnumSet.State _nextState;
        
        public override void Enter()
        {
            CharacterAnimator.PlayAnimation("Guard", CharacterAnimator.Layer.UpperLayer, true);
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
            if(CharacterAnimator.IsEndCurrentAnimation("Guard", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(_nextState);
        }

        public override void Quit()
        {
            
        }
    }
}