using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MenuInputManager : MonoBehaviour
{
    
    private MenuInGame _menu;
    
    [SerializeField] private InputActionAsset _menuInputPackage;
    [SerializeField] private InputActionAsset _playerInputPackage;
    [SerializeField] private InputActionAsset _enemyInputPackage;
    
    private InputActionMap _menuInputInMenu;
    private InputActionMap _playerInputInMenu;
    private InputActionMap _enemyInputInMenu;
    
    private InputAction _verticalScrollKey; // up, down
    private InputAction _optionChangeKey; // left, right

    private InputAction _selectionKey;

    private InputAction _enterMenuKey;
    
    private InputAction _playerSelectionKey;
    private InputAction _enemySelectionKey;
    
    // Start is called before the first frame update
    void Start()
    {
        _menu = GameObject.FindObjectOfType<MenuInGame>();
        
        _menuInputInMenu = _menuInputPackage.FindActionMap("Menu");
        _menuInputInMenu.Enable();
        
        _playerInputInMenu = _playerInputPackage.FindActionMap("Battle");
        _playerInputPackage.Enable();
        
        _enemyInputInMenu = _enemyInputPackage.FindActionMap("Battle");
        _enemyInputPackage.Enable();
        
        _verticalScrollKey = _menuInputInMenu.FindAction("VerticalMove");
        _optionChangeKey = _menuInputInMenu.FindAction("HorizontalMove");
        
        _selectionKey = _menuInputInMenu.FindAction("Select");
        _enemySelectionKey = _enemyInputInMenu.FindAction("Punch");
        _playerSelectionKey = _playerInputInMenu.FindAction("Punch");
        
        _enterMenuKey = _menuInputInMenu.FindAction("EnterMenu");
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
        short enterIndex = -1;
        if (_enemySelectionKey.WasPressedThisFrame()) enterIndex = 0;
        else if(_playerSelectionKey.WasPressedThisFrame()) enterIndex = 1;
        else if (_selectionKey.WasPressedThisFrame()) enterIndex = 2;


        switch (enterIndex)
        {
            case 0:
                if(_enemySelectionKey.ReadValue<float>() > 0.0f);
                return true;
            case 1:
                if(_playerSelectionKey.ReadValue<float>() > 0.0f);
                return true;
            case 2:
                if(_selectionKey.ReadValue<float>() > 0.0f);
                return true;
            default:
                return false;
        }
        
        /*
        float selectButton = _selectionKey.ReadValue<float>();

        if (selectButton > 0.0f) return true;
        else return false;
        //_menu.SelectMenuOption();
        */
        return false;
    }

    public bool IsPressedMenuKey()
    {
        if (!_enterMenuKey.WasPressedThisFrame()) return false;
        float menuOpenButton = _enterMenuKey.ReadValue<float>();
        
        if (menuOpenButton > 0.0f) return true;
        else return false;
    }
}
