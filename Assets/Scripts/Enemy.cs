using UnityEngine;
using static ObjectManager;

abstract public class Enemy : MonoBehaviour
{
    public ObjectType enemyObjectType;
    public int enemyScore = 100;
    public int enemySpeed;
    public int enemyMaxHealth = 10;
    public int enemyCurrentHealth;

    public GameObject player;
    public GameManager gameManager;
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

        // �ǰ� ȿ��
        HitEffect();

        if (enemyCurrentHealth <= 0)
        {
            // �÷��̾� ���� ����
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // �ı� �� ����
            BeforeDeActive();

            // �� �ı�
            gameObject.SetActive(false);
            gameManager.CallExplosion(transform.position, enemyObjectType);

            // �ı� �� ����
            AfterDeActive();
        }
    }

    abstract protected void HitEffect();

    abstract protected void BeforeDeActive();
    
    abstract protected void AfterDeActive();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            collision.gameObject.SetActive(false);
        }
    }

    protected void FireBulletToPlayer(ObjectType bulletGameType, int bulletSpeed, Vector3 startPosition)
    {
        FireBulletToPosition(bulletGameType, bulletSpeed, startPosition, player.transform.position);
    }

    protected void FireBulletToPosition(ObjectType bulletGameType, int bulletSpeed, Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 directionVector = (endPosition - startPosition).normalized;
        FireBulletToDirection(bulletGameType, bulletSpeed, startPosition, directionVector);
    }

    protected void FireBulletToDirection(ObjectType bulletGameType, int bulletSpeed, Vector3 startPosition, Vector3 directionVector)
    {
        GameObject bullet = objectManager.GetObjectWithPosition(bulletGameType, startPosition);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(directionVector * bulletSpeed, ForceMode2D.Impulse);

        // �ش� ������ ������ ��� (���� to ��)
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg + 90;
        // ������Ʈ�� z �� �������� ȸ��. X�� Y�� ȸ����Ű�� ����
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
