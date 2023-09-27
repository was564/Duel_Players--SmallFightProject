using UnityEngine;

namespace Character.CharacterFSM
{
    public class LandState : BehaviorStateInterface
    {
        public LandState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.Land, characterRoot) {}

        public override void Enter()
        {
            Vector3 temp = CharacterTransform.position;
            temp.y = 0;
            CharacterTransform.position = temp;

            CharacterRigidBody.useGravity = false;
            CharacterRigidBody.velocity = Vector3.zero;
            CharacterAnimator.PlayAnimationSmoothly("Land");
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
            if(CharacterAnimator.IsEndCurrentAnimation("Land"))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            
        }
    }
}