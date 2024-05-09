using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int life = 3;
    public int score = 0;
    public bool isHit;
    public int playerSpeed = 3;
    public int bulletSpeed = 10;
    public int bulletPower = 1;
    public float maxShotDelay = 0.2f;
    public GameObject bulletObjectA;
    public GameObject bulletObjectB;
    public GameManager gameManager;

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
            gameManager.UpdateLifeIcon(life);

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
