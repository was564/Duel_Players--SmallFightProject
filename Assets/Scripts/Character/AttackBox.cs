using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private CharacterJudgeBoxController _myCharacterJudgeBoxController;
    private BoxCollider _attackBoxCollider;
    private PlayerCharacter _playerCharacter;
    
    public ParticleSystem HitParticle { get; private set; }
    public ParticleSystem GuardParticle { get; private set; }
    
    [SerializeField] private BehaviorEnumSet.State StateNameForUnique;
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
        
        _myCharacterJudgeBoxController.BindAttackBoxByAttackName(StateNameForUnique, this);

        foreach (var childTransform in this.transform.GetComponentsInChildren<Transform>())
        {
            if (childTransform.tag.Equals("GuardParticle"))
                GuardParticle = childTransform.GetComponent<ParticleSystem>();
            else if (childTransform.tag.Equals("HitParticle"))
                HitParticle = childTransform.GetComponent<ParticleSystem>();
        }

        GuardParticle.transform.position += Vector3.back * 0.5f;
        HitParticle.transform.position += Vector3.back * 0.5f;
        
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
            switch (enemyPlayer.CurrentCharacterPositionState)
            {
                case PassiveStateEnumSet.CharacterPositionState.OnGround:
                    enemyRigidBody.velocity = Vector3.left 
                                              * (((_playerCharacter.transform.position.x - _playerCharacter.EnemyObject.transform.position.x) > 0.0f ? 1.0f : -1.0f) 
                                                 * _backMoveSpeedByAttack);
                    stateManager.ChangeState(BehaviorEnumSet.State.StandingHit);
                    break;
                case PassiveStateEnumSet.CharacterPositionState.Crouch:
                    enemyRigidBody.velocity = Vector3.left 
                                              * (((_playerCharacter.transform.position.x - _playerCharacter.EnemyObject.transform.position.x) > 0.0f ? 1.0f : -1.0f) 
                                                 * _backMoveSpeedByAttack);
                    stateManager.ChangeState(BehaviorEnumSet.State.CrouchHit);
                    break;
                case PassiveStateEnumSet.CharacterPositionState.InAir:
                    Vector3 resultDirection = _hitInAirAwayDirection;
                    resultDirection.x *= (enemyPlayer.transform.position - enemyPlayer.EnemyObject.transform.position).x > 0.0f ? 1.0f : -1.0f;
                    enemyRigidBody.velocity = resultDirection;
                    stateManager.ChangeState(BehaviorEnumSet.State.InAirHit);
                    break;
            }
            
        }
        else if (HitReactLevel == BehaviorEnumSet.HitReactLevel.HitFlyOut)
        {
            enemyRigidBody.velocity = Vector3.left 
                                      * (((_playerCharacter.transform.position.x - _playerCharacter.EnemyObject.transform.position.x) > 0.0f ? 1.0f : -1.0f) 
                                         * _backFlyOutSpeedByAttack.x)
                                      + (Vector3.up * _backFlyOutSpeedByAttack.y);
            stateManager.ChangeState(BehaviorEnumSet.State.InAirHit);
        }
        
    }
}
