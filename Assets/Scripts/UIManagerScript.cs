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
    [SerializeField] GameObject questLogUI;
    [SerializeField] PlaneScript planeScript;
    [SerializeField] Button extraFuelButton; 
    [SerializeField] Button buyMoreButton;
    [SerializeField] GameObject shopMenu;
    float fadeSpeed = 2f;
    float alpha;
    [SerializeField] QuestManager questManager;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject menuUI;
    [SerializeField] Button homeButton; 
    [SerializeField] Button questButton; 
    [SerializeField] Button shopButton; 
    private AudioManager audioManager;
   
    void Start()
    {  
        audioManager = FindFirstObjectByType<AudioManager>();
        StartCoroutine(FadeIn()); 
    }

    public void StartGame()
    {
        startGameUI.SetActive(false);
        tappyPlaneText.SetActive(false);
        gameUI.SetActive(true);
        startGameScripts.SetActive(true);
        planeScript.Play();
        menuUI.SetActive(false);
        
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
        
        audioManager.PlaySwooshSFX();
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
        AdManager.Instance.hasExtraFuel = true;
        AdManager.Instance.ShowAd();
        //planeScript.setMaxFuel(50);
        extraFuelButton.interactable = false;
        extraFuelButton.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        blackScreen.color = new Color (0, 0, 0, 0.5f);

        startGameUI.SetActive(false);
        tappyPlaneText.SetActive(false);
        plane.SetActive(false);
        questLogUI.SetActive(false);

        shopButton.GetComponent<Image>().color = new Color (192f/255f, 59f/255f, 59f/255f, 255f/255f); // red
        homeButton.GetComponent<Image>().color = new Color (70f/255f, 171f/255f, 27f/255f, 255f/255f); // green
        questButton.GetComponent<Image>().color = new Color (70f/255f, 171f/255f, 27f/255f, 255f/255f); // green
    }

    public void ReturnHome()
    {
        shopMenu.SetActive(false);
        questLogUI.SetActive(false);
        blackScreen.color = new Color (0, 0, 0, 0);
        startGameUI.SetActive(true);
        tappyPlaneText.SetActive(true);
        plane.SetActive(true);

        homeButton.GetComponent<Image>().color = new Color (192f/255f, 59f/255f, 59f/255f, 255f/255f); // red
        shopButton.GetComponent<Image>().color = new Color (70f/255f, 171f/255f, 27f/255f, 255f/255f); // green
        questButton.GetComponent<Image>().color = new Color (70f/255f, 171f/255f, 27f/255f, 255f/255f); // green
    }

    public void OpenQuestLog()
    {
        questLogUI.SetActive(true);
        blackScreen.color = new Color (0, 0, 0, 0.5f);

        startGameUI.SetActive(false);
        tappyPlaneText.SetActive(false);
        plane.SetActive(false);
        shopMenu.SetActive(false);

        questButton.GetComponent<Image>().color = new Color (192f/255f, 59f/255f, 59f/255f, 255f/255f); // red
        shopButton.GetComponent<Image>().color = new Color (70f/255f, 171f/255f, 27f/255f, 255f/255f); // green
        homeButton.GetComponent<Image>().color = new Color (70f/255f, 171f/255f, 27f/255f, 255f/255f); // green
    }

    public void BuyMore()
    {
        Debug.Log ("Playing Ads");
        AdManager.Instance.hasExtraStars = true;
        AdManager.Instance.ShowAd();
        buyMoreButton.interactable = false;
        buyMoreButton.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
    }

    


}
