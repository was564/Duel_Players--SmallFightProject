using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private float _time;

    private TextMeshProUGUI _text;
    
    // Start is called before the first frame update
    void Start()
    {
        _text = this.GetComponentInChildren<TextMeshProUGUI>();
        _time = 99.0f;
        _text.text = _time.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;
        _text.text = _time.ToString("F0");
    }
}
