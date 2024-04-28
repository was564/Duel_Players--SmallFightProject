using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.KohakuState.SkillState
{
    public class BackStepOnGroundState : SkillStateInterface
    {
        public BackStepOnGroundState(GameObject characterRoot)
            : base(BehaviorEnumSet.State.BackStepOnGroundState, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.InAir)
        {
            MoveCommand = new List<BehaviorEnumSet.InputSet>()
            {
                BehaviorEnumSet.InputSet.Backward,
                BehaviorEnumSet.InputSet.Idle,
                BehaviorEnumSet.InputSet.Backward
            };
            AttackTrigger = BehaviorEnumSet.Behavior.Null;
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.OnGround);
            CommandManager.AddCommand(
                MoveCommand,
                AttackTrigger,
                AvailableCommandPositionCondition,
                BehaviorEnumSet.Behavior.BackStep);
        }

        public override List<BehaviorEnumSet.InputSet> MoveCommand { get; protected set; }
        public override BehaviorEnumSet.Behavior AttackTrigger { get; protected set; }

        public override List<PassiveStateEnumSet.CharacterPositionState> AvailableCommandPositionCondition
            { get; protected set; } = new List<PassiveStateEnumSet.CharacterPositionState>();

        private Vector2 _backStepVelocity = new Vector2(5.0f, 0.4f);

        private Vector3 _finalVelocity;
        
        private float _yOffsetForLand = 0.2f;
        
        public override void Enter()
        {
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            
            _finalVelocity = ((CharacterTransform.transform.forward.x > 0.0f)
                ? (Vector3.left * _backStepVelocity.x) : (Vector3.right * _backStepVelocity.x))
                + Vector3.up * _backStepVelocity.y;
            CharacterRigidBody.velocity = _finalVelocity;
            CharacterAnimator.PlayAnimationSmoothly("BackStep", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("BackStep", CharacterAnimator.Layer.LowerLayer);
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
            if (this.CharacterTransform.position.y <= this.PlayerCharacter.PositionYOffsetForLand + _yOffsetForLand)
            {
                CharacterRigidBody.velocity = Vector3.zero;
                return BehaviorEnumSet.State.Land;
            }
            else return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            
        }
    }
}