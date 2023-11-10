using Unity.VisualScripting;
using UnityEngine;

namespace Character.CharacterFSM
{
    public class CrouchIdleState : BehaviorStateInterface
    {
        public CrouchIdleState(GameObject characterRoot, BehaviorStateSimulator stateManager) 
            : base(BehaviorEnumSet.State.CrouchIdle, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.Move) {}

        private float _startingTime;
        private bool _isApplyFinalPosition;
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.Crouch);
            
            _startingTime = Time.time;
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

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stand:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
                    break;
                case BehaviorEnumSet.Behavior.Punch:
                    StateManager.ChangeState(BehaviorEnumSet.State.CrouchPunch);
                    break;
                case BehaviorEnumSet.Behavior.Kick:
                    StateManager.ChangeState(BehaviorEnumSet.State.CrouchKick);
                    break;
                case BehaviorEnumSet.Behavior.Guard:
                    StateManager.ChangeState(BehaviorEnumSet.State.CrouchGuard);
                    break;
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            if (!_isApplyFinalPosition)
            {
                Vector3 characterPosition = this.CharacterTransform.position;
                // 애니메이션 재생속도 0.1초
                float animationDurationTime = Mathf.Clamp01((Time.time - _startingTime) * 10.0f);
                float positionY = Mathf.Lerp(
                    0,
                    -0.4f,
                    animationDurationTime);
                
                characterPosition.y = positionY;
                this.CharacterTransform.position = characterPosition;
                if (animationDurationTime == 1.0f)
                    _isApplyFinalPosition = true;
            }
        }

        public override void Quit()
        {
            Vector3 characterPosition = this.CharacterTransform.position;
            characterPosition.y = -0.4f;
            this.CharacterTransform.position = characterPosition;
        }
    }
}