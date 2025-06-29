using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public InputAction moveAction;
    public InputAction transitionAction;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public SpriteRenderer sprite;
    public Sprite baseLenora;
    public Sprite mascLenora;
    public bool masc = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        masc = !masc; // Flip the bool

        sprite.sprite = masc ? mascLenora : baseLenora;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}