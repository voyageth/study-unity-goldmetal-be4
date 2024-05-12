using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectManager;

public class Explosion : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Invoke("Disable", 2f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    public void StartExplosion(ObjectType objectType)
    {
        animator.SetTrigger("OnExplosion");

        switch(objectType)
        {
            case ObjectType.PLAYER:
                transform.localScale = Vector3.one * 1f;
                break;
            case ObjectType.ENEMY_SMALL:
                transform.localScale = Vector3.one * 0.7f;
                break;
            case ObjectType.ENEMY_MEDIUM:
                transform.localScale = Vector3.one * 1f;
                break;
            case ObjectType.ENEMY_LARGE:
                transform.localScale = Vector3.one * 2f;
                break;
            case ObjectType.ENEMY_BOSS:
                transform.localScale = Vector3.one * 3f;
                break;
            default:
                transform.localScale = Vector3.one * 1f;
                break;
        }
    }
}
