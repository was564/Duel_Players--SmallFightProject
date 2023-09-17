using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJudgeBoxController : MonoBehaviour
{
    private HitBox _hitArea;
    
    private Dictionary<BehaviorEnumSet.AttackName, AttackBox> AttackAreaTable =
        new Dictionary<BehaviorEnumSet.AttackName, AttackBox>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableAttackBoxByAttackName(BehaviorEnumSet.AttackName attackName)
    {
        AttackAreaTable[attackName].EnableAttackArea();
    }
    
    public void DisableAttackBoxByAttackName(BehaviorEnumSet.AttackName attackName)
    {
        AttackAreaTable[attackName].DisableAttackArea();
    }

    public void BindAttackBoxByAttackName(BehaviorEnumSet.AttackName attackName, AttackBox attackArea)
    {
        AttackAreaTable.Add(attackName, attackArea);
    }
}
