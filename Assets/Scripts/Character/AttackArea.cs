using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private CharacterAreaController _myCharacter;
    private BoxCollider _attackAreaCollision2D;
    
    [SerializeField]
    private Behavior.AttackName AttackNameForUnique;

    // Start is called before the first frame update
    void Start()
    {
        _myCharacter = this.GetComponentInParent<CharacterAreaController>();
        _attackAreaCollision2D = this.GetComponent<BoxCollider>();
        DisableAttackArea();
        
        _myCharacter.addAttackAreaByAttackName(AttackNameForUnique, this);
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
