using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJudgeBoxController : MonoBehaviour
{
    private HitBox _hitArea;
    
    private Dictionary<Behavior.AttackName, AttackBox> AttackAreaTable =
        new Dictionary<Behavior.AttackName, AttackBox>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableAttackBoxByAttackName(Behavior.AttackName attackName)
    {
        AttackAreaTable[attackName].EnableAttackArea();
    }
    
    public void DisableAttackBoxByAttackName(Behavior.AttackName attackName)
    {
        AttackAreaTable[attackName].DisableAttackArea();
    }

    public void BindAttackBoxByAttackName(Behavior.AttackName attackName, AttackBox attackArea)
    {
        AttackAreaTable.Add(attackName, attackArea);
    }
}
