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
    bool isExtraLifeActivated;

    [Header("GENERAL")]
    [SerializeField] GameManagerScript gameManagerScript;
    [SerializeField] PlaneScript planeScript;
    [SerializeField] GameObject Magnet;
    int stars;

    void Start()
    {
        UpdateShopUI();   
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
        gameManagerScript.SetStars(-6);
        UpdateButton(buyExtraLife, buyExtraLifeText);
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

}
