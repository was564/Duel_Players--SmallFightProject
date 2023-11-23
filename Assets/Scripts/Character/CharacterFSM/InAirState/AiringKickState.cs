using UnityEngine;

namespace Character.CharacterFSM
{
    public class AiringKickState : BehaviorStateInterface
    {
        public AiringKickState(GameObject characterRoot, BehaviorStateSimulator stateManager) 
            : base(BehaviorEnumSet.State.AiringKick, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.BasicAttack) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.InAir);
            
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
            if (this.CharacterTransform.position.y <= this.PlayerCharacter.PositionYOffsetForLand)
                StateManager.ChangeState(BehaviorEnumSet.State.Land);
            
            if(CharacterAnimator.IsEndCurrentAnimation("AiringKick", CharacterAnimator.Layer.LowerLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.InAirIdle);
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.Kick);
        }
    }
}