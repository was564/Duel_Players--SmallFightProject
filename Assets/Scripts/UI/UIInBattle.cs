using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInBattle : MonoBehaviour
{
    public GameObject HPBarPrefab;
    public GameObject TimerPrefab;

    private RectTransform _canvas;

    private float _canvasWidth;
    private float _canvasHeight;
    
    private GameObject _leftPlayerHpBar;
    private GameObject _rightPlayerHpBar;

    private GameObject _timer;

    private GameObject _camera;

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _canvas = this.gameObject.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
        _canvasWidth = _canvas.rect.width;
        _canvasHeight = _canvas.rect.height;

        _leftPlayerHpBar = Instantiate(HPBarPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject;
        _rightPlayerHpBar = Instantiate(HPBarPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject;
        
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        
        _timer = Instantiate(TimerPrefab, Vector3.zero, Quaternion.identity, _canvas) as GameObject;
        _player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;
        _enemy = GameObject.FindGameObjectWithTag("Enemy").transform.root.gameObject;
        
        Vector3 playerDirection = (_player.transform.position - _camera.transform.position);
        Vector3 enemyDirection = (_enemy.transform.position - _camera.transform.position);
        
        if (Vector3.Cross(playerDirection, enemyDirection).y > 0.0f)
        {
            _leftPlayerHpBar.GetComponent<HpBarUI>().CharacterParameter = _player.GetComponent<CharacterStructure>();
            _rightPlayerHpBar.GetComponent<HpBarUI>().CharacterParameter = _enemy.GetComponent<CharacterStructure>();
        }
        else
        {
            _leftPlayerHpBar.GetComponent<HpBarUI>().CharacterParameter = _enemy.GetComponent<CharacterStructure>();
            _rightPlayerHpBar.GetComponent<HpBarUI>().CharacterParameter = _player.GetComponent<CharacterStructure>();
        }
        
        
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
        
        leftHpBarOffset.anchoredPosition += Vector2.up * (_canvasHeight - padderHeight);
        leftHpBarOffset.anchoredPosition += Vector2.right * ((leftHpBarOffset.rect.width * 0.5f) + padderWidth);
        
        
        timerOffset.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, timerDiameter);
        timerOffset.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, timerDiameter);
        
        timerOffset.anchoredPosition += Vector2.up * (_canvasHeight - padderHeight);
        timerOffset.anchoredPosition += Vector2.right * (leftHpBarOffset.rect.width + (padderWidth * 2.0f) + (timerDiameter * 0.5f));

        
        rightHpBarOffset.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
        
        rightHpBarOffset.anchoredPosition += Vector2.up * (_canvasHeight - padderHeight);
        rightHpBarOffset.anchoredPosition += Vector2.right * (leftHpBarOffset.rect.width + (padderWidth * 3.0f) + timerDiameter + rightHpBarOffset.rect.width * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
