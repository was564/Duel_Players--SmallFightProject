using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorStateInterface : MonoBehaviour
{
    protected CharacterAnimator _animator;
    protected Transform _charaterTransform;

    private void Start()
    {
        _animator = this.GetComponent<CharacterAnimator>();
        _charaterTransform = this.GetComponent<Transform>().root;
    }
    
    public abstract void Enter();

    public abstract void StateUpdate();
    
    public abstract void Quit();
}

/*
 * 나중에 해당 상태를 몇 초동안 있어야 하고 그런 규약이 생기면
 * 상속한 스테이트에 그런 규약에 맞는 코드를 작성 후
 * 거기에 적용될 숫자는 생성자에서 할당하도록 만들기
 * 그리고 생성자에 넣고 싶은 값은 캐릭터 별로 Json같은 기록할만한 곳에 해두기
 */

enum BehaviorState
{
    Idle = 0,
    Punch,
    Size
}