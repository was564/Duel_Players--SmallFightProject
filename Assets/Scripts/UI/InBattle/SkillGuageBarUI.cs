using UnityEngine;
using UnityEngine.UI;

public class SkillGuageBarUI : MonoBehaviour
{
    private RectTransform _gauge;
    private RectTransform _gaugeBackBoard;

    public PlayerCharacter PlayerCharacterParameter { get; set; }
    public bool IsRightSideBar { get; set; }
    private RawImage _gaugeRenderer;
    
    private float _barWidth;

    private int _characterGauge;
    private int _characterMaxGauge;
    private float _characterGaugeRate; // 1 / _characterMaxHp

    private Vector2 _initGaugePosition;

    private void Start()
    {
        _gauge = this.gameObject.GetComponentInChildren<RawImage>().GetComponent<RectTransform>();
        _gaugeBackBoard = this.gameObject.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        
        _gaugeRenderer = _gauge.GetComponent<RawImage>();
        
        _characterGauge = PlayerCharacterParameter.SkillGauge;
        _characterMaxGauge = 100;
        _characterGaugeRate = 1f / (float)_characterMaxGauge;
        _barWidth = this.GetComponent<RectTransform>().rect.width;
        
        _gauge.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _barWidth - 5.0f);
        _gaugeBackBoard.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _barWidth);
        _initGaugePosition = _gauge.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_characterGauge <= 0) SetVisibleSkillGauge(false);
        else SetVisibleSkillGauge(true); 
        
        _characterGauge = PlayerCharacterParameter.SkillGauge;
        _gauge.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)_characterGauge * _characterGaugeRate * (_barWidth - 5.0f));
        
        if(IsRightSideBar)
            _gauge.anchoredPosition = 
                _initGaugePosition - Vector2.left * ((float)(_characterMaxGauge - _characterGauge) * _characterGaugeRate * (_barWidth - 5.0f) * 0.5f);
        else
            _gauge.anchoredPosition = 
                _initGaugePosition + Vector2.left * ((float)(_characterMaxGauge - _characterGauge) * _characterGaugeRate * (_barWidth - 5.0f) * 0.5f);
    }

    private void SetVisibleSkillGauge(bool isVisible)
    {
        Color gaugeColor = _gaugeRenderer.color;
        if (!isVisible) gaugeColor.a = 0;
        else gaugeColor.a = 255;
        _gaugeRenderer.color = gaugeColor;
    }
}
