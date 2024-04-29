using System.Collections.Generic;
using UnityEngine;

namespace Character.CharacterFSM.KohakuState.SkillState
{
    public class AiringDashState : SkillStateInterface
    {
        private ParticleSystem _dashEffect;
        
        public AiringDashState(GameObject characterRoot)
            : base(BehaviorEnumSet.State.DashOnGround, characterRoot, 
                BehaviorEnumSet.AttackLevel.Move, PassiveStateEnumSet.CharacterPositionState.OnGround)
        {
            foreach (var childTransform in characterRoot.transform.GetComponentsInChildren<ParticleSystem>())
            {
                if (childTransform.tag.Equals("RunParticle"))
                    _dashEffect = childTransform.GetComponent<ParticleSystem>();
            }
            _dashEffect.Stop();
            
            MoveCommand = new List<BehaviorEnumSet.InputSet>()
            {
                BehaviorEnumSet.InputSet.Forward,
                BehaviorEnumSet.InputSet.Idle,
                BehaviorEnumSet.InputSet.Forward
            };
            AttackTrigger = BehaviorEnumSet.Behavior.Null;
            AvailableCommandPositionCondition.Add(PassiveStateEnumSet.CharacterPositionState.InAir);
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
            PlayerCharacter.ChangeCharacterPosition(CharacterPositionInitialState);
            PlayerCharacter.IsDoubleJumped = true;
            
            _finalVelocity = (CharacterTransform.transform.forward.x > 0.0f)
                ? (Vector3.right * _dashVelocity)
                : (Vector3.left * _dashVelocity);
            CharacterRigidBody.velocity = _finalVelocity;
            CharacterAnimator.PlayAnimationSmoothly("Dash", CharacterAnimator.Layer.UpperLayer);
            CharacterAnimator.PlayAnimationSmoothly("Dash", CharacterAnimator.Layer.LowerLayer);
            
            _dashEffect.Play();
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
            CharacterRigidBody.velocity = _finalVelocity;

            return BehaviorEnumSet.State.Null;
        }

        public override void Quit()
        {
            CharacterAnimator.PlayAnimationSmoothly("StandingIdle", CharacterAnimator.Layer.LowerLayer);
            _dashEffect.Stop();
        }
    }
}