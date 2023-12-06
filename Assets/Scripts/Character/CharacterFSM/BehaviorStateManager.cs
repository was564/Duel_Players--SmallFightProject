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
    
    public BehaviorStateManager(GameObject characterObject, GameObject wall, ComboManager comboManager)
        : base(characterObject, wall, comboManager) { }
    
    public override void HandleInput(BehaviorEnumSet.Behavior behavior)
    {
        if(!ComboManagerInstance.TryActivateSkillState(behavior, this))
            CurrentState.HandleInput(behavior);
    }
    
    public override void UpdateState()
    {
        CurrentState.UpdateState();
        // Debug.Log(CurrentState);
    }

    public override void ChangeState(BehaviorEnumSet.State nextState)
    {
        CurrentState.Quit();
        CurrentState = BehaviorStateSet[nextState];
        CurrentState.Enter();
    }
    
    protected virtual void InitStateSet()
    {
        BehaviorStateSet.Add(BehaviorEnumSet.State.StandingHit, new StandingHitState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.StandingIdle, new StandingIdleState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.StandingPunch, new StandingPunchState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.StandingKick, new StandingKickState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.Forward, new WalkingForwardState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.Backward, new WalkingBackwardState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.Jump, new JumpState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.InAirIdle, new AiringState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.Land, new LandState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchIdle, new CrouchIdleState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchPunch, new CrouchPunchState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchKick, new CrouchKickState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.AiringPunch, new AiringPunchState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.AiringKick, new AiringKickState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.StandingPunchSkill, new StandingPunch236SkillState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.DashOnGround, new DashOnGroundState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.StandingGuard, new StandingGuardState(RootCharacterObject, Wall, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchGuard, new CrouchGuardState(RootCharacterObject, Wall, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.CrouchHit, new CrouchHitState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.InAirHit, new InAirHitState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.FallDown, new FallDownState(RootCharacterObject, this));
        BehaviorStateSet.Add(BehaviorEnumSet.State.GetUp, new GetUpState(RootCharacterObject, this));
    }
}
