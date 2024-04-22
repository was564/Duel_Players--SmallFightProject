using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    private RectTransform _hpGuage;
    private RectTransform _hpBackBoard;

    public PlayerCharacter playerCharacterParameter { get; set; }
    public bool IsRightSideBar { get; set; }
    private RawImage _hpRenderer;

    private Color _hpColor;
    
    private float _barWidth;

    private int _characterHp;
    private int _characterMaxHp;
    private float _characterHpRate; // 1 / _characterMaxHp

    private Vector2 _initHpGuagePosition;

    private void Start()
    {
        _hpGuage = this.gameObject.GetComponentInChildren<RawImage>().GetComponent<RectTransform>();
        _hpBackBoard = this.gameObject.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        _hpRenderer = _hpGuage.GetComponent<RawImage>();
        _hpColor = new Color(200, 200, 100, 255);
        _hpRenderer.color = _hpColor;
        
        _characterHp = playerCharacterParameter.Hp;
        _characterMaxHp = playerCharacterParameter.Hp;
        _characterHpRate = 1f / (float)_characterMaxHp;
        _barWidth = this.GetComponent<RectTransform>().rect.width;
        
        _hpGuage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _barWidth - 5.0f);
        _hpBackBoard.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _barWidth);
        _initHpGuagePosition = _hpGuage.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_characterHp <= 0) SetVisibleHp(false);
        else SetVisibleHp(true); 
        
        _characterHp = playerCharacterParameter.Hp;
        _hpGuage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)_characterHp * _characterHpRate * (_barWidth - 5.0f));
        
        if(IsRightSideBar)
            _hpGuage.anchoredPosition = 
                _initHpGuagePosition - Vector2.left * ((float)(_characterMaxHp - _characterHp) * _characterHpRate * (_barWidth - 5.0f) * 0.5f);
        else
            _hpGuage.anchoredPosition = 
                _initHpGuagePosition + Vector2.left * ((float)(_characterMaxHp - _characterHp) * _characterHpRate * (_barWidth - 5.0f) * 0.5f);
    }

    private void SetVisibleHp(bool isVisible)
    {
        if (!isVisible) _hpColor.a = 0;
        else _hpColor.a = 255;
        _hpRenderer.color = _hpColor;
    }
}
