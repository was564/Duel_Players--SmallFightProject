using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchGuardState: GuardState
    {
        public CrouchGuardState(GameObject characterRoot) : 
            base(characterRoot, BehaviorEnumSet.State.StandingGuard, BehaviorEnumSet.State.CrouchIdle) {}
        
        public override void Enter()
        {
            base.Enter();
            CharacterAnimator.PlayAnimation("CrouchStop", CharacterAnimator.Layer.LowerLayer, true);
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
            base.UpdateState();
        }

        public override void Quit()
        {
            
        }
    }
}