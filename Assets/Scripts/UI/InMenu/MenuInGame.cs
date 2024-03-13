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
    
    private List<MenuOptionInterface> _menuList = new List<MenuOptionInterface>();

    private int _selectingIndexInMenu = 0;
    private RectTransform _menuSelectingUI;
    
    //private bool _isOpenedMenu = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _canvas = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        
        _menuSelectingUI = Instantiate(MenuSelectingPrefab, Vector3.zero, Quaternion.identity, _canvas)
            .GetComponent<RectTransform>();
        
        _menuList.Add(new GameEndOption(Instantiate(MenuOptionPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject));
        _menuList.Add(new ResumeOption(Instantiate(MenuOptionPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject));
        _menuList.Add(new RestartOption(Instantiate(MenuOptionPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject));
        _menuList.Add(new ReplayOption(Instantiate(MenuOptionPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject));
        _menuList.Add(new SaveRoundOption(Instantiate(MenuOptionPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject));

        for (int index = 0; index < _menuList.Count; index++)
        {
            RectTransform optionPosition = _menuList[index].Transform;
            optionPosition.anchoredPosition =
                (Vector2.up * optionPosition.rect.height * 1.5f * (index - (_menuList.Count * 0.5f))) +
                ((Vector2.right * 30.0f) * (_menuList.Count - index));
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
        foreach (var menuOption in _menuList)
        {
            menuOption.Transform.gameObject.SetActive(true);
        }
        _menuSelectingUI.gameObject.SetActive(true);
    }
    
    public void OnDisableMenu()
    {
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