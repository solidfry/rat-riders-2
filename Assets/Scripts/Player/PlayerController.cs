using System.Collections;
using Enemies;
using Events;
using Rage;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : Attackable
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Animator animator;
        [SerializeField] RageMeter rageMeter;
        
        [Header("Movement")]
        [ReadOnly] public bool isFacingRight = true;
        [SerializeField] private float movementSpeed = 8f;
        private float horizontalMovement;
        [SerializeField] private bool isFalling;
        
        [Header("Jumping")]
        public float jumpForce = 5;
        [ReadOnly] public bool isGrounded;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        
        [Header("Attacking")]
        [SerializeField] private float initialWaitTime;
        [SerializeField] private float attackTimeEnd;
        [SerializeField] private BoxCollider2D attackTrigger;

        #region Fields

        public bool IsGrounded
        {
            get => isGrounded;
            set => isGrounded = value;
        }

        public bool IsFalling
        {
            get => isFalling;
            set => isFalling = value;
        }

        public bool IsFacingRight
        {
            get => isFacingRight;
            set => isFacingRight = value;
        }
        

        #endregion
        #region AnimationValues
        private static readonly int JumpAnim = Animator.StringToHash("Jump");
        private static readonly int BiteAnim = Animator.StringToHash("Bite");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Falling = Animator.StringToHash("isFalling");
        #endregion

        void Awake()
        {
            GetRigidBody();
            GetAnimator();
            attackTrigger = GetComponentInChildren<BoxCollider2D>();
            attackTrigger.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvents.onPlayerChangeRageEvent += rageMeter.AddRage;
        }
        
        private void OnDisable()
        {
            GameEvents.onPlayerChangeRageEvent -= rageMeter.AddRage;
        }

        private void Update()
        {
            // ToDo: Raycast test for attacking. Might use later
            // Vector2 direction = IsFacingRight ? Vector2.right : Vector2.left;
            // RaycastHit2D raycastAttack = Physics2D.Raycast(transform.position, Vector2.right * direction);
            // Debug.DrawRay(raycastAttack.point, direction, Color.red);
            
            SetFallingState();

            rb.velocity = new Vector2(horizontalMovement * movementSpeed, rb.velocity.y);
            IsGrounded = IsCharacterGrounded();
            animator.SetBool(Grounded, IsGrounded);
            
            SetFlipFacing();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }

        #region Abilities

        public void Bite(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                animator.SetTrigger(BiteAnim);
                StartCoroutine(ToggleAttackTrigger(initialWaitTime, attackTimeEnd));
            }
            
            IEnumerator ToggleAttackTrigger(float waitTime, float attackEnd)
            {
                yield return new WaitForSeconds(waitTime);
                attackTrigger.gameObject.SetActive(true);
                yield return new WaitForSeconds(attackEnd);
                attackTrigger.gameObject.SetActive(false);
            }
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && IsCharacterGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger(JumpAnim);
            }
            
            if(ctx.canceled && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }

            SetGravityScaleOnJump();
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            horizontalMovement = ctx.ReadValue<Vector2>().x;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMovement * movementSpeed));
        }

        #endregion
        
        #region Setup Methods
        bool IsCharacterGrounded() => Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        void GetRigidBody()
        {
            if (rb != null)
                return;
            
            rb = GetComponent<Rigidbody2D>();
        }

        void GetAnimator()
        {
            if(animator != null)
                return;
            
            animator = GetComponent<Animator>();
        }
        
        void SetGravityScaleOnJump()
        {
            // if the player is moving downwards and is not grounded, set the gravity scale to 2 else set it to 1
            if (rb.velocity.y < 0.1f && !IsGrounded)
            {
                rb.gravityScale = 2;
            }
            else
            {
                rb.gravityScale = 1;
            }
        }
        
        private void SetFallingState()
        {
            if(IsGrounded) return;
            
            IsFalling = rb.velocity.y < 0;
            
            if (IsFalling)
            {
                rb.gravityScale = 2.5f;
                animator.SetBool(Falling,  true);
            }
            else
            {
                animator.SetBool(Falling,  false);
                rb.gravityScale = 1;
            }
        }
        
        private void SetFlipFacing()
        {
            if (!IsFacingRight && horizontalMovement > 0)
            {
                Flip();
            }
            else if (IsFacingRight && horizontalMovement < 0)
            {
                Flip();
            }
            
            void Flip()
            {
                IsFacingRight = !IsFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        #endregion
        
        
        
    }
}
