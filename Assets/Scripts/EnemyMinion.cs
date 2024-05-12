using UnityEngine;
using static ObjectManager;

public class EnemyMinion : Enemy
{
    public Sprite normalSprite;
    public Sprite onHitSprite;
    public float maxShotDelay = 1f;
    public int bulletSpeed = 4;

    float currentShotDelay;
    
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private new void OnEnable()
    {
        base.OnEnable();
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.tag == "BorderBullet")
            gameObject.SetActive(false);
    }

    protected override void HitEffect()
    {
        spriteRenderer.sprite = onHitSprite;
        CancelInvoke();
        Invoke("ReturnSprite", 0.1f);
    }

    protected override void BeforeDeActive()
    {
        CreateItem();
    }

    protected override void AfterDeActive()
    {
    }

    private void CreateItem()
    {
        // Random Ratio Item Drop
        int randomNumber = Random.Range(0, 10);
        if (randomNumber < 3)
        {
            // Coin
            objectManager.GetObjectWithPosition(ObjectType.ITEM_COIN, transform.position);
        }
        else if (randomNumber < 4)
        {
            // Power
            objectManager.GetObjectWithPosition(ObjectType.ITEM_POWER, transform.position);
        }
        else if (randomNumber < 5)
        {
            // Boom
            objectManager.GetObjectWithPosition(ObjectType.ITEM_BOOM, transform.position);
        }
        else
        {
            Debug.Log("No Item");
        }
    }

    void Update()
    {
        Fire();
        Reload();
    }

    private void Fire()
    {
        if (currentShotDelay < maxShotDelay)
            return;
        
        if (enemyObjectType == ObjectType.ENEMY_SMALL)
        {
            FireBulletToPlayer(ObjectType.ENEMY_BULLET_A, bulletSpeed, transform.position);
        }
        else if (enemyObjectType == ObjectType.ENEMY_MEDIUM)
        {
            FireBulletToPlayer(ObjectType.ENEMY_BULLET_A, bulletSpeed, transform.position + Vector3.right * 0.3f);
            FireBulletToPlayer(ObjectType.ENEMY_BULLET_A, bulletSpeed, transform.position + Vector3.left * 0.3f);
        }
        else if (enemyObjectType == ObjectType.ENEMY_LARGE)
        {
            FireBulletToPlayer(ObjectType.ENEMY_BULLET_A, bulletSpeed, transform.position + Vector3.right * 0.3f);
            FireBulletToPlayer(ObjectType.ENEMY_BULLET_B, bulletSpeed, transform.position);
            FireBulletToPlayer(ObjectType.ENEMY_BULLET_A, bulletSpeed, transform.position + Vector3.left * 0.3f);
        }

        currentShotDelay = 0;
    }

    private void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = normalSprite;
    }
}
