using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJudgeBoxController : MonoBehaviour
{
    private HitBox _hitBox;
    
    private Dictionary<BehaviorEnumSet.AttackName, AttackBox> _attackBoxTable =
        new Dictionary<BehaviorEnumSet.AttackName, AttackBox>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AttackBox GetAttackBox(BehaviorEnumSet.AttackName attackName)
    {
        return _attackBoxTable[attackName];
    }
    
    public void EnableAttackBoxByAttackName(BehaviorEnumSet.AttackName attackName)
    {
        _attackBoxTable[attackName].EnableAttackBox();
    }
    
    public void DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName attackName)
    {
        _attackBoxTable[attackName].DisableAttackBox();
    }

    public void BindAttackBoxByAttackName(BehaviorEnumSet.AttackName attackName, AttackBox attackArea)
    {
        _attackBoxTable.Add(attackName, attackArea);
    }
}
