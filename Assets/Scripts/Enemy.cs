using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore = 100;
    public int enemySpeed;
    public int enemyHealth;
    public Sprite normalSprite;
    public Sprite onHitSprite;
    public float maxShotDelay = 1f;
    public int bulletSpeed = 10;
    public GameObject player;
    public GameObject bulletObjectA;
    public GameObject bulletObjectB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;

    float currentShotDelay;
    
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Fire();
        Reload();
    }
    private void CreateBullet(GameObject bulletObject, Vector3 position)
    {
        GameObject bullet = Instantiate(bulletObject, position, transform.rotation);
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
            CreateBullet(bulletObjectA, transform.position);
        }
        else if (enemyName == "M")
        {
            CreateBullet(bulletObjectA, transform.position + Vector3.right * 0.3f);
            CreateBullet(bulletObjectA, transform.position + Vector3.left * 0.3f);
        }
        else if (enemyName == "L")
        {
            CreateBullet(bulletObjectA, transform.position + Vector3.right * 0.3f);
            CreateBullet(bulletObjectB, transform.position);
            CreateBullet(bulletObjectA, transform.position + Vector3.left * 0.3f);
        }

        currentShotDelay = 0;
    }

    private void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        if (enemyHealth <= 0)
            return;

        enemyHealth -= damage;
        spriteRenderer.sprite = onHitSprite;
        CancelInvoke();
        Invoke("ReturnSprite", 0.1f);
        
        if (enemyHealth <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // Random Ratio Item Drop
            int randomNumber = Random.Range(0, 10);
            if (randomNumber < 3)
            {
                // Coin
                Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }
            else if (randomNumber < 4)
            {
                // Power
                Instantiate(itemPower, transform.position, itemPower.transform.rotation);
            }
            else if (randomNumber < 5)
            {
                // Boom
                Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
            }
            else 
            {
                Debug.Log("No Item");
            }

            // Àû ÆÄ±«
            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = normalSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            
            Destroy(collision.gameObject);
        }   
    }
}
