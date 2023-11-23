using UnityEngine;

namespace Character.CharacterFSM
{
    public class GetUpState : BehaviorStateInterface
    {
        public GetUpState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.GetUp, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.SpecialMove) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
             
            CharacterAnimator.PlayAnimation("GetUp", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("GetUp", CharacterAnimator.Layer.LowerLayer);
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
            if(CharacterAnimator.IsEndCurrentAnimation("GetUp", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.EnableHitBox();
        }
    }
}