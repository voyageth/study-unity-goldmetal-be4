using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemySpeed;
    public int enemyHealth;
    public Sprite normalSprite;
    public Sprite onHitSprite;

    SpriteRenderer spriteRenderer;
    Rigidbody2D enemyRigidbody;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyRigidbody.velocity = Vector2.down * enemySpeed;
    }
    void OnHit(int damage)
    {
        enemyHealth -= damage;
        spriteRenderer.sprite = onHitSprite;
        CancelInvoke();
        Invoke("ReturnSprite", 0.1f);
        
        if (enemyHealth <= 0)
        {
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
