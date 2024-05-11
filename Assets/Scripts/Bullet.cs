using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public bool isRotate;

    private void Update()
    {
        if (isRotate)
            transform.Rotate(Vector3.forward * 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
        }
    }
}
