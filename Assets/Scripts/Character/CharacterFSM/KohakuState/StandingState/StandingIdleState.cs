using UnityEngine;

namespace Character.CharacterFSM.KohakuState
{
    public class StandingIdleState : BehaviorStateInterface
    {
        public StandingIdleState(GameObject characterRoot) : 
            base(BehaviorEnumSet.State.StandingIdle, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.OnGround) {}
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            PlayerCharacter.IsHitContinuous = false;
            
            CharacterRigidBody.velocity = Vector3.zero;
            
            CharacterAnimator.PlayAnimation("StandingIdle", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimation("StandingIdle", CharacterAnimator.Layer.LowerLayer);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stand:
                    return BehaviorEnumSet.State.Null;
                    
                case BehaviorEnumSet.Behavior.Punch:
                    return BehaviorEnumSet.State.StandingPunch;
                
                case BehaviorEnumSet.Behavior.Kick:
                    return BehaviorEnumSet.State.StandingKick;
                
                case BehaviorEnumSet.Behavior.Crouch:
                    return BehaviorEnumSet.State.CrouchIdle;
                
                case BehaviorEnumSet.Behavior.Jump:
                    return BehaviorEnumSet.State.Jump;
                
                case BehaviorEnumSet.Behavior.Backward:
                    return BehaviorEnumSet.State.Backward;
                
                case BehaviorEnumSet.Behavior.Forward:
                    return BehaviorEnumSet.State.Forward;
                
                case BehaviorEnumSet.Behavior.Guard:
                    return BehaviorEnumSet.State.StandingGuard;
                
                case BehaviorEnumSet.Behavior.Dash:
                    return BehaviorEnumSet.State.DashOnGround;
                
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
                
                case BehaviorEnumSet.Behavior.Grab:
                    return BehaviorEnumSet.State.GrabStart;
                
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            PlayerCharacter.LookAtEnemy();
            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    } 
}