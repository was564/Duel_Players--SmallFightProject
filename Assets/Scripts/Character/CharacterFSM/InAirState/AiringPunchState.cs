using UnityEngine;

namespace Character.CharacterFSM
{
    public class AiringPunchState : BehaviorStateInterface
    {
        public AiringPunchState(GameObject characterRoot, BehaviorStateSimulator stateManager) 
            : base(BehaviorEnumSet.State.AiringPunch, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.BasicAttack) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.InAir);
            
            CharacterAnimator.PlayAnimation("AiringPunch", CharacterAnimator.Layer.UpperLayer,true);
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
            
            if(CharacterAnimator.IsEndCurrentAnimation("AiringPunch", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.InAirIdle);
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.Punch);
        }
    }
}