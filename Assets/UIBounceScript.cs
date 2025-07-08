using UnityEngine;

public class UIBounce : MonoBehaviour
{
    [SerializeField] private float bounceSpeed = 2f;
    [SerializeField] private float bounceHeight = 10f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = initialPosition + new Vector3(0f, offset, 0f);
    }
}