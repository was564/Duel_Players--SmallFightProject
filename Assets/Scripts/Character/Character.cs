using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterInputManager _inputManager;
    private CharacterAnimator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputManager = this.GetComponent<CharacterInputManager>();
        _animator = this.GetComponent<CharacterAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        BehaviorByInput();
    }

    public void BehaviorByInput()
    {
        if (_inputManager.isEmptyQueue()) return;
        Behavior.Button input = _inputManager.DequeueInputQueue();
        
        switch (input)
        {
            case Behavior.Button.Idle:
                break;
            case Behavior.Button.Crouch:
                break;
            case Behavior.Button.Punch:
                Behavior.AttackName attackName = JudgeAttackNameOnlyPunch();
                _animator.animateByAttackNameInBehavior(attackName);
                break;
        }
    }

    private Behavior.AttackName JudgeAttackNameOnlyPunch()
    {
        return Behavior.AttackName.Punch;
    }
}

public class Behavior
{
    public enum Button{
        Idle = 0,
        Crouch,
        Punch,
        Move,
        Size
    }

    public enum AttackName
    {
        Punch = 0,
        Size
    }
}