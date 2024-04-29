using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class WalkingBackwardState : BehaviorStateInterface
    {
        private float _walkingVelocity = 2.5f;

        private Vector3 _finalVelocity;
        
        public WalkingBackwardState(GameObject characterRoot) :
            base(BehaviorEnumSet.State.Backward, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            _finalVelocity = (CharacterTransform.transform.forward.x < 0.0f)
                    ? (Vector3.right * _walkingVelocity)
                    : (Vector3.left * _walkingVelocity);
            CharacterRigidBody.velocity = _finalVelocity;
            CharacterAnimator.PlayAnimationSmoothly("WalkBackward", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("WalkBackward", CharacterAnimator.Layer.LowerLayer);
        }
    
        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stop:
                    return BehaviorEnumSet.State.StandingIdle;
                
                case BehaviorEnumSet.Behavior.Punch:
                    return BehaviorEnumSet.State.StandingPunch;
                
                case BehaviorEnumSet.Behavior.Kick:
                    return BehaviorEnumSet.State.StandingKick;
                
                case BehaviorEnumSet.Behavior.Crouch:
                    return BehaviorEnumSet.State.CrouchIdle;
                
                case BehaviorEnumSet.Behavior.Jump:
                    return BehaviorEnumSet.State.Jump;
                
                case BehaviorEnumSet.Behavior.Forward:
                    return BehaviorEnumSet.State.Forward;
                
                case BehaviorEnumSet.Behavior.BackStep:
                    return BehaviorEnumSet.State.BackStepOnGroundState;
                
                case BehaviorEnumSet.Behavior.StandingPunch236Skill:
                    return BehaviorEnumSet.State.StandingPunch236Skill;
                
                case BehaviorEnumSet.Behavior.StandingKick236Skill:
                    return BehaviorEnumSet.State.StandingKick236Skill;
                
                case BehaviorEnumSet.Behavior.StandingPunch623Skill:
                    return BehaviorEnumSet.State.StandingPunch623Skill;
                
                case BehaviorEnumSet.Behavior.StandingKick623Skill:
                    return BehaviorEnumSet.State.StandingKick623Skill;
                
                case BehaviorEnumSet.Behavior.StandingPunch6246SpecialSkill:
                    if (PlayerCharacter.SkillGauge < 50)
                        return BehaviorEnumSet.State.StandingPunch623Skill;
                    PlayerCharacter.IncreaseSkillGauge(-50);
                    return BehaviorEnumSet.State.StandingPunch6246SpecialSkillEnter;
                
                case BehaviorEnumSet.Behavior.Guard:
                    return BehaviorEnumSet.State.StandingGuard;
                
                case BehaviorEnumSet.Behavior.Grab:
                    return BehaviorEnumSet.State.GrabStart;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }
    
        public override BehaviorEnumSet.State UpdateState()
        {
            CharacterRigidBody.velocity = _finalVelocity;
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
        }
    }
}