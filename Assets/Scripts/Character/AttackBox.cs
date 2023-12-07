using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private CharacterJudgeBoxController _myCharacterJudgeBoxController;
    private BoxCollider _attackBoxCollider;
    private PlayerCharacter _playerCharacter;
    
    public ParticleSystem HitParticle { get; private set; }
    public ParticleSystem GuardParticle { get; private set; }
    
    [SerializeField] private BehaviorEnumSet.AttackName AttackNameForUnique;
    [SerializeField] public BehaviorEnumSet.AttackPosition AttackPosition;
    [SerializeField] private BehaviorEnumSet.HitReactLevel HitReactLevel;
    
    public int Damage { get; private set; } = 5;
    
    private float _backMoveSpeedByAttack = 2.0f;
    private Vector3 _hitInAirAwayDirection = new Vector3(1.5f, 5.0f, 0);
    private Vector2 _backFlyOutSpeedByAttack = new Vector2(4.0f, 1.5f);
    

    // Start is called before the first frame update
    void Start()
    {
        _myCharacterJudgeBoxController = this.transform.root.GetComponent<CharacterJudgeBoxController>();
        _playerCharacter = _myCharacterJudgeBoxController.GetComponent<PlayerCharacter>();
        _attackBoxCollider = this.GetComponent<BoxCollider>();
        DisableAttackBox();
        
        _myCharacterJudgeBoxController.BindAttackBoxByAttackName(AttackNameForUnique, this);

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

    public void ReactWhenHitByAttack(PlayerCharacter enemyPlayer, BehaviorStateManager stateManager, Rigidbody enemyRigidBody)
    {
        enemyPlayer.DecreaseHp(Damage);
        HitParticle.Play();
        
        if (HitReactLevel == BehaviorEnumSet.HitReactLevel.HitInPlace)
        {
            switch (enemyPlayer.CharacterPositionState)
            {
                case PassiveStateEnumSet.CharacterPositionState.OnGround:
                    stateManager.ChangeState(BehaviorEnumSet.State.StandingHit);
                    enemyRigidBody.velocity = Vector3.left 
                                              * (((_playerCharacter.transform.position.x - _playerCharacter.EnemyObject.transform.position.x) > 0.0f ? 1.0f : -1.0f) 
                                                 * _backMoveSpeedByAttack);
                    break;
                case PassiveStateEnumSet.CharacterPositionState.Crouch:
                    stateManager.ChangeState(BehaviorEnumSet.State.CrouchHit);
                    enemyRigidBody.velocity = Vector3.left 
                                              * (((_playerCharacter.transform.position.x - _playerCharacter.EnemyObject.transform.position.x) > 0.0f ? 1.0f : -1.0f) 
                                                 * _backMoveSpeedByAttack);
                    break;
                case PassiveStateEnumSet.CharacterPositionState.InAir:
                    stateManager.ChangeState(BehaviorEnumSet.State.InAirHit);
                    Vector3 resultDirection = _hitInAirAwayDirection;
                    resultDirection.x *= (enemyPlayer.transform.position - enemyPlayer.EnemyObject.transform.position).x > 0.0f ? 1.0f : -1.0f;
                    enemyRigidBody.velocity = resultDirection;
                    break;
            }
            
        }
        else if (HitReactLevel == BehaviorEnumSet.HitReactLevel.HitFlyOut)
        {
            stateManager.ChangeState(BehaviorEnumSet.State.InAirHit);
            enemyRigidBody.velocity = Vector3.left 
                                      * (((_playerCharacter.transform.position.x - _playerCharacter.EnemyObject.transform.position.x) > 0.0f ? 1.0f : -1.0f) 
                                         * _backFlyOutSpeedByAttack.x)
                                      + (Vector3.up * _backFlyOutSpeedByAttack.y);
        }
        
    }
}
