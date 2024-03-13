using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class OutroPoseState : BehaviorStateInterface
    {
        private bool _isStoppendAnimation = false;
        
        public OutroPoseState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.OutroPose, characterRoot, 
                BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);

            _isStoppendAnimation = false;
            PlayerCharacter.IsEndedPoseAnimation = false;
            CharacterJudgeBoxController.DisableHitBox();
            
            CharacterAnimator.PlayAnimation("OutroAnimationIn", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("OutroAnimationIn", CharacterAnimator.Layer.LowerLayer);
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
            if (!_isStoppendAnimation &&
                CharacterAnimator.IsEndCurrentAnimation("OutroAnimation", CharacterAnimator.Layer.UpperLayer))
            {
                PlayerCharacter.IsEndedPoseAnimation = true;
                CharacterAnimator.PauseAnimation();
                _isStoppendAnimation = true;
            }
            
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterAnimator.ResumeAnimation();
            CharacterJudgeBoxController.EnableHitBox();
        }
    }
}