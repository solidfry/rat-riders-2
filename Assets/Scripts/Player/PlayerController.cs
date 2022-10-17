using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerControls _controls;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Animator animator;
    
        public float jumpForce = 5;
        [ReadOnly] public bool isGrounded;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float initialWaitTime;
        [SerializeField] private float attackTimeEnd;
        [SerializeField] private BoxCollider2D attackTrigger;
    
        #region AnimationValues
        private static readonly int JumpAnim = Animator.StringToHash("Jump");
        private static readonly int BiteAnim = Animator.StringToHash("Bite");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        #endregion
    
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            _controls = new PlayerControls();
            _controls.Enable();
            attackTrigger = GetComponentInChildren<BoxCollider2D>();
            attackTrigger.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _controls.Touch.Jump.performed += Jump;
            _controls.Touch.Bite.performed += Bite;
        }

        private void OnDisable()
        {
            _controls.Touch.Jump.performed -= Jump;
            _controls.Touch.Bite.performed -= Bite;
            _controls.Disable();
        }

        void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            animator.SetBool(Grounded, isGrounded);
        }

        private void Bite(InputAction.CallbackContext ctx)
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

        private void Jump(InputAction.CallbackContext ctx)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(0, jumpForce);
                animator.SetTrigger(JumpAnim);
            }
        }

    }
}
