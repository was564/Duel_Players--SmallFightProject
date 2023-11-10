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

    [SerializeField] private float _pauseTime = 0.15f;
    
    // Start is called before the first frame update
    void Start()
    {
        //_animator = this.GetComponentInParent<CharacterAnimator>();
        _playerCharacter = this.transform.root.GetComponent<PlayerCharacter>();
        _hitBox = this.GetComponent<BoxCollider>();
        _gameManager = GameObject.FindObjectOfType<FrameManager>();
        
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
                _gameManager.PauseAllCharacters(_pauseTime);
                return;
            }

            _playerCharacter.DecreaseHp(attackInfo.Damage);
            switch (_playerCharacter.CharacterPositionState)
            {
                case PassiveStateEnumSet.CharacterPositionState.OnGround:
                    _stateManager.ChangeState(BehaviorEnumSet.State.StandingHit);
                    break;
                case PassiveStateEnumSet.CharacterPositionState.InAir:
                    _stateManager.ChangeState(BehaviorEnumSet.State.InAirHit);
                    break;
                case PassiveStateEnumSet.CharacterPositionState.Crouch:
                    _stateManager.ChangeState(BehaviorEnumSet.State.CrouchHit);
                    break;
            }
            _gameManager.PauseAllCharacters(_pauseTime);
        }
    }
}
