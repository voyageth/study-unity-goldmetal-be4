using System;
using Unity.VisualScripting;
using UnityEngine;
using static ObjectManager;
using Random = UnityEngine.Random;

public class EnemyBoss : Enemy
{
    Animator animator;

    int patternIndex;
    int currentPatternCount;
    int[] maxPatternCount;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        maxPatternCount = new int[] { 2, 3, 99, 10};
    }

    private new void OnEnable()
    {
        base.OnEnable();
        Invoke("Stop", 2);
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        // boss 전용 처리?
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            collision.gameObject.SetActive(false);
        }
    }

    protected override void HitEffect()
    {
        animator.SetTrigger("OnHit");
    }

    protected override void BeforeDeActive()
    {
    }

    private void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = Vector3.zero;

        Invoke("Think", 2);
    }

    private void Think()
    {
        if (enemyCurrentHealth <= 0)
            return;

        Debug.Log("패턴 선택");

        //patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        patternIndex = 3;
        currentPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireForward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireForward()
    {
        if (enemyCurrentHealth <= 0)
            return;

        Debug.Log("앞으로 4발 발사");
        FireBulletToDirection(PrefabType.ENEMY_BULLET_D, 8, transform.position + Vector3.right * 0.4f, Vector2.down);
        FireBulletToDirection(PrefabType.ENEMY_BULLET_D, 8, transform.position + Vector3.right * 0.8f, Vector2.down);
        FireBulletToDirection(PrefabType.ENEMY_BULLET_D, 8, transform.position + Vector3.left * 0.4f, Vector2.down);
        FireBulletToDirection(PrefabType.ENEMY_BULLET_D, 8, transform.position + Vector3.left * 0.8f, Vector2.down);

        currentPatternCount++;

        if (currentPatternCount < maxPatternCount[patternIndex])
            Invoke("FireForward", 2);
        else
            Invoke("Think", 3);
    }

    void FireShot()
    {
        if (enemyCurrentHealth <= 0)
            return;
        
        Debug.Log("플레이어 방향으로 샷건");
        for (int index = 0; index < 5; index++)
        {
            Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(0f, 2f));
            FireBulletToPosition(PrefabType.ENEMY_BULLET_C, 3, transform.position, randomVector);
        }

        currentPatternCount++;

        if (currentPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 3.5f);
        else
            Invoke("Think", 3);
    }

    void FireArc()
    {
        if (enemyCurrentHealth <= 0)
            return;

        Debug.Log("부채 모양으로 발사");
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.PI * 10 * currentPatternCount / maxPatternCount[patternIndex]), -1);
        FireBulletToDirection(PrefabType.ENEMY_BULLET_C, 5, transform.position, directionVector);
        
        currentPatternCount++;

        if (currentPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }

    void FireAround()
    {
        if (enemyCurrentHealth <= 0)
            return;

        Debug.Log("원 형태로 전체 공격");

        int roundNumber = Random.Range(40, 50);
        for (int index = 0; index < roundNumber; index++)
        {
            Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.PI * 2 * index / roundNumber), Mathf.Sin(Mathf.PI * 2 * index / roundNumber));
            //Vector3 rotationVector = Vector3.forward * 360 * index / roundNumber + Vector3.forward * 90;
            //FireBulletToDirectionWithRotation(PrefabType.ENEMY_BULLET_C, 5, transform.position, directionVector, rotationVector);
            FireBulletToDirection(PrefabType.ENEMY_BULLET_C, 5, transform.position, directionVector);
        }

        currentPatternCount++;

        if (currentPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.7f);
        else
            Invoke("Think", 3);
    }
}
