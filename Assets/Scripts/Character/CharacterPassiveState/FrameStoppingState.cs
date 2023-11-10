using Character.CharacterFSM;
using UnityEngine;

namespace Character.CharacterPassiveState
{
    
    // Frame Stop 클래스를 Behavior State 클래스로 진행하려 했으나
    // 중단된 State의 재개를 구현하기 불편하다 생각하여 Passive State로 구현
    public class FrameStoppingState : PassiveStateInterface
    {
        public FrameStoppingState(GameObject characterRoot) : base(characterRoot)
        {
            _animator = characterRoot.GetComponent<CharacterAnimator>();
            _chatacter = characterRoot.GetComponent<PlayerCharacter>();
            _rigidbody = characterRoot.GetComponent<Rigidbody>();

            _passiveManager = characterRoot.GetComponent<PassiveStateManager>();
        }
        
        private PassiveStateManager _passiveManager;

        private BehaviorStateSimulator _stateSimulator;
        
        private CharacterAnimator _animator;
        private PlayerCharacter _chatacter;
        private Rigidbody _rigidbody;
        
        private PassiveStateEnumSet.CharacterPositionState _previousState;
        private Vector3 _previousVelocity; 
        
        public override void EnterPassiveState()
        {
            _previousVelocity = _rigidbody.velocity;
            _previousState = _chatacter.CharacterPositionState;
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            _chatacter.Stop();
            _animator.PauseAnimation();
        }

        public override void UpdatePassiveState()
        {
            this.RemainTime -= Time.deltaTime;
            _passiveManager.AddRemainTimeForAllState(Time.deltaTime);
        }

        public override void QuitPassiveState()
        {
            _chatacter.ChangeCharacterPosition(_previousState, true);
            _chatacter.Resume();
            _rigidbody.velocity = _previousVelocity;
            _animator.ResumeAnimation();
        }
    }
}