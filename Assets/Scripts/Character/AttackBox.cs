using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private CharacterJudgeBoxController _myCharacter;
    private BoxCollider _attackBoxCollider;

    public ParticleSystem HitParticle { get; private set; }
    public ParticleSystem GuardParticle { get; private set; }
    
    [SerializeField] private BehaviorEnumSet.AttackName AttackNameForUnique;

    public int Damage { get; private set; } = 5;
    

    // Start is called before the first frame update
    void Start()
    {
        _myCharacter = this.transform.root.GetComponent<CharacterJudgeBoxController>();
        _attackBoxCollider = this.GetComponent<BoxCollider>();
        DisableAttackBox();
        
        _myCharacter.BindAttackBoxByAttackName(AttackNameForUnique, this);

        foreach (var childTransform in this.transform.GetComponentsInChildren<Transform>())
        {
            if (childTransform.tag.Equals("GuardParticle"))
                GuardParticle = childTransform.GetComponent<ParticleSystem>();
            else if (childTransform.tag.Equals("HitParticle"))
                HitParticle = childTransform.GetComponent<ParticleSystem>();
        }
        GuardParticle.Stop();
        HitParticle.Stop();
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
