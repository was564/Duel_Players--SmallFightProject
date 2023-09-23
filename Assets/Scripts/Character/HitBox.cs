using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    // FSM 매니저로 가게끔 만들기
    private CharacterAnimator _animator;
    
    private BoxCollider _hitBox;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponentInParent<CharacterAnimator>();
        _hitBox = this.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.tag.Equals(this.tag))
        {
            _animator.HitByAttack();
        }
    }
}
