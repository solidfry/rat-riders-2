using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class EnemyController : MonoBehaviour, IAttackable
    {
        [SerializeField]
        private bool isTriggered;
        [SerializeField]
        private bool isDead;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private float speed = 2f;
        private Transform target;

        [Header("Stats")] 
        [SerializeField] private int hitPoints = 1;

        List<Collider2D> colliders = new();
        public int HitPoints
        {
            get => hitPoints;
            set
            {
                hitPoints = value;
                SetIsDead();
            }
        }
        
        #region AnimationValues
            private static readonly int IsMoving = Animator.StringToHash("isMoving");
            private static readonly int Death = Animator.StringToHash("Death");
        #endregion

        private void Awake() => colliders = GetComponentsInChildren<Collider2D>().ToList();

        private void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            if (isTriggered && !isDead)
            {
                animator.SetBool(IsMoving, true);
                // This will make this enemy move towards the player
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                target = other.transform;
                Debug.Log("Player triggered event");
                isTriggered = true;
            }
        }
        
        private void SetIsDead()
        {
            if (HitPoints <= 0)
            {
                isDead = true;
                animator.SetTrigger(Death);
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 2;
                rb.simulated = false;
                foreach (var col in colliders) col.enabled = false;
            }
        }

        public void TakeDamage()
        {
            if(isDead)
                return;
            
            HitPoints--;
        }
    }
}
