using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private CharacterJudgeBoxController _myCharacter;
    private BoxCollider _attackAreaCollision2D;
    
    [SerializeField]
    private Behavior.AttackName AttackNameForUnique;

    // Start is called before the first frame update
    void Start()
    {
        _myCharacter = this.GetComponentInParent<CharacterJudgeBoxController>();
        _attackAreaCollision2D = this.GetComponent<BoxCollider>();
        DisableAttackArea();
        
        _myCharacter.BindAttackBoxByAttackName(AttackNameForUnique, this);
    }

    public void DisableAttackArea()
    {
        _attackAreaCollision2D.enabled = false;
    }

    public void EnableAttackArea()
    {
        _attackAreaCollision2D.enabled = true;
    }
}
