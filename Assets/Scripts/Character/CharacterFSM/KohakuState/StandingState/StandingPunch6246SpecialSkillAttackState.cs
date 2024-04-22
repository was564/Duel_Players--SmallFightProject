using Unity.VisualScripting;
using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class StandingPunch6246SpecialSkillAttackState : BehaviorStateInterface
    {
        public StandingPunch6246SpecialSkillAttackState(GameObject characterRoot) : base(BehaviorEnumSet.State.StandingPunch6246SpecialSkillAttack, characterRoot,
            BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.InAir)
        {
        }
        
        public override void Enter()
        {
            _progressLevel = 0;
            
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            PlayerCharacter.RigidBody.velocity = Vector3.zero;
            
            CharacterAnimator.PlayAnimation("StandingPunch6246SpecialSkill", CharacterAnimator.Layer.UpperLayer, true);
            CharacterAnimator.PlayAnimation("StandingPunch6246SpecialSkill", CharacterAnimator.Layer.LowerLayer, true);
        }

        
        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        private int _progressLevel = 0;
        public override BehaviorEnumSet.State UpdateState()
        {
            Vector3 position = PlayerCharacter.transform.position;
            position.y = 0;
            PlayerCharacter.transform.position = position;
            
            if (_progressLevel == 0 && CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) >= 0.25f)
            {
                PlayerCharacter.SpecialSkillBallInstance.transform.position =
                    PlayerCharacter.transform.position + Vector3.forward * 0.5f;
                PlayerCharacter.SpecialSkillBallInstance.SetActive(true);
                _progressLevel = 1;
            }
            
            if(_progressLevel == 1 && CharacterAnimator.GetCurrentAnimationDuration(CharacterAnimator.Layer.UpperLayer) >= 0.7f)
            {
                CharacterJudgeBoxController.EnableHitBox();
                PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
                _progressLevel = 2;
            }
            
            if (CharacterAnimator.IsEndCurrentAnimation("StandingPunch6246SpecialSkill", CharacterAnimator.Layer.UpperLayer))
            {
                return BehaviorEnumSet.State.StandingIdle;
            }

            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.EnableHitBox();
        }
    }
}