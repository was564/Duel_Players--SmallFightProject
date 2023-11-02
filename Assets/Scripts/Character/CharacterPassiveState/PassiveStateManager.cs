using System.Collections;
using System.Collections.Generic;
using Character.CharacterPassiveState;
using UnityEngine;

public class PassiveStateManager : MonoBehaviour
{
    // 버프 디버프같은 것을 가지게 하고 남는 시간에 따라 정렬할 수 있는 리스트
    // 남는 시간이 끝나면 Remove, 버프나 디버프가 추가되면 Add
    // 제안1 : PriorityQueue를 구현 (key-read-only이면 안됨)
    // 제안2 : 버프나 디버프 마다 Enum으로 요소 위치를 지정 후 HashTable 처럼 관리
    // 문제 : 새로운 효과가 추가되면 Enum을 추가하며 캐릭터는 시작할 때 모든 효과를 가지고 있는다. (Disable, Enable)
    // 제안3 : LinkedList를 이용해 항상 탐색하여 시간에 관계 없이 종료되면 삭제, 추가할 때 중간에 추가가 용이하다
    // 문제 : 이미 있던 상태가 다음 상태가 들어오면 삭제되어야 함, new는 쓰면 안된다.
    // 대안 : 공격을 전체적으로 아는 클래스가 자기가 필요한 모든 스테이트를 만들어 놓기
    // 모든 스테이트를 만들고 초기화(Start) 단계에서 넣어놓고 삭제하거나 추가하거나 하지말고 Dictionary로 쓰기
    // Dictionary 구분을 위해 enum 만들기
    // 대안 : 버프 디버프로 인해 바뀌는 값은 캐릭터의 값 바꾸는 모듈 만들기
    // 참고 : (Linked List vs Vector(List))
    // 메모리 지역성으로 서로 이웃한 메모리가 접근이 더 빠르다.
    // https://johnnysswlab.com/the-quest-for-the-fastest-linked-list/
    // https://scahp.tistory.com/37
    //private List<PassiveStateInterface> _activatedPassiveStateSet = new List<PassiveStateInterface>();
    
    private Dictionary<PassiveStateEnumSet.PassiveState, PassiveStateInterface> _passiveStateSet
        = new Dictionary<PassiveStateEnumSet.PassiveState, PassiveStateInterface>();
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject characterRoot = this.transform.root.gameObject;
        
        _passiveStateSet.Add(PassiveStateEnumSet.PassiveState.StoppingOnGround, new StoppingOnGroundState(characterRoot));
        ActivatePassiveState(PassiveStateEnumSet.PassiveState.StoppingOnGround, 200.0f);
    }

    // Update is called once per frame
    public void UpdatePassiveState()
    {
        foreach (var key in _passiveStateSet.Keys)
        {
            PassiveStateInterface currentPassiveState = _passiveStateSet[key];
            if(!currentPassiveState.IsActivate) continue;
            
            currentPassiveState.UpdatePassiveState();

            currentPassiveState.RemainTime -= Time.deltaTime;
            if (currentPassiveState.RemainTime <= 0.0f)
            {
                currentPassiveState.IsActivate = false;
                currentPassiveState.QuitPassiveState();
            }
        }
    }

    public void ActivatePassiveState(PassiveStateEnumSet.PassiveState passiveState, float maintainTime)
    {
        PassiveStateInterface stateInfo = _passiveStateSet[passiveState];
        stateInfo.IsActivate = true;
        stateInfo.RemainTime = maintainTime;
        stateInfo.EnterPassiveState();
    }
}
