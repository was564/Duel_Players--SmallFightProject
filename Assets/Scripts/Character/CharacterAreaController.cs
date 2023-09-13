using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAreaController : MonoBehaviour
{
    private HitArea _hitArea;
    
    private Dictionary<Behavior.AttackName, AttackArea> AttackAreaTable =
        new Dictionary<Behavior.AttackName, AttackArea>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableAttackAreaByAttackName(Behavior.AttackName attackName)
    {
        AttackAreaTable[attackName].EnableAttackArea();
    }
    
    public void DisableAttackAreaByAttackName(Behavior.AttackName attackName)
    {
        AttackAreaTable[attackName].DisableAttackArea();
    }

    public void addAttackAreaByAttackName(Behavior.AttackName attackName, AttackArea attackArea)
    {
        AttackAreaTable.Add(attackName, attackArea);
    }
}
