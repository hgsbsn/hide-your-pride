using UnityEngine;
using UnityEngine.InputSystem;

public class MouseParallaxInputSystem : MonoBehaviour
{
    [SerializeField] private float parallaxStrength = 10f;
    [SerializeField] private bool smoothMotion = true;
    [SerializeField] private float smoothSpeed = 5f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (Mouse.current == null) return; // Safety check for platforms without mouse

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 normalizedOffset = (mousePos - screenCenter) / screenCenter;

        Vector3 offset = new Vector3(normalizedOffset.x, normalizedOffset.y, 0f) * parallaxStrength;
        targetPosition = initialPosition + offset;

        if (smoothMotion)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * smoothSpeed);
        }
        else
        {
            transform.localPosition = targetPosition;
        }
    }
}