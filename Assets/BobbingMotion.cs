using UnityEngine;

public class BobbingMotion : MonoBehaviour
{
    [SerializeField] private float bobSpeed = 4f;
    [SerializeField] private float bobAmount = 0.1f;
    [SerializeField] private bool bobOnlyWhenMoving = true;

    private Vector3 initialLocalPosition;
    private Rigidbody2D parentRb;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        parentRb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        bool isMoving = !bobOnlyWhenMoving || (parentRb != null && parentRb.linearVelocity.magnitude > 0.1f);

        float offset = isMoving ? Mathf.Sin(Time.time * bobSpeed) * bobAmount : 0f;
        transform.localPosition = initialLocalPosition + new Vector3(0, offset, 0);
    }
}