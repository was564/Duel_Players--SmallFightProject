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

    // 0 : no input, 1 : up, -1 : down
    public short GetPressedMoveKey()
    {
        if (!_verticalScrollKey.WasPressedThisFrame()) return 0;
        float optionMoveDirection = _verticalScrollKey.ReadValue<float>();

        if (optionMoveDirection < 0.0f) return -1; //_menu.MenuScrollDown();
        else if (optionMoveDirection > 0.0f) return 1; //_menu.MenuScrollUp();
        else return 0;
    }
    
    public bool IsPressedMenuEnterKey()
    {
        if (!_selectionKey.WasPressedThisFrame()) return false;
        float selectButton = _selectionKey.ReadValue<float>();

        if (selectButton > 0.0f) return true;
        else return false;
        //_menu.SelectMenuOption();
    }

    public bool IsPressedMenuKey()
    {
        if (!_enterMenuKey.WasPressedThisFrame()) return false;
        float menuOpenButton = _enterMenuKey.ReadValue<float>();
        
        if (menuOpenButton > 0.0f) return true;
        else return false;
    }
}
