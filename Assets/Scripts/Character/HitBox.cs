using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    // FSM 매니저로 가게끔 만들기
    //private CharacterAnimator _animator;
    private BehaviorStateManager _stateManager;
    private CharacterStructure _character;
    
    private BoxCollider _hitBox;

    private FrameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        //_animator = this.GetComponentInParent<CharacterAnimator>();
        _stateManager = this.transform.root.GetComponent<BehaviorStateManager>();
        _character = this.transform.root.GetComponent<CharacterStructure>();
        _hitBox = this.GetComponent<BoxCollider>();
        _gameManager = GameObject.FindObjectOfType<FrameManager>();
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
                _gameManager.PauseAllCharacters(0.2f);
                return;
            }

            _character.DecreaseHp(attackInfo.Damage);
            switch (_character.CharacterPositionState)
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
            _gameManager.PauseAllCharacters(0.2f);
        }
    }
}
