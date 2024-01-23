using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.KohakuState.SkillState
{
    public class StandingKick236SkillState : SkillStateInterface
    {
        public StandingKick236SkillState(GameObject characterRoot)
            : base(BehaviorEnumSet.State.StandingKick236Skill, characterRoot, 
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

        private int _startingFrame;
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
            
            CharacterAnimator.PlayAnimation("StandingKick236Skill", CharacterAnimator.Layer.UpperLayer,true);
            CharacterAnimator.PlayAnimation("StandingKick236Skill", CharacterAnimator.Layer.LowerLayer,true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            if (FrameManager.CurrentFrame - _startingFrame <= 24)
                CharacterRigidBody.velocity = _moveVelocity;

            if (CharacterAnimator.IsEndCurrentAnimation("StandingKick236Skill", CharacterAnimator.Layer.UpperLayer))
                return BehaviorEnumSet.State.StandingIdle;
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterJudgeBoxController.DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName.StandingKick236Skill);
        }
    }
}