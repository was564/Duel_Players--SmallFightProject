using UnityEngine;
using UnityEngine.Assertions;

namespace Character.CharacterFSM
{
    public class StandingPunchState : PunchState
    {
        public StandingPunchState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingPunch, characterRoot, BehaviorEnumSet.State.StandingIdle,
                PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        public override void Enter()
        {
            base.Enter();
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            //CharacterRigidBody.velocity = Vector3.zero;
            CharacterAnimator.PlayAnimation("StandingPunch", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    return BehaviorEnumSet.State.Null;
                
                case BehaviorEnumSet.Behavior.Jump:
                    return BehaviorEnumSet.State.Null;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            return base.UpdateState();
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.StandingPunch);
        }
    }
}