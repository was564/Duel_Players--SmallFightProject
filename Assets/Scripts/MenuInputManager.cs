using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    
    private MenuInGame _menu;
    
    [SerializeField] private InputActionAsset _inputPackage;
    
    private InputActionMap _inputInMenu;

    private InputAction _verticalScrollKey; // up, down
    private InputAction _optionChangeKey; // left, right

    private InputAction _selectionKey;

    private InputAction _enterMenuKey;
    
    // Start is called before the first frame update
    void Start()
    {
        _menu = GameObject.FindObjectOfType<MenuInGame>();
        
        _inputInMenu = _inputPackage.FindActionMap("Menu");
        _inputInMenu.Enable();
        
        _verticalScrollKey = _inputInMenu.FindAction("VerticalMove");
        _optionChangeKey = _inputInMenu.FindAction("HorizontalMove");
        
        _selectionKey = _inputInMenu.FindAction("Select");

        _enterMenuKey = _inputInMenu.FindAction("EnterMenu");
    }

    // Update is called once per frame
    void Update()
    {
        EnterMenu();
        MoveMenuOption();
        SelectMenuOption();
    }

    private void MoveMenuOption()
    {
        if (!_verticalScrollKey.WasPressedThisFrame()) return;
        float optionMoveDirection = _verticalScrollKey.ReadValue<float>();
        
        if(optionMoveDirection < 0.0f) _menu.MenuScrollDown();
        else if(optionMoveDirection > 0.0f) _menu.MenuScrollUp();
    }

    private void SelectMenuOption()
    {
        if (!_selectionKey.WasPressedThisFrame()) return;
        float selectButton = _selectionKey.ReadValue<float>();
        
        if(selectButton > 0.0f) _menu.SelectMenuOption();
    }

    private void EnterMenu()
    {
        if (!_enterMenuKey.WasPressedThisFrame()) return;
        float menuOpenButton = _enterMenuKey.ReadValue<float>();
        
        if (menuOpenButton <= 0.0f) return;
        _menu.ControlMenu();
    }
}
