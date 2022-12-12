using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class Attackable : MonoBehaviour, IAttackable
    {
        [SerializeField] private GameObject destroyParticle;
        [SerializeField] private Color particleColor;
        private ParticleSystem _particleSystem;
        [SerializeField] private bool destroyOnDeath = false;
        
        public void TakeDamage()
        {
            var particle= Instantiate(destroyParticle, transform.position, Quaternion.identity);
            _particleSystem = particle.GetComponent<ParticleSystem>();
            var particleSystemMain = _particleSystem.main;
            particleSystemMain.startColor = particleColor;
            Destroy(particle, 1f);
            if(destroyOnDeath)
                Destroy(gameObject);
        }
    }
}