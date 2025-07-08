using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] private Transform target;       // The character to follow
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // Keep camera behind (z = -10 for 2D)
    [SerializeField] private float smoothSpeed = 5f;

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}