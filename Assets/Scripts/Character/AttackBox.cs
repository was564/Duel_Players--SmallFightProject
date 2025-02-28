using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private CharacterJudgeBoxController _myCharacterJudgeBoxController;
    private BoxCollider _attackBoxCollider;
    private PlayerCharacter _playerCharacter;

    private ParticleSystem _hitParticle;
    private ParticleSystem _guardParticle;
    
    [SerializeField] private int Damage;
    [SerializeField] private BehaviorEnumSet.State StateNameForUnique;
    [SerializeField] public BehaviorEnumSet.AttackPosition AttackPosition;
    [SerializeField] private BehaviorEnumSet.HitReactLevel HitReactLevel;
    
    // public int Damage { get; private set; } = 5;
    
    private float _backMoveSpeedByAttack = 2.0f;
    private Vector3 _hitInAirAwayDirection = new Vector3(1.5f, 5.0f, 0);
    private Vector2 _backFlyOutSpeedByAttack = new Vector2(4.0f, 1.5f);

    private bool _haveCollider;

    // Start is called before the first frame update
    public void Start()
    {
        _myCharacterJudgeBoxController = this.transform.root.GetComponent<CharacterJudgeBoxController>();
        _playerCharacter = _myCharacterJudgeBoxController.GetComponent<PlayerCharacter>();
        _attackBoxCollider = this.GetComponent<BoxCollider>();
        _haveCollider = (_attackBoxCollider != null); // collider == null operation expensive
        DisableAttackBox();
        
        _myCharacterJudgeBoxController.BindAttackBoxByAttackName(StateNameForUnique, this);

        foreach (var childTransform in this.transform.GetComponentsInChildren<ParticleSystem>())
        {
            if (childTransform.tag.Equals("GuardParticle"))
                _guardParticle = childTransform.GetComponent<ParticleSystem>();
            else if (childTransform.tag.Equals("HitParticle"))
                _hitParticle = childTransform.GetComponent<ParticleSystem>();
        }

        _guardParticle.transform.position += Vector3.back * 0.5f;
        _hitParticle.transform.position += Vector3.back * 0.5f;
        
        _guardParticle.Stop();
        _hitParticle.Stop();
    }

    public void DisableAttackBox()
    {
        if(_haveCollider)
            _attackBoxCollider.enabled = false;
    }

    public void EnableAttackBox()
    {
        if(_haveCollider)
            _attackBoxCollider.enabled = true;
    }
    
    public void PlayHitEffect()
    {
        _hitParticle.Play();
    }
    
    public void PlayGuardEffect()
    {
        _guardParticle.Play();
    }

    public void ReactWhenHitByAttack(PlayerCharacter enemyPlayer, BehaviorStateManager stateManager, Rigidbody enemyRigidBody)
    {
        enemyPlayer.DecreaseHp(Damage);
        PlayHitEffect();
        
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
