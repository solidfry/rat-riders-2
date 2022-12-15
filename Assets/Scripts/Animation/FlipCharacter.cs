using UnityEngine;

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
    void Update()
    {
        velocityX = rb.velocity.normalized.x;

        FlipSpriteDirection();
    }

    private void FlipSpriteDirection()
    {
        if (velocityX != 0)
            previousVelocity = velocityX;

        CheckMoveDirection();
        Flip();

        void Flip()
        {
            direction = IsFacingRight ? 1 : -1;
            Vector3 localScale = transform.localScale;
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
