using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTransform : MonoBehaviour
{
    private GameObject _characterRoot;
    private GameObject _camera;

    private float _distance = 1.0f;
    
    private void Start()
    {
        _characterRoot = this.transform.root.gameObject;
        _camera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 characterPosition = _characterRoot.transform.position;
        characterPosition.y = 0.0f;
        
        Vector3 shadowDepthDirection = (characterPosition - _camera.transform.position).normalized;
        this.transform.position = characterPosition + shadowDepthDirection * _distance;
        
        float distanceBetweenShadowAndCamera = Vector3.Distance(this.transform.position, _camera.transform.position);
        float scale = 1.0f + (_distance / distanceBetweenShadowAndCamera);
        this.transform.localScale = new Vector3(scale, scale, scale);
    }
}
