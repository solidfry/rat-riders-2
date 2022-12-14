using System;
using System.Collections;
using Enums;
using Interfaces;
using Rage;
using UnityEngine;

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
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float fleeTime = 3f;
        [SerializeField] private float fleeSpeed = 3.5f;
        [SerializeField] private RageValue rageValue = new ();
        
        [Header("State")]
        [SerializeField] private bool isTriggered;
        [SerializeField] private bool isDead;
        [SerializeField] private bool hasAttacked;
        [SerializeField] private bool isAttacking;
        [SerializeField] private bool distance;
        [SerializeField] private MovementType movementType;
        [SerializeField][ReadOnly] private bool isFacingRight = true;
        
        SpriteRenderer spriteRenderer;
        [SerializeField] private LayerMask ground;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] Color deadColor = new Color(100,100,100,255);
        
        [Header("Trail")]
        [SerializeField] private EnemyTrailHandler trail = new();
        
        [SerializeField] private Collider2D hit;

        [Header("Attack")] 
        [SerializeField] private Transform attackOrigin;
        
        Vector2 posLastFrame;
        Vector2 posThisFrame;
        private float direction;
        private Vector2 directionOfTravel;
        
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

        public bool IsAttacking
        {
            get => isAttacking;
            set => isAttacking = value;
        }

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }
        
        public bool IsFacingRight
        {
            get => isFacingRight;
            set => isFacingRight = value;
        }
        

        #region AnimationValues
            private static readonly int IsMoving = Animator.StringToHash("isMoving");
            private static readonly int Death = Animator.StringToHash("Death");
            private static readonly int EnemyAttack = Animator.StringToHash("isAttacking");

        #endregion

        private void Start()
        {
            SetGravity();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            
        }
        
            
        private void Update()
        {
            if(!target || IsDead) return; 
            distance = Vector2.Distance(transform.position, target.position) < attackRange;
            
            FlipSpriteDirection();

            if(IsAttacking) return;
            
            MoveTowards();
            Flee();
            
            if (!distance || HasAttacked) return;
            
            Attack();
        }

        private void FlipSpriteDirection()
        {
            CheckMoveDirection();

            if (!IsFacingRight && rb.velocity.x > 0 )
            {
                Flip();
            }
            else if (IsFacingRight && rb.velocity.x < 0)
            {
                Flip();
            }
            
            void Flip()
            {
                IsFacingRight = !IsFacingRight;
                direction = IsFacingRight ? 1 : -1;

                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        private void CheckMoveDirection()
        {
            posLastFrame = posThisFrame;
            posThisFrame = transform.position;
            
            if (posThisFrame.x > posLastFrame.x)
            {
                IsFacingRight = true;
            }
            if (posThisFrame.x < posLastFrame.x)
            {
                IsFacingRight = false;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
        }

        private void Attack()
        {
            hit = Physics2D.OverlapCircle(attackOrigin.position, attackRange, playerLayer);
            IsAttacking = true;
            
            if (hit != null && hit.TryGetComponent(out IAttackable attackable) && hit.CompareTag("Player"))
                StartCoroutine(DelayAttack(hit, attackable));
        }
        
        private void SetIsDead()
        {
            if(HitPoints > 0) return;
            
            StopAllCoroutines();
            IsDead = true;
            animator.SetTrigger(Death);
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 2;
            trail.DisableTrail();
            spriteRenderer.color = deadColor;
            Destroy(this.gameObject, 1f);
        }
        
        private void SetGravity()
        {
            switch (movementType)
            {
                case MovementType.Flying:
                    rb.gravityScale = 0;
                    break;
                case MovementType.Walking:
                    rb.gravityScale = 2;
                    break;
                default:
                    break;
            }
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
                
                directionOfTravel = target.position - transform.position;

                directionOfTravel = directionOfTravel.normalized;

                rb.AddForce(speed * directionOfTravel);
                
            }
        }
        
        private void Flee()
        {
            if(IsDead || IsAttacking) return;
            
            if(HasAttacked)
            {
                directionOfTravel = target.position - transform.position;

                directionOfTravel = directionOfTravel.normalized;

                rb.AddForce(-1 * fleeSpeed * directionOfTravel);
                
                // This will make this enemy move away from the player
                // transform.position = Vector2.MoveTowards(transform.position, target.position, -1 * fleeSpeed * Time.deltaTime);

                StartCoroutine(SetFleeTime());
            }
        }

        private IEnumerator SetFleeTime()
        {
            yield return new WaitForSeconds(fleeTime);
            HasAttacked = false;
        }

        private IEnumerator DelayAttack(Collider2D raycastHit2D, IAttackable attackable)
        {
            animator.SetBool(EnemyAttack, true);
            yield return new WaitForSeconds(.5f);
            
            if(distance)
            {
                Debug.Log("The distance was too far so the attack missed");
                attackable.TakeDamage();
            }
            
            Debug.Log($"Hit {raycastHit2D.GetComponent<Collider>()}" );
            
            yield return new WaitForSeconds(.5f);
            IsAttacking = false;
            animator.SetBool(EnemyAttack, false);
            HasAttacked = true;
            Flee();
        }

        public int GetRageValue()
        {
            return rageValue.GetRageValue();
        }

       
    }
}
