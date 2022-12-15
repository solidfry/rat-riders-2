using UnityEngine;

public class FlipCharacter : MonoBehaviour
{

    [SerializeField] float velocityX;
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
        FlipSpriteDirection();
    }

    private void FlipSpriteDirection()
    {
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
        velocityX = Mathf.Clamp(rb.velocity.normalized.x, -1, 1);

        if (velocityX > 0)
        {
            IsFacingRight = true;
        }
        if (velocityX < 0)
        {
            IsFacingRight = false;
        }
    }
}
