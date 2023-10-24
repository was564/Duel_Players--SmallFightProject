using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private CharacterJudgeBoxController _myCharacter;
    private BoxCollider _attackBoxCollider;
    
    [SerializeField]
    private BehaviorEnumSet.AttackName AttackNameForUnique;

    [SerializeField] public int Damage { get; private set; } = 5;

    // Start is called before the first frame update
    void Start()
    {
        _myCharacter = this.transform.root.GetComponent<CharacterJudgeBoxController>();
        _attackBoxCollider = this.GetComponent<BoxCollider>();
        DisableAttackBox();
        
        _myCharacter.BindAttackBoxByAttackName(AttackNameForUnique, this);
    }

    public void DisableAttackBox()
    {
        _attackBoxCollider.enabled = false;
    }

    public void EnableAttackBox()
    {
        _attackBoxCollider.enabled = true;
    }
}
