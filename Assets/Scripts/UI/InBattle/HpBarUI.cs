using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    private RectTransform _hpGauge;
    private RectTransform _hpBackBoard;

    public PlayerCharacter PlayerCharacterParameter { get; set; }
    public bool IsRightSideBar { get; set; }
    private RawImage _hpRenderer;

    //private Color _hpColor;
    
    private float _barWidth;

    private int _characterHp;
    private int _characterMaxHp;
    private float _characterHpRate; // 1 / _characterMaxHp

    private Vector2 _initHpGaugePosition;

    private void Start()
    {
        _hpGauge = this.gameObject.GetComponentInChildren<RawImage>().GetComponent<RectTransform>();
        _hpBackBoard = this.gameObject.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        
        _hpRenderer = _hpGauge.GetComponent<RawImage>();
        /*
        _hpColor = new Color(0, 255, 70, 255);
        _hpRenderer.color = _hpColor;
        */
        
        _characterHp = PlayerCharacterParameter.Hp;
        _characterMaxHp = PlayerCharacterParameter.Hp;
        _characterHpRate = 1f / (float)_characterMaxHp;
        _barWidth = this.GetComponent<RectTransform>().rect.width;
        
        _hpGauge.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _barWidth - 5.0f);
        _hpBackBoard.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _barWidth);
        _initHpGaugePosition = _hpGauge.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_characterHp <= 0) SetVisibleHp(false);
        else SetVisibleHp(true); 
        
        _characterHp = PlayerCharacterParameter.Hp;
        _hpGauge.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)_characterHp * _characterHpRate * (_barWidth - 5.0f));
        
        if(IsRightSideBar)
            _hpGauge.anchoredPosition = 
                _initHpGaugePosition - Vector2.left * ((float)(_characterMaxHp - _characterHp) * _characterHpRate * (_barWidth - 5.0f) * 0.5f);
        else
            _hpGauge.anchoredPosition = 
                _initHpGaugePosition + Vector2.left * ((float)(_characterMaxHp - _characterHp) * _characterHpRate * (_barWidth - 5.0f) * 0.5f);
    }

    private void SetVisibleHp(bool isVisible)
    {
        Color hpColor = _hpRenderer.color;
        if (!isVisible) hpColor.a = 0;
        else hpColor.a = 255;
        _hpRenderer.color = hpColor;
    }
}
