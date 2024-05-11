using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const int MAX_BULLET_POWER = 3;
    const int MAX_LIFE_COUNT = 3;
    const int MAX_BOOM_COUNT = 3;

    public int life = 3;
    public int score = 0;
    public bool isHit;
    public bool isBoomTime;
    public int playerSpeed = 3;
    public int bulletSpeed = 10;
    public int bulletPower = 1;
    public int boomCount = 3;
    public float maxShotDelay = 0.2f;
    public GameManager gameManager;
    public GameObject bulletObjectA;
    public GameObject bulletObjectB;
    public GameObject boomEffect;

    bool isTouchTop;
    bool isTouchBottom;
    bool isTouchRight;
    bool isTouchLeft;
    float currentShotDelay;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
    }

    private void Boom()
    {
        if (!Input.GetButton("Fire2"))
            return;

        if (isBoomTime)
            return;

        if (boomCount <= 0)
            return;

        boomCount--;
        gameManager.UpdateBoomIcon(boomCount, MAX_BOOM_COUNT);

        // 1. boom effect
        OnBoomEffect();
        Invoke("OffBoomEffect", 4);

        // 2. remove enemy
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        for (int index = 0; index < enemyGameObjects.Length; index++)
        {
            Enemy enemy = enemyGameObjects[index].GetComponent<Enemy>();
            enemy.OnHit(9999);
        }

        // 3. remove enemy bullet
        GameObject[] enemyBulletGameObjects = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int index = 0; index < enemyBulletGameObjects.Length; index++)
        {
            Destroy(enemyBulletGameObjects[index]);
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;

        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;

        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = new Vector3(h, v, 0) * playerSpeed * Time.deltaTime;

        transform.position = currentPosition + nextPosition;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
            animator.SetInteger("HorizontalInput", (int)h);
    }

    private void CreateBullet(GameObject bulletObject, Vector3 position)
    {
        GameObject bullet = Instantiate(bulletObject, position, transform.rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (currentShotDelay < maxShotDelay)
            return;
        
        switch(bulletPower)
        {
            case 1:
                CreateBullet(bulletObjectA, transform.position);
                break;
            case 2:
                CreateBullet(bulletObjectA, transform.position + Vector3.right * 0.1f);
                CreateBullet(bulletObjectA, transform.position + Vector3.left * 0.1f);
                break;
            case 3:
                CreateBullet(bulletObjectA, transform.position + Vector3.right * 0.35f);
                CreateBullet(bulletObjectB, transform.position);
                CreateBullet(bulletObjectA, transform.position + Vector3.left * 0.35f);
                break;
        }
        currentShotDelay = 0;
    }

    private void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isHit)
            {
                return;
            }
            isHit = true;

            life--;
            gameManager.UpdateLifeIcon(life, MAX_LIFE_COUNT);

            if (life <= 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch(item.type)
            {
                case Item.ItemType.COIN:
                    score += 1000;
                    break;
                case Item.ItemType.POWER:
                    if (bulletPower < MAX_BULLET_POWER)
                        bulletPower++;
                    else
                        score += 500;
                    break;
                case Item.ItemType.BOOM:
                    if (boomCount < MAX_BOOM_COUNT)
                    {
                        boomCount++;
                        gameManager.UpdateBoomIcon(boomCount, MAX_BOOM_COUNT);
                    }
                    else
                        score += 500;
                    break;
            }

            Destroy(collision.gameObject);
        }
    }

    void OnBoomEffect()
    {
        boomEffect.SetActive(true);
        isBoomTime = true;
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
