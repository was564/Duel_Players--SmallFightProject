using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private int _previousResolutionIndex = 0;
    private bool _previousFullScreenIndex = false;
    
    private int _selectedResolutionIndex = 0;
    private bool _isFullScreen = false;
    
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Slider _VolumeSlider;

    private LeaveDataForGame _data;

    private RebindSaveLoad[] _keyMappingObjects;
    
    private GameObject _settingPanel;
    private GameObject _keyMappingPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.FindObjectOfType<LeaveDataForGame>();
        _settingPanel = GameObject.Find("SettingPanel");
        _keyMappingPanel = GameObject.Find("KeyMappingPanel");
        
        _keyMappingObjects = _keyMappingPanel.GetComponentsInChildren<RebindSaveLoad>();
        
        _fullScreenToggle.isOn = Screen.fullScreen;
        _isFullScreen = _fullScreenToggle.isOn;
        _fullScreenToggle.onValueChanged.AddListener(delegate { OnChangedFullScreen(); });
        
        _selectedResolutionIndex = _resolutionDropdown.value;
        _resolutionDropdown.onValueChanged.AddListener(delegate { OnChangedResolution(); });
        
        _previousResolutionIndex = _selectedResolutionIndex;
        _previousFullScreenIndex = _isFullScreen;
        
        _settingPanel.SetActive(false);
        _keyMappingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSlideVolume()
    {
        _data.Volume = _VolumeSlider.value;
    }
    
    public void OnClickSetting()
    {
        _settingPanel.SetActive(true);
    }
    
    public void OnClickKeyMapping()
    {
        foreach (var keyMappingObject in _keyMappingObjects)
        {
            keyMappingObject.gameObject.SetActive(true);
        }
        
        _keyMappingPanel.SetActive(true);
    }
    
    public void OnClickCloseKeyMappingButton()
    {
        foreach (var keyMappingObject in _keyMappingObjects)
        {
            keyMappingObject.gameObject.SetActive(false);
        }
        
        _keyMappingPanel.SetActive(false);
    }
    
    public void OnClickSingleStartGame()
    {
        _data.SelectedGameState = GameRoundManager.GameState.SingleNormalPlay;
        SceneManager.LoadScene("GameScene");
    }
    
    public void OnClickMultiStartGame()
    {
        _data.SelectedGameState = GameRoundManager.GameState.MultiNormalPlay;
        SceneManager.LoadScene("GameScene");
    }
    
    public void OnClickReplayGame()
    {
        _data.SelectedGameState = GameRoundManager.GameState.Replay;
        SceneManager.LoadScene("GameScene");
    }
    
    public void OnChangedResolution()
    {
        _previousResolutionIndex = _selectedResolutionIndex;
        _selectedResolutionIndex = _resolutionDropdown.value;
    }
    
    public void OnChangedFullScreen()
    {
        _previousFullScreenIndex = _isFullScreen;
        _isFullScreen = _fullScreenToggle.isOn;
    }

    public void OnClickCloseSettingButton()
    {
        _settingPanel.SetActive(false);
    }
    
    public void OnClickCancelButton()
    {
        _fullScreenToggle.isOn = _previousFullScreenIndex;
        _resolutionDropdown.value = _previousResolutionIndex;
        _settingPanel.SetActive(false);
    }
    
    public void OnClickApplyButton()
    {
        switch (_selectedResolutionIndex)
        {
            case 0:
                Screen.SetResolution(1920, 1080, _isFullScreen);
                break;
            
            case 1:
                Screen.SetResolution(1600, 900, _isFullScreen);
                break;
            
            case 2:
                Screen.SetResolution(1280, 720, _isFullScreen);
                break;
        }
        
        _previousFullScreenIndex = _isFullScreen;
        _previousResolutionIndex = _selectedResolutionIndex;
    }
}
