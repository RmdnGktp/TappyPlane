using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("FUEL BOOST")]
    [SerializeField] Button buyFuelBoost;
    [SerializeField] TextMeshProUGUI buyFuelBoostText;
    bool isFuelBoostActivated;

    [Header("SHIELD")]
    [SerializeField] Button buyShield;
    [SerializeField] TextMeshProUGUI buyShieldText;
    bool isShieldActivated;

    [Header ("FUEL MAGNET")]
    [SerializeField] Button buyFuelMagnet;
    [SerializeField] TextMeshProUGUI buyFuelMagnetText;
    bool isFuelMagnetActivated;

    [Header ("EXTRA LIFE")]
    [SerializeField] Button buyExtraLife;
    [SerializeField] TextMeshProUGUI buyExtraLifeText;
    bool isExtraLifeActivated = false;
    int extraLife = 1;

    [Header("GENERAL")]
    [SerializeField] GameManagerScript gameManagerScript;
    [SerializeField] PlaneScript planeScript;
    [SerializeField] GameObject Magnet;
    int stars;

    [Header("REVIVE")]
    [SerializeField] Button reviveButton;
    [SerializeField] TextMeshProUGUI reviveButtonText;

    void Start()
    {
        UpdateShopUI();   
        extraLife = PlayerPrefs.GetInt("extraLife", 1);
        isExtraLifeActivated = GetBool ("isExtraLifeActivated");

        if (isExtraLifeActivated)
        {
            UpdateButton(buyExtraLife, buyExtraLifeText);
        }

        UpdateReviveButton();
    }

    public void BuyFuelBoost()
    {   
        isFuelBoostActivated = true;
        gameManagerScript.SetStars(-3);
        planeScript.setMaxFuel(50);
        UpdateButton(buyFuelBoost, buyFuelBoostText);
    }

    public void BuyShield()
    {   
        isShieldActivated = true;
        gameManagerScript.SetStars(-3);
        planeScript.ActivateShield();
        UpdateButton(buyShield, buyShieldText);
    }
    public void BuyFuelMagnet()
    {   
        isFuelMagnetActivated = true;
        gameManagerScript.SetStars(-3);
        UpdateButton(buyFuelMagnet, buyFuelMagnetText);
        Magnet.SetActive(true);
    }

    public void BuyExtraLife()
    {   
        isExtraLifeActivated = true;
        SetBool("isExtraLifeActivated", true);
        UpdateReviveButton();
        gameManagerScript.SetStars(-6);
        UpdateButton(buyExtraLife, buyExtraLifeText);
        SetExtraLife (1);

    }

    public void SetExtraLife(int value)
    {
        extraLife += value;
        PlayerPrefs.SetInt("extraLife", extraLife);
        PlayerPrefs.Save();
    }

    public int GetExtraLife()
    {
        return extraLife;
    }
    void UpdateButton (Button button, TextMeshProUGUI text)
    {   
        UpdateShopUI();
        button.interactable = false;
        text.text = "Activated";
    }
    

    public void UpdateShopUI()
    {
        stars = gameManagerScript.GetValue("stars", 0);

        if (stars >= 6)
        {   
            ActivateButton(buyExtraLife, isExtraLifeActivated);
        }
        else if (stars >= 3)
        {   
            ActivateButton(buyFuelBoost, isFuelBoostActivated);
            ActivateButton(buyShield, isShieldActivated);
            ActivateButton(buyFuelMagnet, isFuelMagnetActivated);
            buyExtraLife.interactable = false;
        }
        else
        {
            buyFuelBoost.interactable = false;
            buyShield.interactable = false;
            buyFuelMagnet.interactable = false;
            buyExtraLife.interactable = false;
        }
    }

    void ActivateButton (Button button, bool statement)
    {
        if (!statement)
        {
            button.interactable = true;
        }
    }

    public void SetBool (string name, bool value)
    {
        PlayerPrefs.SetInt(name, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool GetBool (string name)
    {
        return PlayerPrefs.GetInt(name, 0) == 1;
    }

    public void Revive()
    {
        if (extraLife == 2)
        {   
            Debug.Log ("Extra Life used!");
            SetExtraLife(-1);
            planeScript.Revive(); 
            isExtraLifeActivated = false;
            SetBool("isExtraLifeActivated", false);
            reviveButton.interactable = false;
            reviveButton.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
            Invoke (nameof(UpdateReviveButton), 0.5f);
            // UpdateReviveButton();
        }
        else if (extraLife == 1)
        {   
            Debug.Log ("Playing Ads");
            reviveButton.interactable = false;
            reviveButton.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
            planeScript.Revive(); 
        }
        else
        {
            return;
        }
    }

    public void UpdateReviveButton()
    {
        if (isExtraLifeActivated)
        {
            reviveButtonText.text = "Extra Life";
        }
        else
        {   
            reviveButtonText.text = "Revive";
            reviveButton.interactable = true;
            reviveButton.GetComponent<Image>().color = new Color (78f/225f, 186f/255f, 26f/255f, 255f/255f);
        }
    }

}
