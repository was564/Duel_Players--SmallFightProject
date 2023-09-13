using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private Dictionary<string, bool> _initializedParametersValueInAnimator = new Dictionary<string, bool>();


    private void Start()
    {
        this._animator = GetComponent<Animator>();

        foreach (var parameter in _animator.parameters)
        {
            _initializedParametersValueInAnimator.Add(parameter.name, _animator.GetBool(parameter.name)); 
        }
        
        return;
    }

    public void InitialzeIdle()
    {
        foreach (var initializedparameter in _initializedParametersValueInAnimator)
        {
            _animator.SetBool(initializedparameter.Key, initializedparameter.Value);
        }
    }

    public void HitByAttack()
    {
        _animator.SetTrigger("Hitted");
    }
    
    public void EndHitAnimation()
    {
        _animator.SetBool("HitEnd", true);
    }

    public void EndAttackAnimation()
    {
        _animator.SetBool("AttackEnd", true);
        _animator.SetBool("PunchKey", false);
    }

    public void animateByAttackNameInBehavior(Behavior.AttackName attackName)
    {
        switch (attackName)
        {
            case Behavior.AttackName.Punch:
                Punch();
                break;
        }
    }
    
    private void Punch()
    {
        _animator.SetBool("PunchKey", true);
    }
}
