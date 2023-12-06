using UnityEngine;

namespace Character.CharacterFSM
{
    public class StandingIdleState : BehaviorStateInterface
    {
        public StandingIdleState(GameObject characterRoot, BehaviorStateSimulator stateManager) : 
            base(BehaviorEnumSet.State.StandingIdle, stateManager, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            PlayerCharacter.IsHitContinuous = false;
            
            CharacterRigidBody.velocity = Vector3.zero;
            
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stand:
                    break;
                case BehaviorEnumSet.Behavior.Punch:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunch);
                    break;
                case BehaviorEnumSet.Behavior.Kick:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingKick);
                    break;
                case BehaviorEnumSet.Behavior.Crouch:
                    StateManager.ChangeState(BehaviorEnumSet.State.CrouchIdle);
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    StateManager.ChangeState(BehaviorEnumSet.State.Jump);
                    break;
                case BehaviorEnumSet.Behavior.Backward:
                    StateManager.ChangeState(BehaviorEnumSet.State.Backward);
                    break;
                case BehaviorEnumSet.Behavior.Forward:
                    StateManager.ChangeState(BehaviorEnumSet.State.Forward);
                    break;
                case BehaviorEnumSet.Behavior.Guard:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingGuard);
                    break;
                case BehaviorEnumSet.Behavior.StandingPunch236Skill:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunch236Skill);
                    break;
                case BehaviorEnumSet.Behavior.StandingKick236Skill:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingKick236Skill);
                    break;
                case BehaviorEnumSet.Behavior.StandingPunch623Skill:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunch623Skill);
                    break;
                case BehaviorEnumSet.Behavior.StandingKick623Skill:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingKick623Skill);
                    break;
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            PlayerCharacter.LookAtEnemy();
        }

        public override void Quit()
        {
            
        }
    } 
}