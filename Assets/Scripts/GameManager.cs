using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static ObjectManager;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int stage;
    public Animator stargeAnimator;
    public Animator clearAnimator;
    public Animator fadeAnimator;
    public Transform playerInitialPosition;

    public GameObject player;
    public Transform[] spawnPoints;
    public float nextSpawnDelay;
    public float currentSpawnDelay;
    public Text scoreText;
    public Image[] lifeImages;
    public Image[] boomImages;
    public GameObject gameOverSet;

    public ObjectManager objectManager;

    List<Spawn> spawns;
    public int spawnIndex;
    public bool spawnEnd;

    private void Awake()
    {
        spawns = new List<Spawn>();
        StageStart();
    }

    public void StageStart()
    {
        // Stage UI Load
        stargeAnimator.SetTrigger("On");
        stargeAnimator.GetComponent<Text>().text = "STAGE " + stage + "\nStart!!";
        clearAnimator.GetComponent<Text>().text = "STAGE " + stage + "\nClear!!";

        // Enemy Spawn File Read
        ReadSpawnFile();

        // Fade In
        fadeAnimator.SetTrigger("In");
    }

    public void StageEnd()
    {
        // Clear UI Load
        clearAnimator.SetTrigger("On");

        // Fade Out
        fadeAnimator.SetTrigger("Out");

        // Player Reposition 
        player.transform.position = playerInitialPosition.position;

        // Stage Increament
        stage++;
        if (stage > 2)
            Invoke("StageStart", 6);
        else
            Invoke("StageStart", 5);
    }

    void ReadSpawnFile()
    {
        // 1. 변수 초기화
        spawns.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // 2. 리스폰 파일 읽기
        TextAsset textFile = Resources.Load("stage" + stage) as TextAsset;
        Debug.Log("textFile : " + textFile);
        StringReader stringReader = new StringReader(textFile.text);
        
        while(true)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
                break;

            // 리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            string[] columns = line.Split(',');
            spawnData.delay = float.Parse(columns[0]);
            spawnData.type = (ObjectType) System.Enum.Parse(typeof(ObjectType), columns[1]);
            spawnData.point = int.Parse(columns[2]);
            spawns.Add(spawnData);
        }

        // 텍스트 파일 닫기
        stringReader.Close();

        // 첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawns[0].delay;
    }

    private void Update()
    {
        currentSpawnDelay += Time.deltaTime;

        if (currentSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            currentSpawnDelay = 0;
        }

        // ^#.UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    private void SpawnEnemy()
    {
        Spawn spawnData = spawns[spawnIndex];
        Transform spawnPoint = spawnPoints[spawnData.point];

        GameObject enemy = objectManager.GetObjectWithPosition(spawnData.type, spawnPoint.position);
        Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
        int enemySpeed;
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;
        enemyLogic.gameManager = this;
        enemySpeed = enemyLogic.enemySpeed;

        if (spawnPoint.tag == "SpawnPointRight")
        {
            enemy.transform.Rotate(Vector3.back * 90);
            enemyRigidbody.velocity = new Vector2(-enemySpeed, -1);

        }
        else if (spawnPoint.tag == "SpawnPointLeft")
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            enemyRigidbody.velocity = new Vector2(enemySpeed, -1);

        }
        else
        {
            enemyRigidbody.velocity = new Vector2(0, -enemySpeed);
        }

        // 리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex >= spawns.Count)
        {
            spawnEnd = true;
            return;
        }

        // 다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawns[spawnIndex].delay;
    }

    internal void UpdateLifeIcon(int currentLifeCount, int maxLifeCount)
    {
        // #.UI Life Init Disable
        for (int index = 0; index < maxLifeCount; index++)
        {
            lifeImages[index].color = new Color(1, 1, 1, 0);
        }

        // #.UI Life Active
        for (int index = 0; index < Mathf.Min(currentSpawnDelay, maxLifeCount); index++)
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

    public void CallExplosion(Vector3 position, ObjectType objectType)
    {
        GameObject explosion = objectManager.GetObjectWithPosition(ObjectType.EXPLOSION, position);
        Explosion explosionLogic = explosion.GetComponent<Explosion>();
        explosionLogic.StartExplosion(objectType);
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
