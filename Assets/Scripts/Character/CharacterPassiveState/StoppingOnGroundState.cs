using UnityEngine;

namespace Character.CharacterPassiveState
{
    public class StoppingOnGroundState : PassiveStateInterface
    {
        public StoppingOnGroundState(GameObject characterRoot) : base(characterRoot)
        {
            _chatacter = characterRoot.GetComponent<CharacterStructure>();
            _characterTransform = characterRoot.GetComponent<Rigidbody>();
        }

        private CharacterStructure _chatacter;
        private Rigidbody _characterTransform;
        
        public override void EnterPassiveState()
        {
            
        }

        public override void UpdatePassiveState()
        {
            if (_chatacter.CharacterPositionState != PassiveStateEnumSet.CharacterPositionState.OnGround) return;
            
            _characterTransform.velocity *= 0.9f;
            if (_characterTransform.velocity.sqrMagnitude <= 0.1f)
                _characterTransform.velocity = Vector3.zero;
        }

        public override void QuitPassiveState()
        {
            
        }
    }
}