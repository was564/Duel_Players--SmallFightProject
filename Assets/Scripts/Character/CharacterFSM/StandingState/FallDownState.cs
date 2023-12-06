using UnityEngine;

namespace Character.CharacterFSM
{
    public class FallDownState : BehaviorStateInterface
    {
        public FallDownState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.FallDown, stateManager, characterRoot, 
                BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround) {}

        private int stateStartingFrame;
        [SerializeField]
        private int lyingDownFrame = 12;
        public override void Enter()
        {
            stateStartingFrame = 0;
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            CharacterJudgeBoxController.DisableHitBox();
             
            CharacterAnimator.PlayAnimation("FallDown", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("FallDown", CharacterAnimator.Layer.LowerLayer);
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
            if (PlayerCharacter.Hp <= 0) return;
            stateStartingFrame += 1;
            if(stateStartingFrame >= lyingDownFrame)
                StateManager.ChangeState(BehaviorEnumSet.State.GetUp);
        }

        public override void Quit()
        {
            PlayerCharacter.IsHitContinuous = false;
        }
    }
}