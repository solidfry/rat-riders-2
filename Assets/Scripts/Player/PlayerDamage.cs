using System.Collections;
using Events;
using UnityEngine;

namespace Player
{
    public class PlayerDamage : MonoBehaviour
    {
        [Header("Particle Settings")]
        [SerializeField] private GameObject damageParticle;
        [SerializeField] private Color particleColor;

        [Space(20)]

        private SpriteRenderer _spriteRenderer;
        [Header("Damage Colour Settings")]
        [SerializeField] private Color damageColor;
        [SerializeField][ReadOnly] private Color originalColor;
        [SerializeField] private float damagedFlashTime;
        [SerializeField] private float damageFlashCount;
        [SerializeField][ReadOnly] private bool damaged;
        private void Awake()
        {
            _spriteRenderer = GetComponentInParent<SpriteRenderer>();
            originalColor = _spriteRenderer.color;
        }

        private void OnEnable() => GameEvents.onPlayerDamagedEvent += SetDamaged;
        private void OnDisable() => GameEvents.onPlayerDamagedEvent -= SetDamaged;

        private void Update()
        {
            var spriteRendererColor = damaged
                ? _spriteRenderer.color = LerpColor(originalColor, damageColor)
                : _spriteRenderer.color = originalColor;
        }

        private void SetDamaged()
        {
            damaged = true; StartCoroutine(DelayedSetDamage());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Obstacle"))
            {
                var particle = Instantiate(damageParticle, transform.position, Quaternion.identity, GameObject.Find("Obstacles").transform);
                var ps = particle.GetComponent<ParticleSystem>();
                var particleSystemMain = ps.main;
                particleSystemMain.startColor = particleColor;
                Destroy(particle, 1f);

                Destroy(col.gameObject);
                GameEvents.onPlayerDamagedEvent?.Invoke();
            }
        }

        Color LerpColor(Color a, Color b) => Color.Lerp(a, b, Mathf.PingPong(Time.time * damageFlashCount, .5f));

        IEnumerator DelayedSetDamage()
        {
            yield return new WaitForSeconds(damagedFlashTime);
            damaged = false;
        }

    }
}