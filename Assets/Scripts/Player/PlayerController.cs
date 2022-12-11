using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Animator animator;
        
        [Header("Movement")]
        [ReadOnly] public bool isFacingRight = true;
        [SerializeField] private float movementSpeed = 8f;
        private float horizontalMovement;
        
        [Header("Jumping")]
        public float jumpForce = 5;
        [ReadOnly] public bool isGrounded;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        
        [Header("Attacking")]
        [SerializeField] private float initialWaitTime;
        [SerializeField] private float attackTimeEnd;
        [SerializeField] private BoxCollider2D attackTrigger;
        
        public bool IsGrounded
        {
            get => isGrounded;
            set => isGrounded = value;
        }

        public bool IsFacingRight
        {
            get => isFacingRight;
            set => isFacingRight = value;
        }

        #region AnimationValues
        private static readonly int JumpAnim = Animator.StringToHash("Jump");
        private static readonly int BiteAnim = Animator.StringToHash("Bite");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        #endregion

        void Awake()
        {
            GetRigidBody();
            GetAnimator();
            attackTrigger = GetComponentInChildren<BoxCollider2D>();
            attackTrigger.gameObject.SetActive(false);
        }

        private void Update()
        {
            rb.velocity = new Vector2(horizontalMovement * movementSpeed, rb.velocity.y);

            if(!IsFacingRight && horizontalMovement > 0)
            {
                Flip();
            }
            else if(IsFacingRight && horizontalMovement < 0)
            {
                Flip();
            }
        }

        void FixedUpdate()
        {
            IsGrounded = IsCharacterGrounded();
            animator.SetBool(Grounded, IsGrounded);
        }

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
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            horizontalMovement = ctx.ReadValue<Vector2>().x;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMovement * movementSpeed));
        }

        bool IsCharacterGrounded() => Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        void Flip()
        {
            IsFacingRight = !IsFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

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

    }
}
