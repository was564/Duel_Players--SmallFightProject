using System.Collections;
using System.Collections.Generic;
using Character.CharacterFSM;
using UnityEngine;

public class BehaviorStateManager : MonoBehaviour
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
    private Dictionary<BehaviorEnumSet.State, BehaviorStateInterface> _behaviorStateSet 
        = new Dictionary<BehaviorEnumSet.State, BehaviorStateInterface>();

    private BehaviorStateInterface _currentState;
     
    // Start is called before the first frame update
    void Start()
    {
        GameObject rootCharacter = this.transform.root.gameObject;
        _behaviorStateSet.Add(BehaviorEnumSet.State.StandingHit, new StandingHitState(rootCharacter));
        _behaviorStateSet.Add(BehaviorEnumSet.State.StandingIdle, new StandingIdleState(rootCharacter));
        _behaviorStateSet.Add(BehaviorEnumSet.State.StandingPunch, new StandingPunchState(rootCharacter));
        _behaviorStateSet.Add(BehaviorEnumSet.State.Forward, new WalkForwardState(rootCharacter));
        _behaviorStateSet.Add(BehaviorEnumSet.State.Backward, new WalkBackwardState(rootCharacter));
        _behaviorStateSet.Add(BehaviorEnumSet.State.Jump, new JumpState(rootCharacter));
        _behaviorStateSet.Add(BehaviorEnumSet.State.InAirIdle, new InAirIdleState(rootCharacter));
        _behaviorStateSet.Add(BehaviorEnumSet.State.Land, new LandState(rootCharacter));
        
        _currentState = _behaviorStateSet[BehaviorEnumSet.State.StandingIdle];
        _currentState.Enter();
    }

    public void HandleInput(BehaviorEnumSet.Behavior behavior)
    {
        _currentState.HandleInput(behavior);
    }
    
    public void UpdateState()
    {
        _currentState.UpdateState();
    }

    public void ChangeState(BehaviorEnumSet.State nextState)
    {
        _currentState.Quit();
        _currentState = _behaviorStateSet[nextState];
        _currentState.Enter();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
