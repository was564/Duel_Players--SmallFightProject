using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.InMenu.MenuOptions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInGame : MonoBehaviour
{
    private RectTransform _canvas;
    
    // width 700, height 60
    public GameObject MenuOptionPrefab;
    public GameObject MenuSelectingPrefab;
    public GameObject MenuPanelPrefab;
    
    private List<MenuOptionInterface> _menuList = new List<MenuOptionInterface>();

    private int _selectingIndexInMenu = 0;
    private RectTransform _menuSelectingUI;
    private RectTransform _menuPanel;
    
    //private bool _isOpenedMenu = false;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var canvas in GameObject.FindObjectsOfType<Canvas>())
        {
            if(canvas.gameObject.layer == LayerMask.NameToLayer("UI")) _canvas = canvas.GetComponent<RectTransform>();
        }
        
        _menuPanel = Instantiate(MenuPanelPrefab, _canvas).GetComponent<RectTransform>();
        
        _menuSelectingUI = Instantiate(MenuSelectingPrefab, _canvas).GetComponent<RectTransform>();
        
        _menuList.Add(new GameEndOption(Instantiate(MenuOptionPrefab, _canvas) as GameObject));
        _menuList.Add(new ResumeOption(Instantiate(MenuOptionPrefab, _canvas) as GameObject));
        _menuList.Add(new RestartOption(Instantiate(MenuOptionPrefab, _canvas) as GameObject));
        _menuList.Add(new ReplayOption(Instantiate(MenuOptionPrefab, _canvas) as GameObject));
        _menuList.Add(new SaveRoundOption(Instantiate(MenuOptionPrefab, _canvas) as GameObject));

        _menuPanel.anchoredPosition = Vector2.zero;
        _menuPanel.Rotate(Vector3.forward * 20.0f);
        for (int index = 0; index < _menuList.Count; index++)
        {
            RectTransform optionPosition = _menuList[index].Transform;
            optionPosition.anchoredPosition =
                (Vector2.up * optionPosition.rect.height * 1.5f * (index - (_menuList.Count * 0.5f))) +
                ((Vector2.right * 30.0f) * ((_menuList.Count * 0.5f) - index));
        }

        SyncPositionPanelAndMenuOption();
        OnDisableMenu();
    }

    public void MenuScrollUp()
    {
        if(_selectingIndexInMenu == _menuList.Count - 1) return;
        _selectingIndexInMenu += 1;
        SyncPositionPanelAndMenuOption();
    }
    
    public void MenuScrollDown()
    {
        if (_selectingIndexInMenu == 0) return;
        _selectingIndexInMenu -= 1;
        SyncPositionPanelAndMenuOption();
    }

    public void SelectMenuOption()
    {
        _menuList[_selectingIndexInMenu].OnSelect();
    }

    public void OnEnableMenu()
    {
        _menuPanel.gameObject.SetActive(true);
        foreach (var menuOption in _menuList)
        {
            menuOption.Transform.gameObject.SetActive(true);
        }
        _menuSelectingUI.gameObject.SetActive(true);
    }
    
    public void OnDisableMenu()
    {
        _menuPanel.gameObject.SetActive(false);
        foreach (var menuOption in _menuList)
        {
            menuOption.Transform.gameObject.SetActive(false);
        }
        _menuSelectingUI.gameObject.SetActive(false);
    }

    public void SyncPositionPanelAndMenuOption()
    {
        _menuSelectingUI.anchoredPosition = _menuList[_selectingIndexInMenu].Transform.anchoredPosition;
    }
}