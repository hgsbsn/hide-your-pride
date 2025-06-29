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
    public Sprite baseLenora;
    public Sprite mascLenora;
    public bool masc = false;
    public bool inTransition = false;
    [SerializeField] private ParticleSystem dangerEffect; // Assign in Inspector
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

        transitionAction.Enable(); // <- This was missing
        transitionAction.performed += OnTransition;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        moveAction.Disable();

        transitionAction.performed -= OnTransition;
        transitionAction.Disable(); // <- Don't forget to disable it too
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnTransition(InputAction.CallbackContext context)
    {
        if (!inTransition)
            StartCoroutine(HandleTransition());
    }

    private IEnumerator HandleTransition()
    {
        inTransition = true;

        // Optional: Set a danger state here (e.g., animation, color change, etc.)
        // Example: sprite.color = Color.red;

        // Spawn particle effect
        if (dangerEffect != null)
            dangerEffect.Play();
        moveSpeed = moveSpeedWhileTransitioning;

        yield return new WaitForSeconds(transitionTime);

        // Flip the bool
        masc = !masc;
        sprite.sprite = masc ? mascLenora : baseLenora;
        if (dangerEffect != null)
            dangerEffect.Stop();
        moveSpeed = defaultMoveSpeed;

        // Optional: Clear danger state
        // Example: sprite.color = Color.white;

        inTransition = false;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}