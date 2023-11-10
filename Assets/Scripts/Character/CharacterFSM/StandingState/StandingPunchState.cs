using UnityEngine;
using UnityEngine.Assertions;

namespace Character.CharacterFSM
{
    public class StandingPunchState : PunchState
    {
        public StandingPunchState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.StandingPunch, stateManager, characterRoot, BehaviorEnumSet.State.StandingIdle) {}
        
        public override void Enter()
        {
            base.Enter();
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
            
            CharacterRigidBody.velocity = Vector3.zero;
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    break;
                case BehaviorEnumSet.Behavior.StandingPunchSkill:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunchSkill);
                    break;
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
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.Punch);
        }
    }
}