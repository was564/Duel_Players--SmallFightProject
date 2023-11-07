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

    public enum Layer
    {
        UpperLayer,
        LowerLayer,
        Size
    }
    
    // 참고 : https://stackoverflow.com/questions/441309/why-are-mutable-structs-evil
    private class AnimationInfo
    {
        public string AnimationName { get; set; } = "StandingIdle";
        // public bool WillAnimationChangeInOneFrame { get; set; } = false;
        public bool IsStartedAtAnimationStarting { get; set; } = false;

        public bool IsSmoothAnimation { get; set; } = false;

        public float AnimationStartingNormalizedTime { get; set; } = 0.0f;
    }
    
    private Dictionary<Layer, AnimationInfo>  _animationInfoByLayer = new Dictionary<Layer, AnimationInfo>()
    {
        {Layer.UpperLayer, new AnimationInfo()},
        {Layer.LowerLayer, new AnimationInfo()}
    };

    // 참고 : https://stackoverflow.com/questions/150750/hashset-vs-list-performance
    private HashSet<Layer> _animationWillBeChangedSet = new HashSet<Layer>();
    
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

        foreach (Layer layer in _animationWillBeChangedSet)
        {
            AnimationInfo animationInfo = _animationInfoByLayer[layer];
            int layerIndex = (int)layer;
            
            // Standing Hit 같은 상태는 Update를 거치지 않고 GetCurrentAnimationDuration을 갈 수 있기 때문에 버그 발생
            // 따라서 PlayAnimation 부분에서 할당하도록 지정
            // _animationStartingNormalizedTime[layerIndex] = _animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
            if (animationInfo.IsSmoothAnimation)
            {
                if (animationInfo.IsStartedAtAnimationStarting) 
                    _animator.CrossFade(animationInfo.AnimationName, 0.1f, layerIndex, 0.0f);
                else _animator.CrossFade(animationInfo.AnimationName, 0.1f, layerIndex);
            }
            else
            {
                if (animationInfo.IsStartedAtAnimationStarting)
                    _animator.Play(animationInfo.AnimationName, layerIndex, 0.0f);
                else _animator.Play(animationInfo.AnimationName, layerIndex);
            }
        }
        _animationWillBeChangedSet.Clear();
    }

    public void InitializeParameterInIdle()
    {
        foreach (var initializedparameter in _initializedParametersValueInAnimator)
        {
            _animator.SetBool(initializedparameter.Key, initializedparameter.Value);
        }
    }

    public float GetCurrentAnimationDuration(Layer layer)
    {
        return _animator.GetCurrentAnimatorStateInfo((int)layer).normalizedTime;
    }
    
    // 참고 : https://icat2048.tistory.com/487 (Unity Handler를 이용한 애니메이션 종료 확인)
    public bool IsEndCurrentAnimation(string animationName, Layer layer)
    {
        int layerIndex = (int)layer;
        if (!_animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animationName))
        {
            Debug.Assert(true, "State and Animation Mismatch : " + _animator.GetCurrentAnimatorStateInfo((int)layer).ToString());
        }

        // normalizeTime이 재생된 직후에는 초기화 되지 않는 현상 발생
        // 따라서 그런 상황에만 해당 프레임 스킵
        if (_animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime.Equals(
                _animationInfoByLayer[layer].AnimationStartingNormalizedTime)) 
            return false;

        if (_animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 1.0f)
        {
            // Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            return true;
        }

        return false;
    }

    // https://sangh518.github.io/record/animationCheck/
    // 일부 애니메이션이 재생 될 때 전 애니메이션의 NormalizedTime이 초기화 되지 않는 현상
    // animator Play를 같은 프레임에 2번 이상 호출하면 첫번째를 제외한 나머지 함수가 무시됨.
    // 따라서 다 결정하게 한뒤 마지막에 애니메이터를 재생하게끔 하기
    public void PlayAnimationSmoothly(string animationName, Layer layer, bool atStarting = false)
    {
        AnimationInfo animationInfo = _animationInfoByLayer[layer];
        
        animationInfo.AnimationName = animationName;
        animationInfo.IsStartedAtAnimationStarting = atStarting;
        animationInfo.IsSmoothAnimation = true;
        animationInfo.AnimationStartingNormalizedTime =
            _animator.GetCurrentAnimatorStateInfo((int)layer).normalizedTime;
        
        _animationWillBeChangedSet.Add(layer);
    }
 
    public void PlayAnimation(string animationName, Layer layer, bool atStarting = false)
    {
        AnimationInfo animationInfo = _animationInfoByLayer[layer];

        animationInfo.AnimationName = animationName;
        animationInfo.IsStartedAtAnimationStarting = atStarting;
        animationInfo.IsSmoothAnimation = false;
        animationInfo.AnimationStartingNormalizedTime =
            _animator.GetCurrentAnimatorStateInfo((int)layer).normalizedTime;

        _animationWillBeChangedSet.Add(layer);
    }

    public void PauseAnimation()
    {
        _animator.speed = 0.0f;
    }

    public void ResumeAnimation()
    {
        _animator.speed = 1.0f;
    }
}
