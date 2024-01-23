using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class AiringState : BehaviorStateInterface
    {
        public AiringState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.InAirIdle, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.InAir) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            CharacterAnimator.PlayAnimationSmoothly("InAirIdle", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("InAirIdle", CharacterAnimator.Layer.LowerLayer);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    return BehaviorEnumSet.State.AiringPunch;
                
                case BehaviorEnumSet.Behavior.Kick:
                    return BehaviorEnumSet.State.AiringKick;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if (this.CharacterTransform.position.y <= this.PlayerCharacter.PositionYOffsetForLand)
                return BehaviorEnumSet.State.Land;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}