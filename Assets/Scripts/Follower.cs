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
    public float smoothTime = 0.2f;  // 감쇠 시간

    public ObjectManager objectManager;

    private Vector3 velocity = Vector3.zero;  // 감쇠 계산에 사용되는 임시 변수

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
        
        // 대상과의 현재 거리 계산
        float distance = Vector3.Distance(transform.position, parentPosition);
        if (distance <= followDistance)
            return;

        // 목표 위치를 따라가되, 일정 거리(followDistance)를 유지
        Vector3 direction = parentPosition - transform.position;
        if (direction.magnitude > followDistance)
        {
            // 목표 지점을 향해 움직이되, 일정 거리 내에서 멈춤
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
