using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static ObjectManager;
using static UnityEngine.GraphicsBuffer;

public class Follower : MonoBehaviour
{
    public float maxShotDelay = 1f;
    public int bulletSpeed = 3;
    public Transform parentTransform;
    public float followDistance = 1.0f;
    public float moveSpeed = 100.0f;
    public float smoothTime = 0.2f;  // ���� �ð�

    public ObjectManager objectManager;

    private Vector3 velocity = Vector3.zero;  // ���� ��꿡 ���Ǵ� �ӽ� ����

    float currentShotDelay;

    private void Awake()
    {
    }

    void Update()
    {
        Follow();
        Fire();
        Reload();
    }

    private void Follow()
    {
        Vector3 parentPosition = parentTransform.position;
        
        // ������ ���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, parentPosition);
        if (distance <= followDistance)
            return;

        // ��ǥ ��ġ�� ���󰡵�, ���� �Ÿ�(followDistance)�� ����
        Vector3 direction = parentPosition - transform.position;
        if (direction.magnitude > followDistance)
        {
            // ��ǥ ������ ���� �����̵�, ���� �Ÿ� ������ ����
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, parentPosition, moveSpeed * Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    private void CreateBullet(ObjectType bulletObjectType, Vector3 position)
    {
        GameObject bullet = objectManager.GetObjectWithPosition(bulletObjectType, position);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
    }


    private void Fire()
    {
        if (currentShotDelay < maxShotDelay)
            return;

        CreateBullet(ObjectType.FOLLOWER_BULLET, transform.position);

        currentShotDelay = 0;
    }

    private void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }
}
