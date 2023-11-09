using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.SkillState
{
    public class StandingPunchSkillState : SkillStateInterface
    {
        public StandingPunchSkillState(GameObject characterRoot, BehaviorStateSimulator stateManager)
            : base(BehaviorEnumSet.State.StandingPunchSkill, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.Technique)
        {
            MoveCommand = new List<BehaviorEnumSet.InputSet>()
            {
                BehaviorEnumSet.InputSet.Down,
                BehaviorEnumSet.InputSet.Forward
            };
            AttackTrigger = BehaviorEnumSet.Behavior.Punch;
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.Crouch);
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.OnGround);
            CommandManager.AddCommand(
                MoveCommand, 
                AttackTrigger, 
                AvailableCommandPositionCondition,
                BehaviorEnumSet.Behavior.StandingPunchSkill);
        }

        public override List<BehaviorEnumSet.InputSet> MoveCommand { get; protected set; }
        public override BehaviorEnumSet.Behavior AttackTrigger { get; protected set; }
        public override List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionCondition { get; protected set; }
            = new List<PassiveStateEnumSet.CharacterPositionState>();

        private float _startingTime;
        private float _moveSpeed = 1.0f;
        private Vector3 _moveVelocity;
        
        public override void Enter()
        {
            _startingTime = Time.time;
            
            Character.ChangeCharacterPosition(PassiveStateEnumSet.CharacterPositionState.OnGround);
                
            _moveVelocity = (CharacterTransform.transform.forward.x > 0.0f)
                ? (Vector3.right * _moveSpeed)
                : (Vector3.left *_moveSpeed);
            CharacterRigidBody.velocity = _moveVelocity;
            
            CharacterAnimator.PlayAnimation("StandingPunchSkill", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingPunchSkill", CharacterAnimator.Layer.LowerLayer,true);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            if (Time.time - _startingTime <= 0.4f)
                CharacterRigidBody.velocity = _moveVelocity;
            
            
            if(CharacterAnimator.IsEndCurrentAnimation("StandingPunchSkill", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            
        }
    }
}