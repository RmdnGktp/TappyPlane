using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.Multiplayer.Center.Common;

public class UIManagerScript : MonoBehaviour
{
    [SerializeField] Image blackScreen;
    [SerializeField] GameObject startGameUI;
    [SerializeField] GameObject tappyPlaneText;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject startGameScripts;
    [SerializeField] PlaneScript planeScript;
    [SerializeField] Button extraFuelButton; 
    [SerializeField] Button buyMoreButton;
    [SerializeField] GameManagerScript gameManagerScript;
    [SerializeField] GameObject shopMenu;
    float fadeSpeed = 2f;
    float alpha;
   
    void Start()
    {  
        StartCoroutine(FadeIn()); 
    }

    public void StartGame()
    {
        startGameUI.SetActive(false);
        tappyPlaneText.SetActive(false);
        gameUI.SetActive(true);
        startGameScripts.SetActive(true);
        planeScript.Play();
        gameManagerScript.ResetUI();
    }
    

    public void Restart ()
    {
        StartCoroutine(LoadGame(0));
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
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

    public void GetExtraFuel()
    {
        Debug.Log ("Playing Ads");
        planeScript.setMaxFuel(50);
        extraFuelButton.interactable = false;
        extraFuelButton.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        blackScreen.color = new Color (0, 0, 0, 0.7f);
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        blackScreen.color = new Color (0, 0, 0, 0);
    }

    public void BuyMore()
    {
        Debug.Log ("Playing Ads");
        gameManagerScript.SetStars(3);
        buyMoreButton.interactable = false;
        buyMoreButton.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
    }

    


}
