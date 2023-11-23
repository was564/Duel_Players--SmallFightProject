using UnityEngine;

namespace Character.CharacterFSM
{
    public class FallDownState : BehaviorStateInterface
    {
        public FallDownState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.FallDown, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.SpecialMove) {}

        private float stateStartingTime;
        [SerializeField]
        private float lyingDownTime = 0.2f;
        public override void Enter()
        {
            stateStartingTime = 0.0f;
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
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
            stateStartingTime += Time.deltaTime;
            if(stateStartingTime >= lyingDownTime)
                StateManager.ChangeState(BehaviorEnumSet.State.GetUp);
        }

        public override void Quit()
        {
            PlayerCharacter.IsHitContinuous = false;
        }
    }
}