using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class StandingKickState : BehaviorStateInterface
    {
        public StandingKickState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingKick, characterRoot, 
                BehaviorEnumSet.AttackLevel.BasicAttack, PassiveStateEnumSet.CharacterPositionState.OnGround) {}

        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            //CharacterRigidBody.velocity = Vector3.zero;
            CharacterAnimator.PlayAnimation("StandingKick", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingKick", CharacterAnimator.Layer.LowerLayer,true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if (CharacterAnimator.IsEndCurrentAnimation("StandingKick", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingIdle;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.State.StandingKick);
        }
    }
}