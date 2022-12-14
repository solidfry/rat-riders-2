using System.Collections;
using Events;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerDamage : MonoBehaviour, IAttackable
    {
        [Header("Particle Settings")]
        [SerializeField] private GameObject damageParticle;
        [SerializeField] private Color particleColor;

        [Space(20)]

        private SpriteRenderer spriteRenderer;
        
        [Header("Damage Colour Settings")]
        [SerializeField] private Color damageColor;
        [SerializeField][ReadOnly] private Color originalColor;
        [SerializeField] private float damagedFlashTime;
        [SerializeField] private float damageFlashCount;
        [SerializeField][ReadOnly] private bool damaged;
        
        private void Awake()
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
        }

        private void OnEnable() => GameEvents.onPlayerDamagedEvent += SetDamaged;
        private void OnDisable() => GameEvents.onPlayerDamagedEvent -= SetDamaged;

        private void Update()
        {
            var spriteRendererColor = damaged
                ? spriteRenderer.color = LerpColor(originalColor, damageColor)
                : spriteRenderer.color = originalColor;
        }

        private void SetDamaged()
        {
            damaged = true; StartCoroutine(DelayedSetDamage());
        }

        Color LerpColor(Color a, Color b) => Color.Lerp(a, b, Mathf.PingPong(Time.time * damageFlashCount, .5f));

        IEnumerator DelayedSetDamage()
        {
            yield return new WaitForSeconds(damagedFlashTime);
            damaged = false;
        }

        public void TakeDamage()
        {
            var particle = Instantiate(damageParticle, transform.position, Quaternion.identity);
            var ps = particle.GetComponent<ParticleSystem>();
            var particleSystemMain = ps.main;
            particleSystemMain.startColor = particleColor;
            Destroy(particle, 1f);

            GameEvents.onPlayerDamagedEvent?.Invoke();
        }
    }
}