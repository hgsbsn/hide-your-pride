using UnityEngine;

public class DustSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dustPrefab;
    [SerializeField] private Transform dustSpawnPoint;
    [SerializeField] private float spawnInterval = 0.4f;
    [SerializeField] private float movementThreshold = 0.1f;
    [SerializeField] private float fastSpawn = 0.4f;
    [SerializeField] private float slowSpawn = 0.8f;

    private Rigidbody2D rb;
    private float timer;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        bool isMoving = rb.linearVelocity.magnitude > movementThreshold;
        timer += Time.deltaTime;

        if (isMoving && timer >= spawnInterval)
        {
            SpawnDust();
            RandomSpawn();
            timer = 0f;
        }
    }

    private void SpawnDust()
    {
        if (dustPrefab != null && dustSpawnPoint != null)
        {
            Instantiate(dustPrefab, dustSpawnPoint.position, Quaternion.identity);
        }
    }

    private void RandomSpawn()
    {
        spawnInterval = Random.Range(fastSpawn,slowSpawn);
    }
}