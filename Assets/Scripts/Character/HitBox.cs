using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    
    private CharacterAnimator _animator;
    private BoxCollider _hitArea;
    
    // Start is called before the first frame update
    void Start()
    {
        
        _animator = this.GetComponentInParent<CharacterAnimator>();
        _hitArea = this.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.tag.Equals(this.tag))
        {
            _animator.HitByAttack();
        }
    }
}
