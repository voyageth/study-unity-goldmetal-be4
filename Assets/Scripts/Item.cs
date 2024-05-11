using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType type;
    Rigidbody2D itemRigidbody;

    public enum ItemType
    {
        BOOM,
        COIN,
        POWER
    }

    private void Awake()
    {
        itemRigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        itemRigidbody.velocity = Vector3.down * 0.3f;
    }
}
