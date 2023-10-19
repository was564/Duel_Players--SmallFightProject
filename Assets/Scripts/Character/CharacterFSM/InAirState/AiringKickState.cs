using UnityEngine;

namespace Character.CharacterFSM
{
    public class AiringKickState : BehaviorStateInterface
    {
        public AiringKickState(GameObject characterRoot) 
            : base(BehaviorEnumSet.State.AiringKick, characterRoot, BehaviorEnumSet.AttackLevel.BasicAttack) {}

        public override void Enter()
        {
            CharacterAnimator.PlayAnimation("AiringKick", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("AiringKick", CharacterAnimator.Layer.LowerLayer,true);
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
            if (this.CharacterTransform.position.y <= this.Character.PositionYOffsetForLand)
                StateManager.ChangeState(BehaviorEnumSet.State.Land);
            
            if(CharacterAnimator.IsEndCurrentAnimation("AiringKick", CharacterAnimator.Layer.LowerLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.InAirIdle);
        }

        public override void Quit()
        {
            
        }
    }
}