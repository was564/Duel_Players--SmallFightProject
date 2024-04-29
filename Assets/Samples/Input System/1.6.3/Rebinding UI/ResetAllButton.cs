using System;
using UnityEngine;
using UnityEngine.InputSystem.RebindUI;

public class ResetAllButton : MonoBehaviour
{
    private RebindActionUI[] _rebindActionUIList;

    private void Start()
    {
        _rebindActionUIList = transform.GetComponentsInChildren<RebindActionUI>();
    }
    
    public void ResetAll()
    {
        foreach (var rebindActionUI in _rebindActionUIList)
        {
            rebindActionUI.ResetToDefault();
        }
    }
}