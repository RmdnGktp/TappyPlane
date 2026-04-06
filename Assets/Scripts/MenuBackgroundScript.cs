using UnityEngine;

public class MenuBackgroundScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float width;

    void Update()
    {
        transform.Translate (Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < -width)
        {   
            transform.position += new Vector3(width, 0f, 0f);
        }

    }
}
