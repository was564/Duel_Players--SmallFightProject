using UnityEngine;

namespace Character.CharacterFSM
{
    public class InAirIdleState : BehaviorStateInterface
    {
        public InAirIdleState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.InAirIdle, characterRoot) {}
        
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
            if (this.CharacterTransform.position.y <= this.Character.PositionYOffsetForLand)
                StateManager.ChangeState(BehaviorEnumSet.State.Land);
        }

        public override void Quit()
        {
            
        }
    }
}