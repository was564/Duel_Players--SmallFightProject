using UnityEngine;

namespace Character.CharacterFSM
{
    public class JumpState : BehaviorStateInterface
    {
        public JumpState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.Jump, characterRoot, BehaviorEnumSet.AttackLevel.BasicAttack) {}

        public override void Enter()
        {
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.InAir);
            
            CharacterRigidBody.velocity += (Vector3.up * 3.5f);
            
            CharacterAnimator.PlayAnimation("Jump", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("Jump", CharacterAnimator.Layer.LowerLayer);
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
            if(CharacterAnimator.IsEndCurrentAnimation("Jump", CharacterAnimator.Layer.LowerLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.InAirIdle);
        }

        public override void Quit()
        {
            
        }
    }
}