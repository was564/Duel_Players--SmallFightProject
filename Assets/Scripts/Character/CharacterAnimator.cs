using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterAnimator : MonoBehaviour
{
    private Animator _animator;
    private Dictionary<string, bool> _initializedParametersValueInAnimator = new Dictionary<string, bool>();
    private Transform _characterRootTransform;

    private float _animationStartingNormalizedTime;

    private void Start()
    {
        this._characterRootTransform = this.transform.root;
        this._animator = this.GetComponent<Animator>();
        
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

    public float GetCurrentAnimationDuration()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    
    public bool IsEndCurrentAnimation(string animationName)
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            Debug.Assert(true, "State and Animation Mismatch : " + _animator.GetCurrentAnimatorStateInfo(0).ToString());
        }

        // normalizeTime이 재생된 직후에는 초기화 되지 않는 현상 발생
        // 따라서 그런 상황에만 해당 프레임 스킵
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime.Equals(_animationStartingNormalizedTime)) return false;

        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            return true;
        }

        return false;
    }

    // https://sangh518.github.io/record/animationCheck/
    // 일부 애니메이션이 재생 될 때 전 애니메이션의 NormalizedTime이 초기화 되지 않는 현상
    public void PlayAnimationSmoothly(string animationName, bool atStarting = false)
    {
        _animationStartingNormalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (atStarting) _animator.CrossFade(animationName, 0.1f, -1, 0.0f);
        else _animator.CrossFade(animationName, 0.1f);
    }
 
    public void PlayAnimation(string animationName, bool atStarting = false)
    {
        _animationStartingNormalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (atStarting) _animator.Play(animationName, -1, 0.0f);
        else _animator.Play(animationName);
    }
    
}
