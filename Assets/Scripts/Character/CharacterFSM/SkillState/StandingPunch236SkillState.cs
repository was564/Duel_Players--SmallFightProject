using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.SkillState
{
    public class StandingPunch236SkillState : SkillStateInterface
    {
        public StandingPunch236SkillState(GameObject characterRoot, BehaviorStateSimulator stateManager)
            : base(BehaviorEnumSet.State.StandingPunch236Skill, stateManager, characterRoot, 
                BehaviorEnumSet.AttackLevel.Technique, PassiveStateEnumSet.CharacterPositionState.OnGround)
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
                BehaviorEnumSet.Behavior.StandingPunch236Skill);
        }

        public override List<BehaviorEnumSet.InputSet> MoveCommand { get; protected set; }
        public override BehaviorEnumSet.Behavior AttackTrigger { get; protected set; }
        public override List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionCondition { get; protected set; }
            = new List<PassiveStateEnumSet.CharacterPositionState>();

        private float _startingFrame;
        private float _moveSpeed = 1.0f;
        private Vector3 _moveVelocity;
        
        public override void Enter()
        {
            _startingFrame = FrameManager.CurrentFrame;
            
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionStateInCurrentState);
                
            _moveVelocity = (CharacterTransform.transform.forward.x > 0.0f)
                ? (Vector3.right * _moveSpeed)
                : (Vector3.left *_moveSpeed);
            CharacterRigidBody.velocity = _moveVelocity;
            
            CharacterAnimator.PlayAnimation("StandingPunch236Skill", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingPunch236Skill", CharacterAnimator.Layer.LowerLayer,true);
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
            if (FrameManager.CurrentFrame - _startingFrame <= 24)
                CharacterRigidBody.velocity = _moveVelocity;
            
            
            if(CharacterAnimator.IsEndCurrentAnimation("StandingPunch236Skill", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(BehaviorEnumSet.State.StandingIdle);
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.StandingPunch236Skill);
        }
    }
}