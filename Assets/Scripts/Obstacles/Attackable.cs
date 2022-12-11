using System;
using Interfaces;
using UnityEngine;

namespace Obstacles
{
    public class Attackable : MonoBehaviour, IAttackable
    {
        [SerializeField] private GameObject destroyParticle;
        [SerializeField] private Color particleColor;
        private ParticleSystem _particleSystem;
        
        public void TakeDamage()
        {
            var particle= Instantiate(destroyParticle, transform.position, Quaternion.identity, GameObject.Find("Obstacles").transform);
            _particleSystem = particle.GetComponent<ParticleSystem>();
            var particleSystemMain = _particleSystem.main;
            particleSystemMain.startColor = particleColor;

            Destroy(gameObject);
            Destroy(particle, 1f);
        }
    }
}