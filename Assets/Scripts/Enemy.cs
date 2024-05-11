using UnityEngine;
using static ObjectManager;

abstract public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore = 100;
    public int enemySpeed;
    public int enemyMaxHealth = 10;
    public int enemyCurrentHealth;

    public GameObject player;
    public ObjectManager objectManager;

    public void OnEnable()
    {
        enemyCurrentHealth = enemyMaxHealth;
    }

    public void OnHit(int damage)
    {
        if (enemyCurrentHealth <= 0)
            return;

        enemyCurrentHealth -= damage;

        // 피격 효과
        HitEffect();

        if (enemyCurrentHealth <= 0)
        {
            // 플레이어 점수 갱신
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // 파괴 전 필요 로직
            BeforeDeActive();

            // 적 파괴
            gameObject.SetActive(false);
        }
    }

    abstract protected void HitEffect();

    abstract protected void BeforeDeActive();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            collision.gameObject.SetActive(false);
        }
    }

    protected void FireBulletToPlayer(PrefabType bulletGameType, int bulletSpeed, Vector3 startPosition)
    {
        FireBulletToPosition(bulletGameType, bulletSpeed, startPosition, player.transform.position);
    }

    protected void FireBulletToPosition(PrefabType bulletGameType, int bulletSpeed, Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 directionVector = (endPosition - startPosition).normalized;
        FireBulletToDirection(bulletGameType, bulletSpeed, startPosition, directionVector);
    }

    protected void FireBulletToDirection(PrefabType bulletGameType, int bulletSpeed, Vector3 startPosition, Vector3 directionVector)
    {
        GameObject bullet = objectManager.GetObject(bulletGameType, startPosition);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(directionVector * bulletSpeed, ForceMode2D.Impulse);

        // 해당 벡터의 각도를 계산 (라디안 to 도)
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg + 90;
        // 오브젝트를 z 축 기준으로 회전. X와 Y는 회전시키지 않음
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
