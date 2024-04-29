using System;
using Character.PlayerMode;
using Unity.VisualScripting;
using UnityEngine;

public class ChasingBall : MonoBehaviour
{
    private PlayerCharacter _owner;
    
    private ParticleSystem _fireParticle;

    private SphereCollider _collider;
    
    public Transform Target { get; set; }
    
    private float _speed = 2.0f;
    public void Start()
    {
        foreach (var particle in transform.GetComponentsInChildren<ParticleSystem>())
        {
            if(particle.CompareTag("FireParticle")) _fireParticle = particle;
        }

        _owner = this.transform.root.GetComponent<PlayerCharacter>();
        _collider = this.GetComponent<SphereCollider>();
        
        this.transform.parent = this.transform.root.parent;
        
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_owner == null) return;
        _collider.enabled = _owner.GetPlayerMode() != PlayerModeManager.PlayerMode.Replaying;
        
        if(_fireParticle != null) _fireParticle.Play();
    }
    
    private void OnDisable()
    {
        if(_fireParticle != null) _fireParticle.Stop();
    }

    private void Update()
    {
        float distance = Vector3.Distance(Target.position + Vector3.up * 0.5f, this.transform.position);
        if (distance < _speed * Time.deltaTime)
            this.transform.position = Target.position + Vector3.up * 0.5f;
        else
        {
            Vector3 direction = (Target.position - this.transform.position).normalized;
            Vector3 position = this.transform.position + direction * (_speed * Time.deltaTime);
            position.y = Target.position.y + 0.5f;
            this.transform.position = position;
        }

        if (_collider.enabled == false && distance < 0.5f)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == this.tag) return;
        this.gameObject.SetActive(false);
    }
}