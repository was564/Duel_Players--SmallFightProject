using UnityEngine;

namespace Character.CharacterFSM
{
    public class JumpState : BehaviorStateInterface
    {
        public JumpState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.Jump, characterRoot) {}

        public override void Enter()
        {
            CharacterRigidBody.useGravity = true;
            CharacterRigidBody.velocity += (Vector3.up * 3.5f);
            Character.InAir = true;
            CharacterAnimator.PlayAnimation("Jump", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("Jump", CharacterAnimator.Layer.LowerLayer);
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
            if(CharacterAnimator.IsEndCurrentAnimation("Jump", CharacterAnimator.Layer.LowerLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.InAirIdle);
        }

        public override void Quit()
        {
            
        }
    }
}