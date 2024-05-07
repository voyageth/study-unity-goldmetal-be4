using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjects;
    public Transform[] spawnPoints;
    public float maxSpawnDelay;
    public float currentSpawnDelay;

    private void Update()
    {
        currentSpawnDelay += Time.deltaTime;

        if (currentSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            currentSpawnDelay = 0;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = enemyObjects[Random.Range(0, enemyObjects.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
