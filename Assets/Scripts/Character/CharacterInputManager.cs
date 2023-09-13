using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterInputManager : MonoBehaviour
{
    private Queue<Behavior.Button> _inputQueue = new Queue<Behavior.Button>();

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("Punch"))
        {
            _inputQueue.Enqueue(Behavior.Button.Punch);
        }
        else
        {
            _inputQueue.Enqueue(Behavior.Button.Idle);
        }

        return;
    }

    public Behavior.Button DequeueInputQueue()
    {
        return _inputQueue.Dequeue();
    }

    public Behavior.Button PeekInputQueue()
    {
        return _inputQueue.Peek();
    }

    public bool isEmptyQueue()
    {
        return _inputQueue.Count <= 0;
    }
}
