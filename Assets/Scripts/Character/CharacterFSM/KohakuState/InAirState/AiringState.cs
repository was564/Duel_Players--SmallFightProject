using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class AiringState : BehaviorStateInterface
    {
        public AiringState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.InAirIdle, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.InAir) {}
        
        private float _horizontalVelocity = 2.5f;
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            CharacterAnimator.PlayAnimationSmoothly("InAirIdle", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("InAirIdle", CharacterAnimator.Layer.LowerLayer);
        }

        private BehaviorEnumSet.Behavior directionKeyInFrame = BehaviorEnumSet.Behavior.Null;
        private int frameWhenInputdirectionKey;
        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            if(frameWhenInputdirectionKey != FrameManager.CurrentFrame) directionKeyInFrame = BehaviorEnumSet.Behavior.Null;
            
            if(behavior == BehaviorEnumSet.Behavior.Forward || behavior == BehaviorEnumSet.Behavior.Backward)
            {
                directionKeyInFrame = behavior;
                frameWhenInputdirectionKey = FrameManager.CurrentFrame;
            }
            
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    return BehaviorEnumSet.State.AiringPunch;
                
                case BehaviorEnumSet.Behavior.Kick:
                    return BehaviorEnumSet.State.AiringKick;
                
                case BehaviorEnumSet.Behavior.Jump:
                    if(PlayerCharacter.IsDoubleJumped) return BehaviorEnumSet.State.Null;
                    SetVelocityByDirectionKey(directionKeyInFrame);
                    return BehaviorEnumSet.State.DoubleJump;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if (this.CharacterTransform.position.y <= this.PlayerCharacter.PositionYOffsetForLand)
                return BehaviorEnumSet.State.Land;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
        
        private void SetVelocityByDirectionKey(BehaviorEnumSet.Behavior direction)
        {
            Vector3 characterVelocity = CharacterRigidBody.velocity;
            if (direction == BehaviorEnumSet.Behavior.Forward)
                characterVelocity.x = (CharacterTransform.forward.x > 0 ? _horizontalVelocity : -_horizontalVelocity);
            else if (direction == BehaviorEnumSet.Behavior.Backward)
                characterVelocity.x = (CharacterTransform.forward.x > 0 ? -_horizontalVelocity : _horizontalVelocity);
            else characterVelocity.x = 0;
            CharacterRigidBody.velocity = characterVelocity;
        }
    }
}