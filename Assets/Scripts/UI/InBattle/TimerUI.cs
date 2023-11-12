using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private GameRoundManager _gameManager;
    
    private TextMeshProUGUI _text;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameRoundManager>();
        _text = this.GetComponentInChildren<TextMeshProUGUI>();
        _text.text = _gameManager.RoundRemainTime.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _gameManager.RoundRemainTime.ToString("F0");
    }
}
