using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    // FSM 매니저로 가게끔 만들기
    //private CharacterAnimator _animator;
    private BehaviorStateManager _stateManager;
    private PlayerCharacter _playerCharacter;
    
    private BoxCollider _hitBox;

    private FrameManager _gameManager;

    private Rigidbody _rigidbody;

    [SerializeField] private float _pauseTime = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        //_animator = this.GetComponentInParent<CharacterAnimator>();
        _playerCharacter = this.transform.root.GetComponent<PlayerCharacter>();
        _hitBox = this.GetComponent<BoxCollider>();
        _gameManager = GameObject.FindObjectOfType<FrameManager>();
        _rigidbody = this.transform.root.GetComponent<Rigidbody>();
        
        _stateManager = _playerCharacter.StateManager;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.tag.Equals(this.tag))
        {
            AttackBox attackInfo = col.GetComponent<AttackBox>();

            BehaviorEnumSet.State currentState = _stateManager.CurrentState.StateName;
            if (currentState == BehaviorEnumSet.State.CrouchGuard ||
                currentState == BehaviorEnumSet.State.StandingGuard)
            {
                _gameManager.PauseAllCharactersInTime(_pauseTime);
                _rigidbody.velocity = this.transform.forward.normalized * (-2.0f);
                return;
            }
            
            switch (_playerCharacter.CharacterPositionState)
            {
                case PassiveStateEnumSet.CharacterPositionState.OnGround:
                    _stateManager.ForceChangeState(BehaviorEnumSet.State.StandingHit);
                    break;
                case PassiveStateEnumSet.CharacterPositionState.InAir:
                    _stateManager.ForceChangeState(BehaviorEnumSet.State.InAirHit);
                    break;
                case PassiveStateEnumSet.CharacterPositionState.Crouch:
                    _stateManager.ForceChangeState(BehaviorEnumSet.State.CrouchHit);
                    break;
            }
            _playerCharacter.DecreaseHp(attackInfo.Damage);
            _gameManager.PauseAllCharactersInTime(_pauseTime);
        }
    }
}
