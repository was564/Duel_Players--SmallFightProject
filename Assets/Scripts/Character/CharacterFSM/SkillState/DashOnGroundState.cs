using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.SkillState
{
    public class DashOnGroundState : SkillStateInterface
    {
        
        
        public DashOnGroundState(GameObject characterRoot)
            : base(BehaviorEnumSet.State.DashOnGround, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            MoveCommand = new List<BehaviorEnumSet.InputSet>()
            {
                BehaviorEnumSet.InputSet.Forward,
                BehaviorEnumSet.InputSet.Idle,
                BehaviorEnumSet.InputSet.Forward
            };
            AttackTrigger = BehaviorEnumSet.Behavior.Null;
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.OnGround);
            CommandManager.AddCommand(
                MoveCommand,
                AttackTrigger,
                AvailableCommandPositionCondition,
                BehaviorEnumSet.Behavior.Dash);
        }

        public override List<BehaviorEnumSet.InputSet> MoveCommand { get; protected set; }
        public override BehaviorEnumSet.Behavior AttackTrigger { get; protected set; }

        public override List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionCondition
            { get; protected set; } = new List<PassiveStateEnumSet.CharacterPositionState>();

        private float _dashVelocity = 4.0f;

        private Vector3 _finalVelocity;
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
            
            _finalVelocity = (CharacterTransform.transform.forward.x > 0.0f)
                ? (Vector3.right * _dashVelocity)
                : (Vector3.left * _dashVelocity);
            CharacterRigidBody.velocity = _finalVelocity;
            CharacterAnimator.PlayAnimationSmoothly("Dash", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("Dash", CharacterAnimator.Layer.LowerLayer);
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
                
                case BehaviorEnumSet.Behavior.Backward:
                    return BehaviorEnumSet.State.Backward;
                
                case BehaviorEnumSet.Behavior.StandingKick236Skill:
                    return BehaviorEnumSet.State.StandingKick236Skill;
                
                case BehaviorEnumSet.Behavior.StandingKick623Skill:
                    return BehaviorEnumSet.State.StandingKick623Skill;
                
                case BehaviorEnumSet.Behavior.StandingPunch236Skill:
                    return BehaviorEnumSet.State.StandingPunch236Skill;
                
                case BehaviorEnumSet.Behavior.StandingPunch623Skill:
                    return BehaviorEnumSet.State.StandingPunch623Skill;
                
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