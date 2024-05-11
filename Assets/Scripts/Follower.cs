using System.Collections.Generic;
using UnityEngine;
using static ObjectManager;

public class Follower : MonoBehaviour
{
    public float maxShotDelay = 1f;
    public int bulletSpeed = 3;
    public Transform parentTransform;
    public int followDelay = 12;
    public ObjectManager objectManager;

    Vector3 followPosition;
    Queue<Vector3> parentPositionQueue;

    float currentShotDelay;

    private void Awake()
    {
        parentPositionQueue = new Queue<Vector3>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    private void Watch()
    {
        // Input position
        if (!parentPositionQueue.Contains(parentTransform.position))
            parentPositionQueue.Enqueue(parentTransform.position);

        // Output position
        if (parentPositionQueue.Count > followDelay)
            followPosition = parentPositionQueue.Dequeue();
        else if (followPosition == null)
            followPosition = parentTransform.position;
    }

    private void Follow()
    {
        transform.position = followPosition;
    }

    private void CreateBullet(PrefabType bulletObjectType, Vector3 position)
    {
        GameObject bullet = objectManager.GetObject(bulletObjectType, position, transform.rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
    }


    private void Fire()
    {
        if (currentShotDelay < maxShotDelay)
            return;

        CreateBullet(PrefabType.FOLLOWER_BULLET, transform.position);

        currentShotDelay = 0;
    }

    private void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }
}
