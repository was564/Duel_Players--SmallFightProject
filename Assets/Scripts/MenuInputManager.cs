using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputPackage;

    private InputActionMap _inputInMenu;

    private InputAction _verticalScrollKey; // up, down
    private InputAction _optionChangeKey; // left, right

    private InputAction _selectionKey;

    private InputAction _enterMenuKey;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputInMenu = _inputPackage.FindActionMap("Menu");

        _verticalScrollKey = _inputInMenu.FindAction("VerticalMove");
        _optionChangeKey = _inputInMenu.FindAction("HorizontalMove");
        
        _selectionKey = _inputInMenu.FindAction("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
