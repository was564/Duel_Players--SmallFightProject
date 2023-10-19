using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingKickState : BehaviorStateInterface
    {
        public StandingKickState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingKick, characterRoot, BehaviorEnumSet.AttackLevel.BasicAttack) {}

        public override void Enter()
        {
            CharacterAnimator.PlayAnimation("StandingKick", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingKick", CharacterAnimator.Layer.LowerLayer,true);
            CharacterRigidBody.velocity = Vector3.zero;
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
            if(CharacterAnimator.IsEndCurrentAnimation("StandingKick", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            
        }
    }
}