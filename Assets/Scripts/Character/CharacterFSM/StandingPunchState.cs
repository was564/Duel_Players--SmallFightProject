using UnityEngine;
using UnityEngine.Assertions;

namespace Character.CharacterFSM
{
    public class StandingPunchState : BehaviorStateInterface
    {
        public StandingPunchState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.Punch, characterRoot) {}
        
        public override void Enter()
        {
            CharacterAnimator.Play("StandingPunch", -1, 0.0f);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Idle:
                    break;
                case BehaviorEnumSet.Behavior.Punch:
                    // StateManager.ChangeState(BehaviorEnumSet.State.Punch);
                    break;
                case BehaviorEnumSet.Behavior.Crouch:
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    break;
                case BehaviorEnumSet.Behavior.Left:
                    break;
                case BehaviorEnumSet.Behavior.Right:
                    break;
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            if (CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("StandingPunch"))
            {
                Debug.Assert(true, "State and Animation Mismatch : " + CharacterAnimator.GetCurrentAnimatorStateInfo(0).ToString());
            }
            if (CharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                StateManager.ChangeState(BehaviorEnumSet.State.Idle);
            }
            return;
        }

        public override void Quit()
        {
            
        }
    }
}