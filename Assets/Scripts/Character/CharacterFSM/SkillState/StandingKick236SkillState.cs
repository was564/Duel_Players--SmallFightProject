using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.SkillState
{
    public class StandingKick236SkillState : SkillStateInterface
    {
        public StandingKick236SkillState(GameObject characterRoot, BehaviorStateSimulator stateManager)
            : base(BehaviorEnumSet.State.StandingKick236Skill, stateManager, characterRoot, 
                BehaviorEnumSet.AttackLevel.Technique, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            MoveCommand = new List<BehaviorEnumSet.InputSet>()
            {
                BehaviorEnumSet.InputSet.Down,
                BehaviorEnumSet.InputSet.Forward
            };
            AttackTrigger = BehaviorEnumSet.Behavior.Kick;
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.Crouch);
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.OnGround);
            CommandManager.AddCommand(
                MoveCommand, 
                AttackTrigger, 
                AvailableCommandPositionCondition,
                BehaviorEnumSet.Behavior.StandingKick236Skill);
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
            
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
                
            _moveVelocity = (CharacterTransform.transform.forward.x > 0.0f)
                ? (Vector3.right * _moveSpeed)
                : (Vector3.left *_moveSpeed);
            CharacterRigidBody.velocity = _moveVelocity;
            
            CharacterAnimator.PlayAnimation("StandingKick236Skill", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingKick236Skill", CharacterAnimator.Layer.LowerLayer,true);
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
            
            
            if(CharacterAnimator.IsEndCurrentAnimation("StandingKick236Skill", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.StandingKick236Skill);
        }
    }
}