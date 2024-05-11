using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public enum PrefabType
    {
        ENEMY_SMALL, 
        ENEMY_MEDIUM, 
        ENEMY_LARGE, 
        ITEM_COIN, 
        ITEM_POWER, 
        ITEM_BOOM, 
        PLAYER_BULLET_A, 
        PLAYER_BULLET_B, 
        ENEMY_BULLET_A, 
        ENEMY_BULLET_B,
    }

    public GameObject enemySmallPrefab;
    public GameObject enemyMediumPrefab;
    public GameObject enemyLargePrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject playerBulletObjectAPrefab;
    public GameObject playerBulletObjectBPrefab;
    public GameObject enemyBulletObjectAPrefab;
    public GameObject enemyBulletObjectBPrefab;

    GameObject[] enemySmallPool;
    GameObject[] enemyMediumPool;
    GameObject[] enemyLargePool;
    GameObject[] itemCoinPool;
    GameObject[] itemPowerPool;
    GameObject[] itemBoomPool;
    GameObject[] playerBulletAPool;
    GameObject[] playerBulletBPool;
    GameObject[] enemyBulletAPool;
    GameObject[] enemyBulletBPool;

    private void Awake()
    {
        InitializePool();
    }

    GameObject GetPrefab(PrefabType gameObjectType)
    {
        switch (gameObjectType)
        {
            case PrefabType.ENEMY_SMALL:
                return enemySmallPrefab;
            case PrefabType.ENEMY_MEDIUM:
                return enemyMediumPrefab;
            case PrefabType.ENEMY_LARGE:
                return enemyLargePrefab;
            case PrefabType.ITEM_COIN:
                return itemCoinPrefab;
            case PrefabType.ITEM_POWER:
                return itemPowerPrefab;
            case PrefabType.ITEM_BOOM:
                return itemBoomPrefab;
            case PrefabType.PLAYER_BULLET_A:
                return playerBulletObjectAPrefab;
            case PrefabType.PLAYER_BULLET_B:
                return playerBulletObjectBPrefab;
            case PrefabType.ENEMY_BULLET_A:
                return enemyBulletObjectAPrefab;
            case PrefabType.ENEMY_BULLET_B:
                return enemyBulletObjectBPrefab;
        }

        throw new Exception("No Prefab for " + gameObjectType);
    }

    GameObject[] GetPool(PrefabType gameObjectType)
    {
        switch (gameObjectType)
        {
            case PrefabType.ENEMY_SMALL:
                return enemySmallPool;
            case PrefabType.ENEMY_MEDIUM:
                return enemyMediumPool;
            case PrefabType.ENEMY_LARGE:
                return enemyLargePool;
            case PrefabType.ITEM_COIN:
                return itemCoinPool;
            case PrefabType.ITEM_POWER:
                return itemPowerPool;
            case PrefabType.ITEM_BOOM:
                return itemBoomPool;
            case PrefabType.PLAYER_BULLET_A:
                return playerBulletAPool;
            case PrefabType.PLAYER_BULLET_B:
                return playerBulletBPool;
            case PrefabType.ENEMY_BULLET_A:
                return enemyBulletAPool;
            case PrefabType.ENEMY_BULLET_B:
                return enemyBulletBPool;
        }

        throw new Exception("No ObjectPool for " + gameObjectType);
    }

    private void InitializePool()
    {
        enemySmallPool = new GameObject[20];
        enemyMediumPool = new GameObject[10];
        enemyLargePool = new GameObject[10];
        itemCoinPool = new GameObject[20];
        itemPowerPool = new GameObject[10];
        itemBoomPool = new GameObject[10];
        playerBulletAPool = new GameObject[100];
        playerBulletBPool = new GameObject[100];
        enemyBulletAPool = new GameObject[100];
        enemyBulletBPool = new GameObject[100];

        foreach (PrefabType gameObjectType in Enum.GetValues(typeof(PrefabType)))
        {
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

    public GameObject GetObject(PrefabType gameObjectType, Vector3 position, Quaternion rotation)
    {
        GameObject[] pool = GetPool(gameObjectType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (!pool[index].activeSelf)
            {
                pool[index].SetActive(true);
                pool[index].transform.position = position;
                pool[index].transform.rotation = rotation;
                return pool[index];
            }
        }

        return null;
    }

    public void DestroyAllEnemies()
    {
        DestroyEnemiesByPrefabType(PrefabType.ENEMY_SMALL);
        DestroyEnemiesByPrefabType(PrefabType.ENEMY_MEDIUM);
        DestroyEnemiesByPrefabType(PrefabType.ENEMY_LARGE);
    }

    private void DestroyEnemiesByPrefabType(PrefabType prefabType)
    {
        GameObject[] pool = GetPool(prefabType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (pool[index].activeSelf)
            {
                Enemy enemy = pool[index].GetComponent<Enemy>();
                enemy.OnHit(9999);
            }
        }
    }

    public void DestroyAllEnemyBullets()
    {
        DestroyEnemyBulletsByPrefabType(PrefabType.ENEMY_BULLET_A);
        DestroyEnemyBulletsByPrefabType(PrefabType.ENEMY_BULLET_B);
    }

    private void DestroyEnemyBulletsByPrefabType(PrefabType prefabType)
    {
        GameObject[] pool = GetPool(prefabType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (pool[index].activeSelf)
                pool[index].SetActive(false);
        }
    }
}
