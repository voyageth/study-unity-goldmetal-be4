using System;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public enum ObjectType
    {
        PLAYER,
        ENEMY_SMALL, 
        ENEMY_MEDIUM,
        ENEMY_LARGE,
        ENEMY_BOSS,
        ITEM_COIN, 
        ITEM_POWER, 
        ITEM_BOOM, 
        PLAYER_BULLET_A, 
        PLAYER_BULLET_B, 
        ENEMY_BULLET_A,
        ENEMY_BULLET_B,
        ENEMY_BULLET_C,
        ENEMY_BULLET_D,
        FOLLOWER_BULLET,
        EXPLOSION,
    }

    public GameObject enemySmallPrefab;
    public GameObject enemyMediumPrefab;
    public GameObject enemyLargePrefab;
    public GameObject enemyBossPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject playerBulletObjectAPrefab;
    public GameObject playerBulletObjectBPrefab;
    public GameObject enemyBulletObjectAPrefab;
    public GameObject enemyBulletObjectBPrefab;
    public GameObject enemyBulletObjectCPrefab;
    public GameObject enemyBulletObjectDPrefab;
    public GameObject followerBulletObjectAPrefab;
    public GameObject explosionPrefab;

    GameObject[] enemySmallPool;
    GameObject[] enemyMediumPool;
    GameObject[] enemyLargePool;
    GameObject[] enemyBossPool;
    GameObject[] itemCoinPool;
    GameObject[] itemPowerPool;
    GameObject[] itemBoomPool;
    GameObject[] playerBulletAPool;
    GameObject[] playerBulletBPool;
    GameObject[] enemyBulletAPool;
    GameObject[] enemyBulletBPool;
    GameObject[] enemyBulletCPool;
    GameObject[] enemyBulletDPool;
    GameObject[] followerBulletAPool;
    GameObject[] explosionPool;

    private void Awake()
    {
        InitializePool();
    }

    GameObject GetPrefab(ObjectType gameObjectType)
    {
        switch (gameObjectType)
        {
            case ObjectType.ENEMY_SMALL:
                return enemySmallPrefab;
            case ObjectType.ENEMY_MEDIUM:
                return enemyMediumPrefab;
            case ObjectType.ENEMY_LARGE:
                return enemyLargePrefab;
            case ObjectType.ENEMY_BOSS:
                return enemyBossPrefab;
            case ObjectType.ITEM_COIN:
                return itemCoinPrefab;
            case ObjectType.ITEM_POWER:
                return itemPowerPrefab;
            case ObjectType.ITEM_BOOM:
                return itemBoomPrefab;
            case ObjectType.PLAYER_BULLET_A:
                return playerBulletObjectAPrefab;
            case ObjectType.PLAYER_BULLET_B:
                return playerBulletObjectBPrefab;
            case ObjectType.ENEMY_BULLET_A:
                return enemyBulletObjectAPrefab;
            case ObjectType.ENEMY_BULLET_B:
                return enemyBulletObjectBPrefab;
            case ObjectType.ENEMY_BULLET_C:
                return enemyBulletObjectCPrefab;
            case ObjectType.ENEMY_BULLET_D:
                return enemyBulletObjectDPrefab;
            case ObjectType.FOLLOWER_BULLET:
                return followerBulletObjectAPrefab;
            case ObjectType.EXPLOSION:
                return explosionPrefab;
        }

        throw new Exception("No Prefab for " + gameObjectType);
    }

    GameObject[] GetPool(ObjectType gameObjectType)
    {
        switch (gameObjectType)
        {
            case ObjectType.ENEMY_SMALL:
                return enemySmallPool;
            case ObjectType.ENEMY_MEDIUM:
                return enemyMediumPool;
            case ObjectType.ENEMY_LARGE:
                return enemyLargePool;
            case ObjectType.ENEMY_BOSS:
                return enemyBossPool;
            case ObjectType.ITEM_COIN:
                return itemCoinPool;
            case ObjectType.ITEM_POWER:
                return itemPowerPool;
            case ObjectType.ITEM_BOOM:
                return itemBoomPool;
            case ObjectType.PLAYER_BULLET_A:
                return playerBulletAPool;
            case ObjectType.PLAYER_BULLET_B:
                return playerBulletBPool;
            case ObjectType.ENEMY_BULLET_A:
                return enemyBulletAPool;
            case ObjectType.ENEMY_BULLET_B:
                return enemyBulletBPool;
            case ObjectType.ENEMY_BULLET_C:
                return enemyBulletCPool;
            case ObjectType.ENEMY_BULLET_D:
                return enemyBulletDPool;
            case ObjectType.FOLLOWER_BULLET:
                return followerBulletAPool;
            case ObjectType.EXPLOSION:
                return explosionPool;
        }

        throw new Exception("No ObjectPool for " + gameObjectType);
    }

    private void InitializePool()
    {
        enemySmallPool = new GameObject[20];
        enemyMediumPool = new GameObject[10];
        enemyLargePool = new GameObject[10];
        enemyBossPool = new GameObject[3];
        itemCoinPool = new GameObject[20];
        itemPowerPool = new GameObject[10];
        itemBoomPool = new GameObject[10];
        playerBulletAPool = new GameObject[100];
        playerBulletBPool = new GameObject[100];
        enemyBulletAPool = new GameObject[100];
        enemyBulletBPool = new GameObject[100];
        enemyBulletCPool = new GameObject[1000];
        enemyBulletDPool = new GameObject[50];
        followerBulletAPool = new GameObject[100];
        explosionPool = new GameObject[100];

        foreach (ObjectType gameObjectType in Enum.GetValues(typeof(ObjectType)))
        {
            if (ObjectType.PLAYER == gameObjectType)
                continue;
            
            InstantiateData(GetPool(gameObjectType), GetPrefab(gameObjectType));
        }
    }

    private void InstantiateData(GameObject[] pool, GameObject prefab)
    {
        for (int index = 0; index < pool.Length; index++)
        {
            pool[index] = Instantiate(prefab);
            pool[index].SetActive(false);
        }
    }

    public GameObject GetObjectWithPosition(ObjectType gameObjectType, Vector3 position)
    {
        GameObject[] pool = GetPool(gameObjectType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (!pool[index].activeSelf)
            {
                pool[index].SetActive(true);
                pool[index].transform.position = position;
                pool[index].transform.rotation = Quaternion.identity;
                return pool[index];
            }
        }

        return null;
    }

    public void BoomDamageToAllEnemies()
    {
        BoomDamangeToEnemyByPrefabType(ObjectType.ENEMY_SMALL);
        BoomDamangeToEnemyByPrefabType(ObjectType.ENEMY_MEDIUM);
        BoomDamangeToEnemyByPrefabType(ObjectType.ENEMY_LARGE);
        BoomDamangeToEnemyByPrefabType(ObjectType.ENEMY_BOSS);
    }

    private void BoomDamangeToEnemyByPrefabType(ObjectType prefabType)
    {
        GameObject[] pool = GetPool(prefabType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (pool[index].activeSelf)
            {
                Enemy enemy = pool[index].GetComponent<Enemy>();
                enemy.OnHit(1000);
            }
        }
    }

    public void DestroyAllEnemyBullets()
    {
        DestroyEnemyBulletsByPrefabType(ObjectType.ENEMY_BULLET_A);
        DestroyEnemyBulletsByPrefabType(ObjectType.ENEMY_BULLET_B);
        DestroyEnemyBulletsByPrefabType(ObjectType.ENEMY_BULLET_C);
        DestroyEnemyBulletsByPrefabType(ObjectType.ENEMY_BULLET_D);
    }

    private void DestroyEnemyBulletsByPrefabType(ObjectType prefabType)
    {
        GameObject[] pool = GetPool(prefabType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (pool[index].activeSelf)
                pool[index].SetActive(false);
        }
    }
}
