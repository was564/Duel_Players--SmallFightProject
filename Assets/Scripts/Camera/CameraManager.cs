using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private List<Transform> _playerTransforms = new List<Transform>();
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter[] players = GameObject.FindObjectsByType<PlayerCharacter>(FindObjectsSortMode.None);

        foreach (var player in players)
            _playerTransforms.Add(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
