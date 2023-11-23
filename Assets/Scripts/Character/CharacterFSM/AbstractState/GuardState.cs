using UnityEngine;

namespace Character.CharacterFSM
{
    public abstract class GuardState : BehaviorStateInterface
    {
        public GuardState(GameObject characterRoot, BehaviorStateSimulator stateManager, BehaviorEnumSet.State guardStateName,
            BehaviorEnumSet.State nextState) :
            base(guardStateName, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.Guard)
        {
            _enemyRigidbody = PlayerCharacter.EnemyObject.GetComponent<Rigidbody>();
            _initBackWardVelocity = Vector3.right * (CharacterTransform.forward.x < 0.0f ? -1.0f : 1.0f) * (-3.0f);
            _nextState = nextState;
        }

        private BehaviorEnumSet.State _nextState;

        private Rigidbody _enemyRigidbody;
        private Vector3 _previousFrameVelocity;
        private Vector3 _initBackWardVelocity;
        private bool isStopped;
        
        
        public override void Enter()
        {
            isStopped = false;
            CharacterAnimator.PlayAnimation("Guard", CharacterAnimator.Layer.UpperLayer, true);
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
            if (PlayerCharacter.IsHitContinuous && !isStopped && CharacterRigidBody.velocity.x == 0.0f)
            {
                if (_previousFrameVelocity == Vector3.zero)
                    _previousFrameVelocity = _initBackWardVelocity;
                _enemyRigidbody.velocity = _previousFrameVelocity * (-1.0f);
                isStopped = true;
            }

            _previousFrameVelocity = CharacterRigidBody.velocity;
            if(CharacterAnimator.IsEndCurrentAnimation("Guard", CharacterAnimator.Layer.UpperLayer))
                StateManager.ChangeState(_nextState);
        }

        public override void Quit()
        {
            
        }
    }
}