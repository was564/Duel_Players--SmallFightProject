using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CameraState
    {
        Normal = 0,
        FreeMoving,
        SkillMoving,
        Size
    }
    
    private List<Transform> _playerTransforms = new List<Transform>();

    private Camera _camera;
    
    [SerializeField] private float _depthZOffset = -4.0f;
    [SerializeField] private float _borderBottomOffset = 1.5f;
    
    private Vector2 _leftBottomBorderPosition;
    private Vector2 _rightTopBorderPosition;

    private CameraState _cameraState;
    private float _cameraSpeed = 10.0f;
    
    private Vector3 _cameraGoalPosition;
    private Vector3 _cameraTargetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        
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
        _cameraState = CameraState.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraTransform();
        
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        float distance = Vector3.Distance(_cameraGoalPosition, this.transform.position);
        if (distance < _cameraSpeed * Time.deltaTime)
            this.transform.position = _cameraGoalPosition;
        else
        {
            Vector3 direction = (_cameraGoalPosition - this.transform.position).normalized;
            this.transform.position += direction * (_cameraSpeed * Time.deltaTime);
        }
        
        this.transform.LookAt(_cameraTargetPosition);
    }
    

    
    private void UpdateCameraTransform()
    {
        switch (_cameraState)
        {
            case CameraState.Normal:
                _cameraGoalPosition = GetNormalCameraPoint();
                _cameraTargetPosition = _cameraGoalPosition + Vector3.forward;
                break;
            case CameraState.FreeMoving:
                
                break;
            default:
                break;
        }
    }

    public Vector3 GetNormalCameraPoint()
    {
        Vector3 targetPosition = this.transform.position;
        
        float positionY = _playerTransforms.Max(player => player.position.y);
        float positionX = _playerTransforms.Average(player => player.position.x);

        if (positionX < _leftBottomBorderPosition.x) positionX = _leftBottomBorderPosition.x;
        else if (positionX > _rightTopBorderPosition.x) positionX = _rightTopBorderPosition.x;

        if (positionY < _leftBottomBorderPosition.y) positionY = _leftBottomBorderPosition.y;
        else if (positionY > _rightTopBorderPosition.y) positionY = _rightTopBorderPosition.y;
        
        targetPosition.x = positionX;
        targetPosition.y = positionY;
        targetPosition.z = _depthZOffset;
                
        return targetPosition;
    }
    
    public void SetGoalPoint(Vector3 goalPosition)
    {
        _cameraGoalPosition = goalPosition;
    }
    
    public void SetTargetPoint(Vector3 targetPosition)
    {
        _cameraTargetPosition = targetPosition;
    }
    
    public void SetCameraState(CameraState state)
    {
        _cameraState = state;
    }
}
