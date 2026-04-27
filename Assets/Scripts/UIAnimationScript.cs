using System.Collections;
using UnityEngine;

public class UIAnimationScript : MonoBehaviour
{
    RectTransform rect;
    float currentY;
    [SerializeField] float startY;
    [SerializeField] float speed;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        currentY = rect.anchoredPosition.y;
        RestartUIPosiotion();
    }

    [ContextMenu ("GameOver")]
    public void GameOver()
    {
        StartCoroutine(MoveUI());
    }
    
    IEnumerator MoveUI()
    {
        float t = 0;
        while ( t < 1)
        {
            t += Time.deltaTime * speed;
            float y = Mathf.SmoothStep (startY, currentY, t);
            rect.anchoredPosition = new Vector2(0, y);
            yield return null;
        }
    }

    public void RestartUIPosiotion()
    {
        rect.anchoredPosition = new Vector2(0, startY);
    }
}
