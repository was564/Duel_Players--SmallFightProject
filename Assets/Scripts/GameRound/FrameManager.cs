using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 문제 : 컴퓨터 사양에 따라 프레임 속도가 다를 수 있는데 이를 동기화할 방법 (서버 멀티 관련 문제)
// 1프레임이 늦으면 배로 계산하게 하여 2프레임 이후를 계산하여 적당한 타이밍에 넣기
// 가능한가..?
// 60프레임으로 할 것임

// 접근 할 수 있는 경우
// 공격 성공 및 방어 성공 (HitBox)
// 캐릭터 고유 스킬 발동 (Character State)
public class FrameManager : MonoBehaviour
{
    private Dictionary<int, PassiveStateManager> _charactersList = new Dictionary<int, PassiveStateManager>();

    public static int CurrentFrame { get; private set; } = 0;

    public bool IsFramePaused { get; set; } = false;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    
    private void Start()
    {
        CurrentFrame = 0;
        IsFramePaused = true;
        PlayerCharacter[] characters = GameObject.FindObjectsOfType<PlayerCharacter>();
        foreach (var character in characters)
        {
            _charactersList.Add(character.gameObject.GetInstanceID(), character.gameObject.GetComponent<PassiveStateManager>());
        }
    }

    private void Update()
    {
        if (IsFramePaused) return;
        CurrentFrame += 1;
    }

    public void PauseAllCharactersInFrame(int frame)
    {
        foreach (var key in _charactersList.Keys)
        {
            _charactersList[key].ActivatePassiveState(PassiveStateEnumSet.PassiveState.FrameStopping, frame);
        }
    }

    public void PauseOtherCharactersInFrame(int frame, int characterId)
    {
        foreach (var key in _charactersList.Keys)
        {
            if (key == characterId) continue;
            _charactersList[key].ActivatePassiveState(PassiveStateEnumSet.PassiveState.FrameStopping, frame);
        }
    }
    
    public void ResumeAllCharacters()
    {
        foreach (var key in _charactersList.Keys)
        {
            _charactersList[key].DeActivatePassiveState(PassiveStateEnumSet.PassiveState.FrameStopping);
        }
    }

    public void ResetFrame()
    {
        CurrentFrame = 0;
    }
}
