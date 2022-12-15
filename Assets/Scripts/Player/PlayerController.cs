using System.Collections;
using Enemies;
using Events;
using Interfaces;
using Rage;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utilities;

namespace Player
{
    public class PlayerController : Attackable
    {
        [Header("Setup")]
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Animator animator;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField][ReadOnly] private bool isDead;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 8f;
        private float horizontalMovement;
        [SerializeField] private bool isFalling;
        private int direction;
        private RaycastHit2D ray;

        [Header("Jumping")]
        public float jumpForce = 5;
        [ReadOnly] public bool isGrounded;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;

        [Header("Attacking")]
        [SerializeField][ReadOnly] bool canAttack = true;
        [SerializeField] private float biteAttackCoolDown;
        [SerializeField] private float biteAttackLength = 0.5f;
        [SerializeField] private Transform attackTransform;
        [SerializeField] private LayerMask attackLayer;

        [Header("Rage")]
        [SerializeField] RageMeter rageMeter;

        #region Fields

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }
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
        #endregion

        #region AnimationValues
        private static readonly int JumpAnim = Animator.StringToHash("Jump");
        private static readonly int BiteAnim = Animator.StringToHash("Bite");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Falling = Animator.StringToHash("isFalling");
        private static readonly int Speed = Animator.StringToHash("Speed");

        #endregion

        void Awake()
        {
            GetRigidBody();
            GetAnimator();
        }

        private void OnEnable()
        {
            GameEvents.onPlayerChangeRageEvent += rageMeter.ChangeRage;
            GameEvents.onPlayerDiedEvent += Die;
        }

        private void OnDisable()
        {
            GameEvents.onPlayerChangeRageEvent -= rageMeter.ChangeRage;
            GameEvents.onPlayerDiedEvent -= Die;
        }

        private void Update()
        {
            // Bite attack ray
            ray = Physics2D.CircleCast(attackTransform.position, biteAttackLength, Vector2.right * direction, 0f, attackLayer);

            SetFallingState();

            rb.velocity = new Vector2(horizontalMovement * movementSpeed, rb.velocity.y);
            IsGrounded = IsCharacterGrounded();
            animator.SetBool(Grounded, IsGrounded);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackTransform.position, biteAttackLength);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }

        #region Abilities

        public void Bite(InputAction.CallbackContext ctx)
        {
            if (canAttack && ctx.started)
            {
                GameEvents.onScreenShakeEvent?.Invoke(CameraShake.Strength.VeryLow);
                StartCoroutine(AttackCooldown(biteAttackCoolDown));
                animator.SetTrigger(BiteAnim);
                if (ray.collider != null)
                {
                    if (ray.collider.TryGetComponent(out IAttackable attackable))
                    {
                        attackable.TakeDamage();

                        Debug.Log($"{attackable} was attacked");

                        if (ray.collider.TryGetComponent(out IRage rage))
                        {
                            GameEvents.onPlayerChangeRageEvent?.Invoke(rage.GetRageValue());
                            Debug.Log("Rage has been sent");
                        }
                    }
                }
            }
        }

        private IEnumerator AttackCooldown(float waitTime)
        {
            canAttack = false;
            yield return new WaitForSeconds(waitTime);
            canAttack = true;
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && IsCharacterGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger(JumpAnim);
            }

            if (ctx.canceled && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }

            SetGravityScaleOnJump();
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            horizontalMovement = ctx.ReadValue<Vector2>().x;
            animator.SetFloat(Speed, Mathf.Abs(horizontalMovement * movementSpeed));
        }

        public void Die()
        {
            IsDead = true;
            Debug.Log("Player died");
            playerInput.DeactivateInput();
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
            if (animator != null)
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
            if (IsGrounded) return;

            IsFalling = rb.velocity.y < 0;

            if (IsFalling)
            {
                rb.gravityScale = 2.5f;
                animator.SetBool(Falling, true);
            }
            else
            {
                animator.SetBool(Falling, false);
                rb.gravityScale = 1;
            }
        }

        #endregion



    }
}
