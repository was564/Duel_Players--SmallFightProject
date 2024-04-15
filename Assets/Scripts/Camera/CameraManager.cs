using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CameraMode
    {
        Default = 0,
        SkillMoving,
        Size
    }
    
    private List<Transform> _playerTransforms = new List<Transform>();

    [SerializeField] private float _depthZOffset = -4.0f;
    [SerializeField] private float _borderBottomOffset = 1.5f;
    
    private Vector2 _leftBottomBorderPosition;
    private Vector2 _rightTopBorderPosition;

    private CameraMode _cameraMode;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter[] players = GameObject.FindObjectsByType<PlayerCharacter>(FindObjectsSortMode.None);
        foreach (var player in players)
            _playerTransforms.Add(player.transform);

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        float wallsCenterPosition = walls.Average(wall => wall.transform.position.x);
        float wallsDistance = walls.Max(wall => wall.transform.position.x) 
                         - walls.Min(wall => wall.transform.position.x);

        _leftBottomBorderPosition.x = wallsCenterPosition - ((wallsDistance * 0.5f) + _depthZOffset);
        _leftBottomBorderPosition.y = _borderBottomOffset;
        _rightTopBorderPosition.x = wallsCenterPosition + ((wallsDistance * 0.5f) + _depthZOffset);
        _rightTopBorderPosition.y = 100.0f;
        
        this.transform.position = Vector3.forward * _depthZOffset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nextPosition = this.transform.position;
        
        float positionY = _playerTransforms.Max(player => player.position.y);
        float positionX = _playerTransforms.Average(player => player.position.x);

        if (positionX < _leftBottomBorderPosition.x) positionX = _leftBottomBorderPosition.x;
        else if (positionX > _rightTopBorderPosition.x) positionX = _rightTopBorderPosition.x;

        if (positionY < _leftBottomBorderPosition.y) positionY = _leftBottomBorderPosition.y;
        else if (positionY > _rightTopBorderPosition.y) positionY = _rightTopBorderPosition.y;
        
        nextPosition.x = positionX;
        nextPosition.y = positionY;
        this.transform.position = nextPosition;
    }

    private void updateGoalPosition()
    {
        switch (_cameraMode)
        {
            case CameraMode.Default:
                
                break;
            case CameraMode.SkillMoving:
                break;
            
            default:
                break;
        }
    }
}
