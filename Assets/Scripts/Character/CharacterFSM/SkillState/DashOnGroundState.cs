using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.SkillState
{
    public class DashOnGroundState : SkillStateInterface
    {
        
        
        public DashOnGroundState(GameObject characterRoot)
            : base(BehaviorEnumSet.State.StandingPunchSkill, characterRoot, BehaviorEnumSet.AttackLevel.Technique)
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
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
            
            _finalVelocity = (CharacterTransform.transform.forward.x > 0.0f)
                ? (Vector3.right * _dashVelocity)
                : (Vector3.left * _dashVelocity);
            CharacterRigidBody.velocity = _finalVelocity;
            CharacterAnimator.PlayAnimationSmoothly("Dash", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("Dash", CharacterAnimator.Layer.LowerLayer);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                case BehaviorEnumSet.Behavior.Stop:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
                    break;
                case BehaviorEnumSet.Behavior.Punch:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingPunch);
                    break;
                case BehaviorEnumSet.Behavior.Kick:
                    StateManager.ChangeState(BehaviorEnumSet.State.StandingKick);
                    break;
                case BehaviorEnumSet.Behavior.Crouch:
                    StateManager.ChangeState(BehaviorEnumSet.State.CrouchIdle);
                    break;
                case BehaviorEnumSet.Behavior.Jump:
                    StateManager.ChangeState(BehaviorEnumSet.State.Jump);
                    break;
                case BehaviorEnumSet.Behavior.Backward:
                    StateManager.ChangeState(BehaviorEnumSet.State.Backward);
                    break;
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            CharacterRigidBody.velocity = _finalVelocity;
        }

        public override void Quit()
        {
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
        }
    }
}