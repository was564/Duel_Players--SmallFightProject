using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class AiringJumpState : BehaviorStateInterface
    {
        public AiringJumpState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.InAirIdle, characterRoot, 
                BehaviorEnumSet.AttackLevel.CancelableMove, PassiveStateEnumSet.CharacterPositionState.InAir) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            PlayerCharacter.IsDoubleJumped = true;
            PlayerCharacter.LookAtEnemy();
            
            Vector3 characterVelocity = CharacterRigidBody.velocity;
            characterVelocity.y = 6.5f;
            CharacterRigidBody.velocity = characterVelocity;
            
            CharacterAnimator.PlayAnimation("Jump", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("Jump", CharacterAnimator.Layer.LowerLayer);
        }

        
        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            
            
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Punch:
                    return BehaviorEnumSet.State.AiringPunch;
                
                case BehaviorEnumSet.Behavior.Kick:
                    return BehaviorEnumSet.State.AiringKick;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if (CharacterAnimator.IsEndCurrentAnimation("Jump", CharacterAnimator.Layer.LowerLayer))
                return BehaviorEnumSet.State.InAirIdle;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}