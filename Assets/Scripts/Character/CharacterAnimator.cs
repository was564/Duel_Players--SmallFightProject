using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private Dictionary<string, bool> _initializedParametersValueInAnimator = new Dictionary<string, bool>();
    private Transform _characterRootTransform;


    private void Start()
    {
        this._characterRootTransform = this.transform.root;
        this._animator = GetComponent<Animator>();
        
        foreach (var parameter in _animator.parameters)
        {
            _initializedParametersValueInAnimator.Add(parameter.name, _animator.GetBool(parameter.name)); 
        }
        
        return;
    }

    private void Update()
    {
        if(_characterRootTransform.forward.x > 0)
            _animator.SetBool("IsRight", false);
        
        else if(_characterRootTransform.forward.x < 0)
            _animator.SetBool("IsRight", true);
    }

    public void InitializeParameterInIdle()
    {
        foreach (var initializedparameter in _initializedParametersValueInAnimator)
        {
            _animator.SetBool(initializedparameter.Key, initializedparameter.Value);
        }
    }

    public bool IsEndCurrentAnimation(string animationName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("animationName"))
        {
            Debug.Assert(true, "State and Animation Mismatch : " + _animator.GetCurrentAnimatorStateInfo(0).ToString());
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            return true;
        }

        return false;
    }

    public void PlayAnimationSmoothly(string animationName, bool atStarting = false)
    {
        if (atStarting) _animator.CrossFade(animationName, 0.1f, -1, 0.0f);
        else _animator.CrossFade(animationName, 0.1f);
    }

    public void PlayAnimation(string animationName, bool atStarting = false)
    {
        if (atStarting) _animator.Play(animationName, -1, 0.0f);
        else _animator.Play(animationName);
    }
    
    public void EndHitAnimation()
    {
        _animator.SetBool("HitEnd", true);
    }
}
