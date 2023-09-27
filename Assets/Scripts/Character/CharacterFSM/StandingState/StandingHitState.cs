using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingHitState : BehaviorStateInterface
    {
        public StandingHitState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingHit, characterRoot) {}
        
        public override void Enter()
        {
            CharacterAnimator.PlayAnimation("StandingHit", true);
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
            if(CharacterAnimator.IsEndCurrentAnimation("StandingHit"))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            
        }
    }
}