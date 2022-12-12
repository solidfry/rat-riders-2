using System.Collections;
using Enums;
using Interfaces;
using Rage;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class EnemyController : Attackable, IAttackable, IRage
    {
        
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Rigidbody2D rb;
        private Transform target;
        
        [Header("Stats")] 
        [SerializeField] private int hitPoints = 1;
        [SerializeField] private float speed = 2f;
        // [SerializeField] private float attackRange = 1f;
        [SerializeField] private float fleeTime = 3f;
        [SerializeField] private float fleeSpeed = 3.5f;
        [SerializeField] private RageValue rageValue = new ();
        
        [Header("State")]
        [SerializeField]
        private bool isTriggered;
        [SerializeField]
        private bool isDead;
        [FormerlySerializedAs("isAttacking")] [SerializeField]
        private bool hasAttacked;
        [SerializeField]
        private MovementType movementType;
        SpriteRenderer spriteRenderer;
        [SerializeField] private LayerMask ground;
        [SerializeField] Color deadColor = new Color(100,100,100,255);
        [Header("Trail")]
        [SerializeField] private EnemyTrailHandler trail = new();
        
        public int HitPoints
        {
            get => hitPoints;
            set
            {
                hitPoints = value;
                SetIsDead();
            }
        }

        public bool HasAttacked
        {
            get => hasAttacked;
            set => hasAttacked = value;
        }

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        #region AnimationValues
            private static readonly int IsMoving = Animator.StringToHash("isMoving");
            private static readonly int Death = Animator.StringToHash("Death");
        #endregion

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            MoveTowards();
            Flee();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
                HasAttacked = true;
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
            if(HitPoints > 0) return;
            
            IsDead = true;
            animator.SetTrigger(Death);
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 2;
            trail.DisableTrail();
            spriteRenderer.color = deadColor;
        }

        public new void TakeDamage()
        {
            base.TakeDamage();
            if(IsDead)
                return;
            
            HitPoints--;
        }

        private void MoveTowards()
        {
            if(IsDead) return;

            if (isTriggered && !HasAttacked)
            {
                animator.SetBool(IsMoving, true);
                // This will make this enemy move towards the player
                if(movementType == MovementType.Flying)
                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
        
        private void Flee()
        {
            if(IsDead) return;
            
            if(HasAttacked)
            {
                // This will make this enemy move away from the player
                if(movementType == MovementType.Flying)
                    transform.position = Vector2.MoveTowards(transform.position, target.position, -1 * fleeSpeed * Time.deltaTime);

                StartCoroutine(SetFleeTime());
            }
        }

        private IEnumerator SetFleeTime()
        {
            yield return new WaitForSeconds(fleeTime);
            HasAttacked = false;
        }

        public int GetRageValue()
        {
            return rageValue.GetRageValue();
        }
    }
}
