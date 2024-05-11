using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;
    
    float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }

    void Update()
    {
        Move();
        Scrolling();
    }

    private void Move()
    {
        // move
        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = Vector3.down * speed * Time.deltaTime;
        transform.position = currentPosition + nextPosition;
    }

    private void Scrolling()
    {
        if (sprites[endIndex].position.y < -viewHeight)
        {
            // sprite reuse
            Vector3 backSpritePosition = sprites[startIndex].localPosition;
            Vector3 frontSpritePosition = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePosition + Vector3.up * viewHeight;

            // cursor index change
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = --startIndexSave;
            if (endIndex <= -1)
                endIndex = sprites.Length - 1;
        }
    }
}
