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
    
    // Tuple은 생성은 느리지만 접근은 빠름
    // Reference : https://codingcoding.tistory.com/206
    // key : tuple (StateForPressed, StateForNotPressed)
    private Dictionary<Tuple<BehaviorEnumSet.Button, BehaviorEnumSet.Button>, InputAction> _opposingInputsActions
        = new Dictionary<Tuple<BehaviorEnumSet.Button, BehaviorEnumSet.Button>, InputAction>();
    
    private Queue<BehaviorEnumSet.Button> _inputQueue = new Queue<BehaviorEnumSet.Button>();

    public bool IsAvailableInput = true;

    public bool NeverUseInput = false;
    
    private void Start()
    {
        if(_inputPackage == null) return;
        _inputInBattle = _inputPackage.FindActionMap("Battle");
        _inputInBattle.Enable();
        
        _moveInputAction = _inputInBattle.FindAction("Move");

        _inputActions[BehaviorEnumSet.Button.Jump] = _inputInBattle.FindAction("Jump");
        _inputActions[BehaviorEnumSet.Button.Punch] = _inputInBattle.FindAction("Punch");
        _inputActions[BehaviorEnumSet.Button.Kick] = _inputInBattle.FindAction("Kick");
        _inputActions[BehaviorEnumSet.Button.Guard] = _inputInBattle.FindAction("Guard");
        _inputActions[BehaviorEnumSet.Button.Assist] = _inputInBattle.FindAction("Assist");
        
        _opposingInputsActions[Tuple.Create(BehaviorEnumSet.Button.Crouch, BehaviorEnumSet.Button.Stand)] 
            = _inputInBattle.FindAction("Crouch");
    }

    
    
    private void Update()
    {
        if(NeverUseInput || !IsAvailableInput) return;
        
        int previousCommandCount = _inputQueue.Count;

        // 입력 우선도는 조작키 > 공격키
        
        EnqueueMove(_moveInputAction);
        
        foreach (KeyValuePair<Tuple<BehaviorEnumSet.Button, BehaviorEnumSet.Button>, InputAction> input in _opposingInputsActions)
        {
            EnqueueOpposingInputs(input.Key, input.Value);
        }
        
        foreach (KeyValuePair<BehaviorEnumSet.Button, InputAction> input in _inputActions)
        {
            EnqueueInput(input.Key, input.Value);
        }
        
        /*
        if (_inputQueue.Count == previousCommandCount)
            _inputQueue.Enqueue(BehaviorEnumSet.Button.Idle);
        */
        
        return;
    }

    private void EnqueueMove(InputAction input)
    {
        float inputDirection = input.ReadValue<float>();
        float characterDirection = this.transform.root.forward.x;
        float resultDirection = inputDirection * characterDirection;
        if (resultDirection > 0.0f) _inputQueue.Enqueue(BehaviorEnumSet.Button.Forward);
        else if (resultDirection < 0.0f) _inputQueue.Enqueue(BehaviorEnumSet.Button.Backward);
        else if (resultDirection == 0.0f) _inputQueue.Enqueue(BehaviorEnumSet.Button.Stop);
    }
    
    private void EnqueueInput(BehaviorEnumSet.Button button, InputAction input)
    {
        if (!input.WasPressedThisFrame()) return;
        float pressed = input.ReadValue<float>();
        
        if(pressed > 0.0f) _inputQueue.Enqueue(button);
    }

    private void EnqueueOpposingInputs(Tuple<BehaviorEnumSet.Button, BehaviorEnumSet.Button> states, InputAction input)
    {
        (BehaviorEnumSet.Button pressedState, BehaviorEnumSet.Button notPressedState) = states;
        float pressed = input.ReadValue<float>();
        if(pressed > 0.0f) _inputQueue.Enqueue(pressedState);
        else _inputQueue.Enqueue(notPressedState);
    }
    
    public BehaviorEnumSet.Button DequeueInputQueue()
    {
        return _inputQueue.Dequeue();
    }
    
    public void EnqueueInputQueue(BehaviorEnumSet.Button button)
    {
        _inputQueue.Enqueue(button);
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


