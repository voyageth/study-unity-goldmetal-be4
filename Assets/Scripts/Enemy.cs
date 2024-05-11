using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
using static ObjectManager;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore = 100;
    public int enemySpeed;
    public int enemyMaxHealth = 10;
    public int enemyCurrentHealth;
    public Sprite normalSprite;
    public Sprite onHitSprite;
    public float maxShotDelay = 1f;
    public int bulletSpeed = 10;

    public GameObject player;
    public ObjectManager objectManager;

    float currentShotDelay;
    
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        enemyCurrentHealth = enemyMaxHealth;
    }

    void Update()
    {
        Fire();
        Reload();
    }
    
    private void CreateBullet(PrefabType bulletGameType, Vector3 position)
    {
        GameObject bullet = objectManager.GetObject(bulletGameType, position, transform.rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        Vector3 directionVector = (player.transform.position - position).normalized;
        bulletRigidbody.AddForce(directionVector * bulletSpeed, ForceMode2D.Impulse);
    }

    private void Fire()
    {
        if (currentShotDelay < maxShotDelay)
            return;
        
        if (enemyName == "S")
        {
            CreateBullet(PrefabType.ENEMY_BULLET_A, transform.position);
        }
        else if (enemyName == "M")
        {
            CreateBullet(PrefabType.ENEMY_BULLET_A, transform.position + Vector3.right * 0.3f);
            CreateBullet(PrefabType.ENEMY_BULLET_A, transform.position + Vector3.left * 0.3f);
        }
        else if (enemyName == "L")
        {
            CreateBullet(PrefabType.ENEMY_BULLET_A, transform.position + Vector3.right * 0.3f);
            CreateBullet(PrefabType.ENEMY_BULLET_B, transform.position);
            CreateBullet(PrefabType.ENEMY_BULLET_A, transform.position + Vector3.left * 0.3f);
        }

        currentShotDelay = 0;
    }

    private void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        if (enemyCurrentHealth <= 0)
            return;

        enemyCurrentHealth -= damage;
        spriteRenderer.sprite = onHitSprite;
        CancelInvoke();
        Invoke("ReturnSprite", 0.1f);
        
        if (enemyCurrentHealth <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // Random Ratio Item Drop
            int randomNumber = Random.Range(0, 10);
            if (randomNumber < 3)
            {
                // Coin
                objectManager.GetObject(PrefabType.ITEM_COIN, transform.position, Quaternion.identity);
            }
            else if (randomNumber < 4)
            {
                // Power
                objectManager.GetObject(PrefabType.ITEM_POWER, transform.position, Quaternion.identity);
            }
            else if (randomNumber < 5)
            {
                // Boom
                objectManager.GetObject(PrefabType.ITEM_BOOM, transform.position, Quaternion.identity);
            }
            else 
            {
                Debug.Log("No Item");
            }

            // Àû ÆÄ±«
            gameObject.SetActive(false);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = normalSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
            gameObject.SetActive(false);
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            collision.gameObject.SetActive(false);
        }   
    }
}
