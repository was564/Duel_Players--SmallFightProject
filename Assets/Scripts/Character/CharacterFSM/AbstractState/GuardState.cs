using Unity.Mathematics;
using UnityEngine;

namespace Character.CharacterFSM
{
    public abstract class GuardState : BehaviorStateInterface
    {
        public GuardState(GameObject characterRoot, GameObject wall, BehaviorEnumSet.State guardStateName,
            BehaviorEnumSet.State nextState, PassiveStateEnumSet.CharacterPositionState positionState) :
            base(guardStateName, characterRoot, BehaviorEnumSet.AttackLevel.Guard, positionState)
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
        
        public int ContinuousFrameByBlockAttack { get; set; }
        
        public override void Enter()
        {
            _isPressGuardKeyContinuous = true;
            PlayerCharacter.IsGuarded = true;
            ContinuousFrameByBlockAttack = 0;
            CharacterAnimator.PlayAnimation("Guard", CharacterAnimator.Layer.UpperLayer, true);
        }

        public override BehaviorEnumSet.State GetResultStateByHandleInput(BehaviorEnumSet.Behavior behavior)
        {
            PressGuardKey(behavior);
            
            switch (behavior)
            {
                default:
                    return BehaviorEnumSet.State.Null;
            }
        }

        public override BehaviorEnumSet.State UpdateState()
        {
            ContinuousFrameByBlockAttack -= 1;

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

            if (ContinuousFrameByBlockAttack < 0)
                PlayerCharacter.IsHitContinuous = false;
            if (!_isPressGuardKeyContinuous && !PlayerCharacter.IsHitContinuous)
                return _nextState;
            else _isPressGuardKeyContinuous = false;

            return BehaviorEnumSet.State.Null;
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