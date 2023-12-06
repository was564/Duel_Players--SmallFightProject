using Unity.Mathematics;
using UnityEngine;

namespace Character.CharacterFSM
{
    public abstract class GuardState : BehaviorStateInterface
    {
        public GuardState(GameObject characterRoot, GameObject wall, BehaviorStateSimulator stateManager, BehaviorEnumSet.State guardStateName,
            BehaviorEnumSet.State nextState, PassiveStateEnumSet.CharacterPositionState positionState) :
            base(guardStateName, stateManager, characterRoot, BehaviorEnumSet.AttackLevel.Guard, positionState)
        {
            _wallAcknowledgeDistance = math.abs(wall.transform.position.x) - 1.0f;
            _enemyRigidbody = PlayerCharacter.EnemyObject.transform.root.GetComponent<Rigidbody>();
            _nextState = nextState;
        }

        private BehaviorEnumSet.State _nextState;
        private float _wallAcknowledgeDistance;
        
        private Rigidbody _enemyRigidbody;
        private Vector3 _previousFrameVelocity;
        private bool _isPlayerPositionClosedWithWall;

        private bool _isPressGuardKeyContinuous;
        
        public float ContinuousTimeByBlockAttack { get; set; }
        
        public override void Enter()
        {
            _isPressGuardKeyContinuous = true;
            PlayerCharacter.IsGuarded = true;
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

            if (PlayerCharacter.IsGuarded && _isPlayerPositionClosedWithWall)
            {
                if (PlayerCharacter.IsHitContinuous)
                    _enemyRigidbody.velocity = Vector3.right * ((CharacterTransform.forward.x < 0.0f ? -1.0f : 1.0f) * (3.0f));

                PlayerCharacter.IsGuarded = false;
            }
            
            if (math.abs(CharacterTransform.position.x) > _wallAcknowledgeDistance)
            {
                if (_isPlayerPositionClosedWithWall == false)
                    ExecuteFunctionWhenTouchTheWall();
                _isPlayerPositionClosedWithWall = true;
            }
            else _isPlayerPositionClosedWithWall = false;
            //Debug.Log(PlayerCharacter.name + _isPlayerPositionClosedWithWall);
            
            _previousFrameVelocity = CharacterRigidBody.velocity;

            if (ContinuousTimeByBlockAttack < 0.0f)
                PlayerCharacter.IsHitContinuous = false;
            if (!_isPressGuardKeyContinuous && !PlayerCharacter.IsHitContinuous)
                StateManager.ChangeState(_nextState);
            else _isPressGuardKeyContinuous = false;
        }

        public override void Quit()
        {
            PlayerCharacter.IsGuarded = false;
            return;
        }

        protected void PressGuardKey(BehaviorEnumSet.Behavior behavior)
        {
            if (behavior == BehaviorEnumSet.Behavior.Guard) _isPressGuardKeyContinuous = true;
        }

        private void ExecuteFunctionWhenTouchTheWall()
        {
            if (PlayerCharacter.IsHitContinuous)
                _enemyRigidbody.velocity = _previousFrameVelocity * (-1.0f);
        }
    }
}