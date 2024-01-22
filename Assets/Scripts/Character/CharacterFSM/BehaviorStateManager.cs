using System.Collections.Generic;
using Character;
using Character.CharacterFSM;
using Character.CharacterFSM.SkillState;
using UnityEngine;

public class BehaviorStateManager : BehaviorStateSimulator
{
    // update를 안쓰는데 Start는 쓴다
    // Start는 다른 객체에서 Init을 시키면 가능하다
    // MonoBehavior를 하는 이유가 있을까
    // 상담하기

    // 모든 behaviorState를 set으로 관리 enum과 연결
    // 다른 State로 바꿀 때 O(1)로 찾기 위함
    // 답안 : State에 고유 코드(enum) 가지고 있기
    // 이의 : State의 추가 마다 enum 추가해야함 (문제 없음)
    // 나중에 나온 답 : Dictionary 사용 (시간은 O(logN)임)
    // 추가 : Dictionary의 시간은 O(1)이다.
    // 물론 index로 접근하면 List가 더 빠르겠지만 그렇게 차이 안난다
    // Reference: https://prographers.com/blog/list-vs-dictionary-performance
    /*
    protected Dictionary<BehaviorEnumSet.State, BehaviorStateInterface> BehaviorStateSet
        = new Dictionary<BehaviorEnumSet.State, BehaviorStateInterface>();

    private GameObject _rootCharacterObject;

    public BehaviorStateInterface CurrentState { get; protected set; }

    protected ComboManager ComboManagerInstance;
    */

    public BehaviorStateManager(GameObject characterObject, GameObject wall, BehaviorStateSetInterface stateSet, ComboManager comboManager)
        : base(characterObject, wall, stateSet, comboManager)
    {
    }

    public override void UpdateState()
    {
        BehaviorEnumSet.State resultState = this.CurrentState.UpdateState();
        this.ChangeState(resultState);
        // Debug.Log(CurrentState);
    }

    public override void ChangeState(BehaviorEnumSet.State nextState)
    {
        if (nextState == BehaviorEnumSet.State.Null) return;
        CurrentState.Quit();
        CurrentState = StateSet.GetStateInfo(nextState);
        CurrentState.Enter();
    }
}
