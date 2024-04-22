using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJudgeBoxController : MonoBehaviour
{
    private PlayerCharacter _playerCharacter;
    private HitBox _hitBox;
    
    private Dictionary<BehaviorEnumSet.State, AttackBox> _attackBoxTable =
        new Dictionary<BehaviorEnumSet.State, AttackBox>();
    
    // Start is called before the first frame update
    void Start()
    {
        _playerCharacter = this.GetComponent<PlayerCharacter>();
        _hitBox = this.transform.GetComponentInChildren<HitBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AttackBox GetAttackBox(BehaviorEnumSet.State attackName)
    {
        return _attackBoxTable[attackName];
    }
    
    public bool CompareAttackBoxByAttackPosition(BehaviorEnumSet.State attackName, BehaviorEnumSet.AttackPosition attackPosition)
    {
        if (!_attackBoxTable.ContainsKey(attackName)) return false;
        return _attackBoxTable[attackName].AttackPosition == attackPosition;
    }
    
    public void EnableAttackBoxByAttackName(BehaviorEnumSet.State attackName)
    {
        _attackBoxTable[attackName].EnableAttackBox();
    }
    
    public void DisableAttackBoxByAttackName(BehaviorEnumSet.State attackName)
    {
        _attackBoxTable[attackName].DisableAttackBox();
    }

    public void BindAttackBoxByAttackName(BehaviorEnumSet.State attackName, AttackBox attackArea)
    {
        _attackBoxTable.Add(attackName, attackArea);
    }

    public void EnableGuardPointDuringAttack()
    {
        _playerCharacter.IsGuarded = true;
    }
    
    public void DisableGuardPointDuringAttack()
    {
        _playerCharacter.IsGuarded = false;
    }

    public void EnableHitBox()
    {
        _hitBox.EnableHitBox();
    }

    public void DisableHitBox()
    {
        _hitBox.DisableHitBox();
    }
}
