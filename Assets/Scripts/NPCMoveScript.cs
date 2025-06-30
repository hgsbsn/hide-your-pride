using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class RandomNPCMovement : MonoBehaviour
{
    [SerializeField] private float defaultMoveSpeed = 2f;
    private float moveSpeed = 2f;
    private float directionChangeInterval = 2f;

    [Header("Directional Sprites")]
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Vector2 moveDirection;
    public Vector2 spottedPlayerDirection = Vector2.right;
    public bool playerSpotted = false;
    private float timer;
    [SerializeField] private FieldOfView2D fieldOfView;
    [SerializeField] private float minInterval = 2f;
    [SerializeField] private float maxInterval = 20f;
    public bool family = true;
    public PlayerMovement2D player;
    public GameManagerScript gameManager;
    public GameObject speechBubble;
    public float personalLeoTimeReset = 30f;
    [SerializeField] private float personalLeoTime = 30f;
    [SerializeField] private float minPersonalTime = 30f;
    [SerializeField] private float maxPersonalTime = 45f;

    [SerializeField] private float stopDistanceFromPlayer = 1.5f;
    [SerializeField] private GameObject personalTimeIndicator;

    //[SerializeField] private RaycastVision raycastVision;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResumeMovement();
        RandomizeInterval();
        personalLeoTime = Random.Range(minPersonalTime, maxPersonalTime);
    }

    private enum MovementState { Moving, Paused }
    private MovementState currentState = MovementState.Moving;

    private float stateTimer;
    [SerializeField] private float pauseDurationMin = 1f;
    [SerializeField] private float pauseDurationMax = 3f;

    private void Update()
    {
        fieldOfView.facingDirection = moveDirection;
        stateTimer -= Time.deltaTime;

        if (playerSpotted)
        {
            FacePlayer();
            return; // Skip idle behavior
        }

        if (personalLeoTime <= 0f)
        {
            // Aggressive state: chase the player constantly
            moveSpeed = defaultMoveSpeed * 2f;
            personalTimeIndicator.SetActive(true);
            Vector2 toPlayer = (player.transform.position - transform.position).normalized;
            Vector2 randomOffset = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
            moveDirection = (toPlayer + randomOffset).normalized;
            moveDirection = toPlayer;
            UpdateSpriteDirection();
        }
        else
        {
            // Normal idle/pause behavior
            switch (currentState)
            {
                case MovementState.Moving:
                    if (stateTimer <= 0f)
                    {
                        PauseMovement();
                    }
                    break;

                case MovementState.Paused:
                    if (stateTimer <= 0f)
                    {
                        ResumeMovement();
                    }
                    break;
            }
        }
    }


    private void FixedUpdate()
    {
        if (playerSpotted)
        {
            float distance = Vector2.Distance(transform.position, spottedPlayerDirection);

            if (player.inTransition)
            {
                gameManager.GameOver();
                return;
            }

            if (distance <= stopDistanceFromPlayer)
            {
                personalTimeIndicator.SetActive(false);
                moveSpeed = defaultMoveSpeed;
                rb.linearVelocity = Vector2.zero;
                if (family && !player.masc || !family && player.masc)
                {
                    gameManager.GameOver();
                    return;
                }
                if(family && player.masc)
                {
                    speechBubble.SetActive(true);
                    gameManager.IncreaseFamilyScore();
                    gameManager.familyTimeTimer = gameManager.timeSpentTimerReset;
                    personalLeoTime = personalLeoTimeReset;
                    //print(gameManager.familyTimeTimer);
                }
                if (!family && !player.masc)
                {
                    speechBubble.SetActive(true);
                    gameManager.IncreaseFriendScore();
                    gameManager.friendTimeTimer = gameManager.timeSpentTimerReset;
                    personalLeoTime = personalLeoTimeReset;
                }
                return;
            }
            else
            {
                speechBubble.SetActive(false);
            }
        }

        personalLeoTime -= Time.deltaTime;
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void ResumeMovement()
    {
        currentState = MovementState.Moving;
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        moveSpeed = defaultMoveSpeed;
        UpdateSpriteDirection();
        RandomizeInterval(); // for how long it moves
    }

    private void FacePlayer()
    {
        if (playerSpotted)
        {
            print("moving towards player!");

            // Fix: Use direction vector, not raw position
            Vector2 toPlayer = (spottedPlayerDirection - (Vector2)transform.position).normalized;
            moveDirection = toPlayer;

            UpdateSpriteDirection();
        }
    }

    private void UpdateSpriteDirection()
    {
       
        // Update the sprite based on major direction and enable the matching pointer
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            if (moveDirection.x > 0)
            {
                spriteRenderer.sprite = rightSprite;
                    
            }
            else
            {
                spriteRenderer.sprite = leftSprite;
            }
        }
        else
        {
            if (moveDirection.y > 0)
            {
                spriteRenderer.sprite = upSprite;
            }
            else
            {
                spriteRenderer.sprite = downSprite;
            }
        }
    }

    private void RandomizeInterval()
    {
        directionChangeInterval = Random.Range(minInterval, maxInterval);
        stateTimer = directionChangeInterval;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reflect moveDirection based on the surface normal
        if (collision.contacts.Length > 0)
        {
            Vector2 normal = collision.contacts[0].normal;
            moveDirection = Vector2.Reflect(moveDirection, normal).normalized;

            // Update facing for vision cone and sprite
            fieldOfView.facingDirection = moveDirection;
            UpdateSpriteDirection();
        }
    }

    private void PauseMovement()
    {
        currentState = MovementState.Paused;
        moveSpeed = 0f;
        // Pause is at least 1.5x longer than the last movement duration
        stateTimer = directionChangeInterval * Random.Range(1.5f, 2f);
    }
}