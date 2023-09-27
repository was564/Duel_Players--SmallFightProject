using UnityEngine;

namespace Character.CharacterFSM
{
    public class InAirIdleState : BehaviorStateInterface
    {
        public InAirIdleState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingIdle, characterRoot) {}
        
        public override void Enter()
        {
            CharacterAnimator.PlayAnimationSmoothly("InAirIdle");
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    break;
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            if (CharacterTransform.position.y <= 0)
            {
                StateManager.ChangeState(BehaviorEnumSet.State.Land);
            }
        }

        public override void Quit()
        {
            
        }
    }
}