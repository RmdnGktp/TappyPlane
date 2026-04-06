using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIManagerScript : MonoBehaviour
{
    [SerializeField] Image blackScreen;
    float fadeSpeed = 2f;
    float alpha;

    void Start()
    {  
        StartCoroutine(FadeIn()); 
    }
    

    public void Restart ()
    {
        StartCoroutine(LoadGame(1));
    }

    public void Menu()
    {
        StartCoroutine(LoadGame(0));
    }

    public IEnumerator LoadGame(int value)
    {   
        // Fade out animation
        alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            blackScreen.color = new Color (0, 0, 0, alpha);
            yield return null;
        }
        
        SceneManager.LoadScene (value);
        yield return null;
    }

    IEnumerator FadeIn()
    {   
        alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            blackScreen.color = new Color (0, 0, 0, alpha);
            yield return null;
        }
    }

    

}
