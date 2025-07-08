using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float defaultMoveSpeed = 5f;
    private float moveSpeed = 5f;
    [SerializeField] private float moveSpeedWhileTransitioning = 1f;

    public InputAction moveAction;
    public InputAction transitionAction;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public SpriteRenderer sprite;

    [Header("Base Sprites")]
    public Sprite baseLenora;
    public Sprite mascLenora;

    [Header("Directional Sprites - Base")]
    public Sprite baseUp;
    public Sprite baseDown;
    public Sprite baseLeft;
    public Sprite baseRight;

    [Header("Directional Sprites - Masc")]
    public Sprite mascUp;
    public Sprite mascDown;
    public Sprite mascLeft;
    public Sprite mascRight;

    public bool masc = false;
    public bool inTransition = false;
    [SerializeField] private ParticleSystem dangerEffect;
    [SerializeField] private float transitionTime = 4f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = defaultMoveSpeed;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        transitionAction.Enable();
        transitionAction.performed += OnTransition;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        moveAction.Disable();

        transitionAction.performed -= OnTransition;
        transitionAction.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        UpdateDirectionSprite();
    }

    private void OnTransition(InputAction.CallbackContext context)
    {
        if (!inTransition)
            StartCoroutine(HandleTransition());
    }

    private IEnumerator HandleTransition()
    {
        inTransition = true;

        if (dangerEffect != null)
            dangerEffect.Play();

        moveSpeed = moveSpeedWhileTransitioning;

        yield return new WaitForSeconds(transitionTime);

        masc = !masc;

        UpdateDirectionSprite(); // Reapply correct direction with new style

        if (dangerEffect != null)
            dangerEffect.Stop();

        moveSpeed = defaultMoveSpeed;
        inTransition = false;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void UpdateDirectionSprite()
    {
        if (moveInput == Vector2.zero)
        {
            sprite.sprite = masc ? mascLenora : baseLenora;
            return;
        }

        Sprite newSprite = null;

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            newSprite = moveInput.x > 0 ? (masc ? mascRight : baseRight) : (masc ? mascLeft : baseLeft);
        }
        else
        {
            newSprite = moveInput.y > 0 ? (masc ? mascUp : baseUp) : (masc ? mascDown : baseDown);
        }

        if (newSprite != null)
            sprite.sprite = newSprite;
    }
}