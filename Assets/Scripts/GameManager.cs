using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemyObjects;
    public Transform[] spawnPoints;
    public float maxSpawnDelay;
    public float currentSpawnDelay;
    public Text scoreText;
    public Image[] lifeImages;
    public Image[] boomImages;
    public GameObject gameOverSet;

    private void Update()
    {
        currentSpawnDelay += Time.deltaTime;

        if (currentSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            currentSpawnDelay = 0;
        }

        // ^#.UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = enemyObjects[Random.Range(0, enemyObjects.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if (spawnPoint.tag == "SpawnPointRight")
        {
            enemy.transform.Rotate(Vector3.back * 90);
            enemyRigidbody.velocity = new Vector2(-enemyLogic.enemySpeed, -1);

        }
        else if (spawnPoint.tag == "SpawnPointLeft")
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            enemyRigidbody.velocity = new Vector2(enemyLogic.enemySpeed, -1);

        }
        else
        {
            enemyRigidbody.velocity = new Vector2(0, -enemyLogic.enemySpeed);

        }
    }

    internal void UpdateLifeIcon(int currentLifeCount, int maxLifeCount)
    {
        // #.UI Life Init Disable
        for (int index = 0; index < maxLifeCount; index++)
        {
            lifeImages[index].color = new Color(1, 1, 1, 0);
        }

        // #.UI Life Active
        for (int index = 0; index < currentLifeCount; index++)
        {
            lifeImages[index].color = new Color(1, 1, 1, 1);
        }
    }

    internal void UpdateBoomIcon(int currentBoomCount, int maxBoomCount)
    {
        // #.UI Boom Init Disable
        for (int index = 0; index < maxBoomCount; index++)
        {
            boomImages[index].color = new Color(1, 1, 1, 0);
        }

        // #.UI Boom Active
        for (int index = 0; index < currentBoomCount; index++)
        {
            boomImages[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2);
    }

    private void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);
        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
