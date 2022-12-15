using UnityEngine;

namespace Animation
{
    public class FlipCharacter : MonoBehaviour
    {

        [SerializeField] float velocityX = 1f;
        float previousVelocity;
        [SerializeField] private float direction;
        [SerializeField][ReadOnly] private bool isFacingRight = true;
        Rigidbody2D rb;

        public bool IsFacingRight
        {
            get => isFacingRight;
            set => isFacingRight = value;
        }

        private void Start() => rb = GetComponent<Rigidbody2D>();

        // Update is called once per frame
        void FixedUpdate()
        {
            FlipSpriteDirection();
            velocityX = rb.velocity.normalized.x;

        }

        private void FlipSpriteDirection()
        {
            if (velocityX != 0)
                previousVelocity = velocityX;

            CheckMoveDirection();
            Flip();

            void Flip()
            {
                Vector3 localScale = transform.localScale;
                direction = IsFacingRight ? 1 : -1;
                localScale.x = direction;
                transform.localScale = localScale;
            }
        }

        private void CheckMoveDirection()
        {
            if (velocityX > 0)
            {
                IsFacingRight = true;
            }
            else if (velocityX < 0)
            {
                IsFacingRight = false;
            }

        }
    }
}
