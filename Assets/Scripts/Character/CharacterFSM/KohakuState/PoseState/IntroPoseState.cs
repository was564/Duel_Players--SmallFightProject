using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class IntroPoseState : BehaviorStateInterface
    {
        private int _waitFrame = 0;
        
        private bool _isEndedPoseAnimation = false;
        
        public IntroPoseState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.IntroPose, characterRoot, 
                BehaviorEnumSet.AttackLevel.SpecialMove, PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            _isEndedPoseAnimation = false;
            PlayerCharacter.IsEndedPoseAnimation = false;
            CharacterJudgeBoxController.DisableHitBox();
            
            CharacterAnimator.PlayAnimation("IntroAnimationIn", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("IntroAnimationIn", CharacterAnimator.Layer.LowerLayer);
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
            if (!_isEndedPoseAnimation &&
                CharacterAnimator.IsEndCurrentAnimation("IntroAnimationOut", CharacterAnimator.Layer.UpperLayer))
            {
                CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.UpperLayer);
                CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
                _isEndedPoseAnimation = true;
            }

            if (_isEndedPoseAnimation)
            {
                _waitFrame++;
                if (_waitFrame >= 60)
                {
                    _waitFrame = 0;
                    return BehaviorEnumSet.State.StandingIdle;
                }
            }
            
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            _isEndedPoseAnimation = false;
            PlayerCharacter.IsEndedPoseAnimation = true;
            CharacterJudgeBoxController.EnableHitBox();
        }
    }
}