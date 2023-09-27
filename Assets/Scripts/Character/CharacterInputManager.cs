using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CharacterInputManager : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _inputPackage;

    private InputActionMap _inputInBattle;
    // private InputActionMap _inputInMenu;

    // dictionary의 Key로 Enum은 성능 하락 발생 (GetHashCode 같은 것이 없음)
    // Unity .Net 4 이상으로 가서 문제 없어짐
    // Reference : https://pizzasheepsdev.tistory.com/2
    private InputAction _moveInputAction;
    private Dictionary<BehaviorEnumSet.Button, InputAction> _inputActions 
        = new Dictionary<BehaviorEnumSet.Button, InputAction>();
    private Queue<BehaviorEnumSet.Button> _inputQueue = new Queue<BehaviorEnumSet.Button>();
    
    private void Start()
    {
        _inputInBattle = _inputPackage.FindActionMap("Battle");

        _moveInputAction = _inputInBattle.FindAction("Move");
        _inputActions[BehaviorEnumSet.Button.Jump] = _inputInBattle.FindAction("Jump");
        _inputActions[BehaviorEnumSet.Button.Crouch] = _inputInBattle.FindAction("Crouch");
        _inputActions[BehaviorEnumSet.Button.Punch] = _inputInBattle.FindAction("Punch");
        _inputActions[BehaviorEnumSet.Button.Kick] = _inputInBattle.FindAction("Kick");
        _inputActions[BehaviorEnumSet.Button.Guard] = _inputInBattle.FindAction("Guard");
        _inputActions[BehaviorEnumSet.Button.Assist] = _inputInBattle.FindAction("Assist");
    }

    
    
    private void Update()
    {
        int previousCommandCount = _inputQueue.Count;

        EnqueueMove(_moveInputAction);
        
        foreach (KeyValuePair<BehaviorEnumSet.Button, InputAction> input in _inputActions)
        {
            EnqueueInput(input.Key, input.Value);
        }

        if (_inputQueue.Count == previousCommandCount)
            _inputQueue.Enqueue(BehaviorEnumSet.Button.Idle);
        
        /*
        if (Input.GetKeyDown("Punch"))
        {
            _inputQueue.Enqueue(BehaviorEnumSet.Button.Punch);
        }
        else
        {
            _inputQueue.Enqueue(BehaviorEnumSet.Button.Idle);
        }
        */
        
        return;
    }

    private void EnqueueMove(InputAction input)
    {
        float inputDirection = input.ReadValue<float>();
        float characterDirection = this.transform.root.forward.x;
        if (inputDirection * characterDirection > 0.0f) _inputQueue.Enqueue(BehaviorEnumSet.Button.Forward);
        else if (inputDirection * characterDirection < 0.0f) _inputQueue.Enqueue(BehaviorEnumSet.Button.Backward);
    }
    
    private void EnqueueInput(BehaviorEnumSet.Button button, InputAction input)
    {
        float pressed = input.ReadValue<float>();
        if(pressed > 0.0f) _inputQueue.Enqueue(button);
    }
    
    public BehaviorEnumSet.Button DequeueInputQueue()
    {
        return _inputQueue.Dequeue();
    }

    public BehaviorEnumSet.Button PeekInputQueue()
    {
        return _inputQueue.Peek();
    }

    public bool isEmptyInputQueue()
    {
        return _inputQueue.Count <= 0;
    }
}


