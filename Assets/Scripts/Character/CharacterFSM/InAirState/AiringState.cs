using UnityEngine;

namespace Character.CharacterFSM
{
    public class AiringState : BehaviorStateInterface
    {
        public AiringState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.InAirIdle, characterRoot, BehaviorEnumSet.AttackLevel.Move) {}
        
        public override void Enter()
        {
            Character.CharacterPositionState = PassiveStateEnumSet.CharacterPositionState.InAir;
            
            CharacterAnimator.PlayAnimationSmoothly("InAirIdle", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("InAirIdle", CharacterAnimator.Layer.LowerLayer);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    StateManager.ChangeState(BehaviorEnumSet.State.AiringPunch);
                    break;
                case BehaviorEnumSet.Behavior.Kick:
                    StateManager.ChangeState(BehaviorEnumSet.State.AiringKick);
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