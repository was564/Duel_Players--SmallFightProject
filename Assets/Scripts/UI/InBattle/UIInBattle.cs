using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInBattle : MonoBehaviour
{
    public GameObject HPBarPrefab;
    public GameObject TimerPrefab;
    public GameObject ComboTextPrefab;

    private RectTransform _canvas;

    private float _canvasWidth;
    private float _canvasHeight;
    
    private GameObject _leftPlayerHpBar;
    private GameObject _rightPlayerHpBar;

    private GameObject _timer;

    private GameObject _leftPlayerComboText;
    private GameObject _rightPlayerComboText;
    
    private Camera _camera;

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _enemy;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var canvas in GameObject.FindObjectsOfType<Canvas>())
        {
            if(canvas.gameObject.layer == LayerMask.NameToLayer("InGameUI")) _canvas = canvas.GetComponent<RectTransform>();
        }
        
        _canvasWidth = _canvas.rect.width;
        _canvasHeight = _canvas.rect.height;

        _leftPlayerComboText = Instantiate(ComboTextPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject;
        _rightPlayerComboText = Instantiate(ComboTextPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject;
        
        _leftPlayerHpBar = Instantiate(HPBarPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject;
        _rightPlayerHpBar = Instantiate(HPBarPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject;
        HpBarUI leftHpBar, rightHpBar;
        leftHpBar = _leftPlayerHpBar.GetComponent<HpBarUI>();
        rightHpBar = _rightPlayerHpBar.GetComponent<HpBarUI>();
        leftHpBar.IsRightSideBar = false;
        rightHpBar.IsRightSideBar = true;
        
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        _timer = Instantiate(TimerPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject;
        _player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;
        _enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.gameObject;
        
        Vector3 playerDirection = (_player.transform.position - _camera.transform.position);
        Vector3 enemyDirection = (_enemy.transform.position - _camera.transform.position);
        
        if (Vector3.Cross(playerDirection, enemyDirection).y > 0.0f)
        {
            leftHpBar.playerCharacterParameter = _player.GetComponent<PlayerCharacter>();
            rightHpBar.playerCharacterParameter = _enemy.GetComponent<PlayerCharacter>();
            _leftPlayerComboText.tag = _player.tag;
            _rightPlayerComboText.tag = _enemy.tag;
        }
        else
        {
            leftHpBar.playerCharacterParameter = _enemy.GetComponent<PlayerCharacter>();
            rightHpBar.playerCharacterParameter = _player.GetComponent<PlayerCharacter>();
            _leftPlayerComboText.tag = _enemy.tag;
            _rightPlayerComboText.tag = _player.tag;
        }
        
        RectTransform leftComboTextOffset = _leftPlayerComboText.GetComponent<RectTransform>();
        RectTransform rightComboTextOffset = _rightPlayerComboText.GetComponent<RectTransform>();
        
        leftComboTextOffset.anchoredPosition3D = Vector3.zero;
        rightComboTextOffset.anchoredPosition3D = Vector3.zero;
        
        leftComboTextOffset.anchoredPosition3D += -Vector3.right * (_canvasWidth * 0.5f - leftComboTextOffset.rect.width * 0.5f);
        rightComboTextOffset.anchoredPosition3D += Vector3.right * (_canvasWidth * 0.5f - rightComboTextOffset.rect.width * 0.5f);
        
        // 화면 Width 를 20으로 가정
        // 공백 : 체력바 : 공백 : 타이머 : 공백 : 체력바 : 공백
        // 공백 1, 체력바 7, 타이머 2 
        // 4 * 공백 + 2 * 체력바 + 타이머 = 4 + 14 + 2 = 20
        RectTransform leftHpBarOffset = _leftPlayerHpBar.GetComponent<RectTransform>();
        RectTransform rightHpBarOffset = _rightPlayerHpBar.GetComponent<RectTransform>();
        RectTransform timerOffset = _timer.GetComponent<RectTransform>();

        float padderHeight = (_canvasHeight * 0.01f) + (leftHpBarOffset.rect.height * 0.5f);
        float padderWidth = _canvasWidth * 0.05f;
        
        float barWidth = _canvasWidth * 0.05f * 7.0f;

        float timerDiameter = _canvasWidth * 0.05f * 2.0f;
        
        
        leftHpBarOffset.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
        timerOffset.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, timerDiameter);
        timerOffset.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, timerDiameter);
        rightHpBarOffset.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);

        leftHpBarOffset.anchoredPosition3D = Vector3.zero;
        timerOffset.anchoredPosition3D = Vector3.zero;
        rightHpBarOffset.anchoredPosition3D = Vector3.zero;

        
        leftHpBarOffset.anchoredPosition3D += Vector3.up * (_canvasHeight - padderHeight);
        leftHpBarOffset.anchoredPosition3D += Vector3.right * ((leftHpBarOffset.rect.width * 0.5f) + padderWidth);

        timerOffset.anchoredPosition3D += Vector3.up * (_canvasHeight - padderHeight);
        timerOffset.anchoredPosition3D += Vector3.right * (leftHpBarOffset.rect.width + (padderWidth * 2.0f) + (timerDiameter * 0.5f));

        rightHpBarOffset.anchoredPosition3D += Vector3.up * (_canvasHeight - padderHeight);
        rightHpBarOffset.anchoredPosition3D += Vector3.right * (leftHpBarOffset.rect.width + (padderWidth * 3.0f) + timerDiameter + rightHpBarOffset.rect.width * 0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
