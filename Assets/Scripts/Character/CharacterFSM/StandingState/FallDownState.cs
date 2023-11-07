using UnityEngine;

namespace Character.CharacterFSM
{
    public class FallDownState : BehaviorStateInterface
    {
        public FallDownState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.FallDown, characterRoot, BehaviorEnumSet.AttackLevel.SpecialMove) {}

        private float stateStartingTime;
        [SerializeField]
        private float lyingDownTime = 0.2f;
        public override void Enter()
        {
            stateStartingTime = 0.0f;
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
             
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
            stateStartingTime += Time.deltaTime;
            if(stateStartingTime >= lyingDownTime)
                StateManager.ChangeState(BehaviorEnumSet.State.GetUp);
        }

        public override void Quit()
        {
            
        }
    }
}