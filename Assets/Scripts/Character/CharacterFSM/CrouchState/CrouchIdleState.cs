using Unity.VisualScripting;
using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchIdleState : BehaviorStateInterface
    {
        public CrouchIdleState(GameObject characterRoot) 
            : base(BehaviorEnumSet.State.CrouchIdle, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.Crouch) {}

        private int _startingFrame;
        private bool _isApplyFinalPosition;
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            PlayerCharacter.IsHitContinuous = false;
            
            _startingFrame = FrameManager.CurrentFrame;
            if (PlayerCharacter.CharacterPositionState == PassiveStateEnumSet.CharacterPositionState.Crouch) 
                _isApplyFinalPosition = true;
            else
            {
                _isApplyFinalPosition = false;
                PlayerCharacter.CharacterPositionState = PassiveStateEnumSet.CharacterPositionState.Crouch;
            }
                
            CharacterRigidBody.velocity = Vector3.zero;
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("Crouch", CharacterAnimator.Layer.LowerLayer);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stand:
                    return BehaviorEnumSet.State.StandingIdle;
                
                case BehaviorEnumSet.Behavior.Punch:
                    return BehaviorEnumSet.State.CrouchPunch;
                
                case BehaviorEnumSet.Behavior.Kick:
                    return BehaviorEnumSet.State.CrouchKick;
                
                case BehaviorEnumSet.Behavior.Guard:
                    return BehaviorEnumSet.State.CrouchGuard;
                
                case BehaviorEnumSet.Behavior.StandingPunch236Skill:
                    return BehaviorEnumSet.State.StandingPunch236Skill;
                
                case BehaviorEnumSet.Behavior.StandingKick236Skill:
                    return BehaviorEnumSet.State.StandingKick236Skill;
                
                case BehaviorEnumSet.Behavior.StandingPunch623Skill:
                    return BehaviorEnumSet.State.StandingPunch623Skill;
                
                case BehaviorEnumSet.Behavior.StandingKick623Skill:
                    return BehaviorEnumSet.State.StandingKick623Skill;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            PlayerCharacter.LookAtEnemy();
            
            if (!_isApplyFinalPosition)
            {
                Vector3 characterPosition = this.CharacterTransform.position;
                // 애니메이션 재생속도 0.1초
                float animationDurationTime = Mathf.Clamp01((FrameManager.CurrentFrame - _startingFrame) * 10.0f);
                float positionY = Mathf.Lerp(
                    0,
                    -0.4f,
                    animationDurationTime);
                
                characterPosition.y = positionY;
                this.CharacterTransform.position = characterPosition;
                if (animationDurationTime == 1.0f)
                    _isApplyFinalPosition = true;
            }
            
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            Vector3 characterPosition = this.CharacterTransform.position;
            characterPosition.y = -0.4f;
            this.CharacterTransform.position = characterPosition;
        }
    }
}