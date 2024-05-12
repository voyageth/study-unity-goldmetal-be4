using UnityEngine;
using static ObjectManager;

public class Player : MonoBehaviour
{
    const int MAX_BULLET_POWER = 6;
    const int MAX_LIFE_COUNT = 3;
    const int MAX_BOOM_COUNT = 3;

    public int life = 3;
    public int score = 0;
    public bool isHit;
    public bool isBoomTime;
    public int playerSpeed = 3;
    public int bulletSpeed = 10;
    public int bulletPower = 1;
    public int boomCount = 3;
    public float maxShotDelay = 0.2f;
    public GameManager gameManager;
    public ObjectManager objectManager;
    public GameObject boomEffect;
    public GameObject[] followers;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;

    bool isTouchTop;
    bool isTouchBottom;
    bool isTouchRight;
    bool isTouchLeft;
    float currentShotDelay;
    bool isRespawnTime;

    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        TurnOnUnbeatable();
        Invoke("TurnOffUnbeatable", 3);
    }

    void TurnOnUnbeatable()
    {
        Unbeatable(true);
    }

    void TurnOffUnbeatable()
    {
        Unbeatable(false);
    }

    void Unbeatable(bool active)
    {

        if (active)
        {
            isRespawnTime = true;
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            for (int index = 0; index < followers.Length; index++)
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            isRespawnTime = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);

            for (int index = 0; index < followers.Length; index++)
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
    }

    private void Boom()
    {
        //if (!Input.GetButton("Jump"))
        //    return;

        if (!isButtonB)
            return;

        if (isBoomTime)
            return;

        if (boomCount <= 0)
            return;

        boomCount--;
        isButtonB = false;
        gameManager.UpdateBoomIcon(boomCount, MAX_BOOM_COUNT);

        // 1. boom effect
        OnBoomEffect();
        Invoke("OffBoomEffect", 4);

        // 2. remove enemy
        objectManager.BoomDamageToAllEnemies();

        // 3. remove enemy bullet
        objectManager.DestroyAllEnemyBullets();

    }

    public void JoyPanel(int type)
    {
        for (int index = 0; index < 9; index++)
        {
            joyControl[index] = index == type;
        }
    }

    public void JoyDown()
    {
        isControl = true;
    }

    public void JoyUp()
    {
        isControl = false;
    }

    private void Move()
    {
        // keyboard control
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // joy control
        if (joyControl[0]) { h = -1;v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }


        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1) || !isControl)
            h = 0;

        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1) || !isControl)
            v = 0;

        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = new Vector3(h, v, 0) * playerSpeed * Time.deltaTime;

        transform.position = currentPosition + nextPosition;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
            animator.SetInteger("HorizontalInput", (int)h);
    }

    private void CreateBullet(ObjectType bulletObjectType, Vector3 position)
    {
        GameObject bullet = objectManager.GetObjectWithPosition(bulletObjectType, position);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
    }

    public void ButtonADown()
    {
        isButtonA = true;
    }
    public void ButtonAUp()
    {
        isButtonA = false;
    }

    public void ButtonBDown()
    {
        isButtonB = true;
    }


    private void Fire()
    {
        //if (!Input.GetButton("Fire1"))
        //    return;

        if (!isButtonA)
            return;

        if (currentShotDelay < maxShotDelay)
            return;

        if (bulletPower <= 1)
            CreateBullet(ObjectType.PLAYER_BULLET_A, transform.position);
        else if (bulletPower == 2)
        {
            CreateBullet(ObjectType.PLAYER_BULLET_A, transform.position + Vector3.right * 0.1f);
            CreateBullet(ObjectType.PLAYER_BULLET_A, transform.position + Vector3.left * 0.1f);
        }
        else if (bulletPower >= 3)
        {
            CreateBullet(ObjectType.PLAYER_BULLET_B, transform.position);
            for (int index = 0; index < bulletPower - 2; index++)
            {
                float positionFactor = 0.35f + 0.25f * index;
                CreateBullet(ObjectType.PLAYER_BULLET_B, transform.position);
                CreateBullet(ObjectType.PLAYER_BULLET_A, transform.position + Vector3.right * positionFactor);
                CreateBullet(ObjectType.PLAYER_BULLET_A, transform.position + Vector3.left * positionFactor);
            }
        }

        currentShotDelay = 0;
    }

    private void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isRespawnTime)
            {
                return;
            }
            
            if (isHit)
            {
                return;
            }
            isHit = true;

            life--;
            gameManager.UpdateLifeIcon(life, MAX_LIFE_COUNT);
            gameManager.CallExplosion(transform.position, ObjectType.PLAYER);

            if (life <= 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch(item.type)
            {
                case Item.ItemType.COIN:
                    score += 1000;
                    break;
                case Item.ItemType.POWER:
                    if (bulletPower < MAX_BULLET_POWER)
                        bulletPower++;
                    else
                        score += 500;

                    AddFollower();
                    break;
                case Item.ItemType.BOOM:
                    if (boomCount < MAX_BOOM_COUNT)
                    {
                        boomCount++;
                        gameManager.UpdateBoomIcon(boomCount, MAX_BOOM_COUNT);
                    }
                    else
                        score += 500;
                    break;
            }

            collision.gameObject.SetActive(false);
        }
    }

    private void AddFollower()
    {
        if (bulletPower >= 4)
            followers[0].SetActive(true);
        if (bulletPower >= 5)
            followers[1].SetActive(true);
        if (bulletPower >= 6)
            followers[2].SetActive(true);
    }

    void OnBoomEffect()
    {
        boomEffect.SetActive(true);
        isBoomTime = true;
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
