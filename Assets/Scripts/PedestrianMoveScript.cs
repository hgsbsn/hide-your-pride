using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PedestrianMoveScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float minMoveTime = 2f;
    [SerializeField] private float maxMoveTime = 4f;
    [SerializeField] private float minPauseTime = 1f;
    [SerializeField] private float maxPauseTime = 2f;

    [Header("Directional Sprites")]
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;
    private float stateTimer;
    private bool isMoving = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDirection();
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            if (isMoving)
                Pause();
            else
                Resume();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = isMoving ? moveDirection * moveSpeed : Vector2.zero;
    }

    private void SetNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        UpdateSpriteDirection();
    }

    private void Resume()
    {
        isMoving = true;
        SetNewDirection();
        stateTimer = Random.Range(minMoveTime, maxMoveTime);
    }

    private void Pause()
    {
        isMoving = false;
        stateTimer = Random.Range(minPauseTime, maxPauseTime);
    }

    private void UpdateSpriteDirection()
    {
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            spriteRenderer.sprite = moveDirection.x > 0 ? rightSprite : leftSprite;
        }
        else
        {
            spriteRenderer.sprite = moveDirection.y > 0 ? upSprite : downSprite;
        }
    }
}