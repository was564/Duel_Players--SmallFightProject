using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchKickState : BehaviorStateInterface
    {
        public CrouchKickState(GameObject characterRoot, BehaviorStateSimulator stateManager) 
            : base(BehaviorEnumSet.State.CrouchKick, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.BasicAttack) {}

        public override void Enter()
        {
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.Crouch);
            
            CharacterAnimator.PlayAnimation("CrouchKick", CharacterAnimator.Layer.LowerLayer,true);
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
            if(CharacterAnimator.IsEndCurrentAnimation("CrouchKick", CharacterAnimator.Layer.LowerLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.CrouchIdle);
        }

        public override void Quit()
        {
            
        }
    }
}