using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 24;
    public Transform player;
    public float spawnDistance = 20f;
    public float horizontalAngle = 30f;
    public float verticalAngle = 15f;

    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        float distance = Random.Range(spawnDistance * 0.5f, spawnDistance);
        float yaw = Random.Range(-horizontalAngle, horizontalAngle);
        float pitch = Random.Range(-verticalAngle, verticalAngle);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rot * player.forward;
        Vector3 spawnPos = player.position + direction * distance;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
