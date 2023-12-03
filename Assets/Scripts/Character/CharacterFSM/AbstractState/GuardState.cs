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
        private bool _isPlayerVelocityStoppedByWall;

        private bool _isPressGuardKeyContinuous;
        
        public float ContinuousTimeByBlockAttack { get; set; }
        
        public override void Enter()
        {
            _isPlayerVelocityStoppedByWall = false;
            _isPressGuardKeyContinuous = true;
            ContinuousTimeByBlockAttack = 0.0f;
            CharacterAnimator.PlayAnimation("Guard", CharacterAnimator.Layer.UpperLayer, true);
        }

        public override void HandleInput(BehaviorEnumSet.Behavior behavior)
        {
            PressGuardKey(behavior);
            
            switch (behavior)
            {
                default:
                    break;
            }
        }

        public override void UpdateState()
        {
            ContinuousTimeByBlockAttack -= Time.deltaTime;
            if (PlayerCharacter.IsHitContinuous && !_isPlayerVelocityStoppedByWall && CharacterRigidBody.velocity.x == 0.0f)
            {
                if (_previousFrameVelocity == Vector3.zero)
                    _previousFrameVelocity = _initBackWardVelocity;
                _enemyRigidbody.velocity = _previousFrameVelocity * (-1.0f);
                _isPlayerVelocityStoppedByWall = true;
            }
            
            _previousFrameVelocity = CharacterRigidBody.velocity;
            
            if (!_isPressGuardKeyContinuous && ContinuousTimeByBlockAttack < 0.0f)
                StateManager.ChangeState(_nextState);
            else _isPressGuardKeyContinuous = false;
        }

        public override void Quit()
        {
            return;
        }

        protected void PressGuardKey(BehaviorEnumSet.Behavior behavior)
        {
            if (behavior == BehaviorEnumSet.Behavior.Guard) _isPressGuardKeyContinuous = true;
        }
    }
}