using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class RandomNPCMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float directionChangeInterval = 2f;

    [Header("Directional Sprites")]
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("Child Pointers")]
    [SerializeField] private GameObject directionalPointerUp;
    [SerializeField] private GameObject directionalPointerDown;
    [SerializeField] private GameObject directionalPointerLeft;
    [SerializeField] private GameObject directionalPointerRight;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseNewDirection();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= directionChangeInterval)
        {
            ChooseNewDirection();
            timer = 0f;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        // Disable all pointers initially
        directionalPointerUp.SetActive(false);
        directionalPointerDown.SetActive(false);
        directionalPointerLeft.SetActive(false);
        directionalPointerRight.SetActive(false);

        // Update the sprite based on major direction and enable the matching pointer
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            if (moveDirection.x > 0)
            {
                spriteRenderer.sprite = rightSprite;
                directionalPointerRight.SetActive(true);
            }
            else
            {
                spriteRenderer.sprite = leftSprite;
                directionalPointerLeft.SetActive(true);
            }
        }
        else
        {
            if (moveDirection.y > 0)
            {
                spriteRenderer.sprite = upSprite;
                directionalPointerUp.SetActive(true);
            }
            else
            {
                spriteRenderer.sprite = downSprite;
                directionalPointerDown.SetActive(true);
            }
        }
    }
}