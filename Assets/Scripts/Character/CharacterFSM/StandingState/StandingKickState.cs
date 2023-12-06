using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingKickState : BehaviorStateInterface
    {
        public StandingKickState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.StandingKick, stateManager, characterRoot, 
                BehaviorEnumSet.AttackLevel.BasicAttack, PassiveStateEnumSet.CharacterPositionState.OnGround) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            //CharacterRigidBody.velocity = Vector3.zero;
            CharacterAnimator.PlayAnimation("StandingKick", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingKick", CharacterAnimator.Layer.LowerLayer,true);
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
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.StandingKick);
        }
    }
}