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
            CharacterRigidBody.AddForce(Vector3.up * 2.0f);
            CharacterAnimator.PlayAnimation("Jump");
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
            if(CharacterAnimator.IsEndCurrentAnimation("Jump"))
                StateManager.ChangeState(BehaviorEnumSet.State.InAirIdle);
        }

        public override void Quit()
        {
            
        }
    }
}