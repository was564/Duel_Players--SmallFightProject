using System.Collections;
using System.Collections.Generic;
using Character;
using Character.CharacterFSM;
using UnityEngine;

public abstract class BehaviorStateInterface
{
    protected BehaviorStateInterface(
        BehaviorEnumSet.State stateName,
        BehaviorStateSimulator stateManager,
        GameObject characterRoot, 
        BehaviorEnumSet.AttackLevel attackLevel,
        PassiveStateEnumSet.CharacterPositionState positionState)
    {
        this.AttackLevel = (int)attackLevel;
        this.StateName = stateName;
        this.CharacterTransform = characterRoot.transform;
        this.CharacterAnimator = characterRoot.GetComponent<CharacterAnimator>();
        this.CharacterRigidBody = characterRoot.GetComponent<Rigidbody>();
        this.StateManager = stateManager;
        this.PlayerCharacter = characterRoot.GetComponent<PlayerCharacter>();
        this.CharacterJudgeBoxController = characterRoot.GetComponent<CharacterJudgeBoxController>();
        this.CharacterPositionStateInCurrentState = positionState;
    }

    public BehaviorEnumSet.State StateName { get; private set; }
    public PassiveStateEnumSet.CharacterPositionState CharacterPositionStateInCurrentState;
    
    protected PlayerCharacter PlayerCharacter;
    protected CharacterAnimator CharacterAnimator;
    protected Transform CharacterTransform;
    protected Rigidbody CharacterRigidBody;
    protected BehaviorStateSimulator StateManager;
    protected CharacterJudgeBoxController CharacterJudgeBoxController;

    

    // AttackLevel은 레벨이 낮은 기술에서 같거나 높은 기술로 연계가 되며 반대는 연계를 못하도록 한다.
    public int AttackLevel { get; protected set; }
    
    public abstract void Enter();

    public abstract void HandleInput(BehaviorEnumSet.Behavior behavior);
    public abstract void UpdateState();
    
    public abstract void Quit();
}

/*
 * 나중에 해당 상태를 몇 초동안 있어야 하고 그런 규약이 생기면
 * 상속한 스테이트에 그런 규약에 맞는 코드를 작성 후
 * 거기에 적용될 숫자는 생성자에서 할당하도록 만들기
 * 그리고 생성자에 넣고 싶은 값은 캐릭터 별로 Json같은 기록할만한 곳에 해두기
 */