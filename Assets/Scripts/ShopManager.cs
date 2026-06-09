using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("ENERGY BOOST")]
    [SerializeField] Button buyFuelBoost;
    [SerializeField] TextMeshProUGUI buyFuelBoostText;
    [SerializeField] bool isFuelBoostActivated;
    [SerializeField] GameObject boostGameIcon;

    [Header("SHIELD")]
    [SerializeField] Button buyShield;
    [SerializeField] TextMeshProUGUI buyShieldText;
    [SerializeField] bool isShieldActivated;
    [SerializeField] GameObject shieldGameIcon;

    [Header ("ENERGY MAGNET")]
    [SerializeField] Button buyFuelMagnet;
    [SerializeField] TextMeshProUGUI buyFuelMagnetText;
    [SerializeField] bool isFuelMagnetActivated;
    [SerializeField] GameObject magnetGameIcon;

    [Header ("REBOOT")]
    [SerializeField] Button buyExtraLife;
    [SerializeField] TextMeshProUGUI buyExtraLifeText;
    [SerializeField] bool isExtraLifeActivated = false;
    int extraLife = 1;
    [SerializeField] GameObject rebootGameIcon;

    [Header("GENERAL")]
    [SerializeField] QuestManager questManager;
    [SerializeField] PlaneScript planeScript;
    [SerializeField] GameObject Magnet;
    private int stars;

    [Header("REVIVE")]
    [SerializeField] Button reviveButton;
    [SerializeField] TextMeshProUGUI reviveButtonText;


    void Start()
    {
        //UpdateShopUI();   
        extraLife = PlayerPrefs.GetInt("extraLife", 1);
        isExtraLifeActivated = GetBool ("isExtraLifeActivated");

        if (isExtraLifeActivated)
        {
            UpdateButton(buyExtraLife, buyExtraLifeText);
            ActivateGameIcon(rebootGameIcon, true);
        }

        UpdateReviveButton();
    }

    public void BuyFuelBoost()
    {   
        isFuelBoostActivated = true;
        questManager.AddStars(-3);
        questManager.UpdateQuest(QuestType.UseFuelBoost, 1);
        planeScript.setMaxFuel(50);
        UpdateButton(buyFuelBoost, buyFuelBoostText);
        ActivateGameIcon(boostGameIcon, true);
    }

    public void BuyShield()
    {   
        isShieldActivated = true;
        questManager.AddStars(-3);
        questManager.UpdateQuest(QuestType.UseShield, 1);
        planeScript.ActivateShield();
        UpdateButton(buyShield, buyShieldText);
        ActivateGameIcon(shieldGameIcon, true);
    }
    public void BuyFuelMagnet()
    {   
        isFuelMagnetActivated = true;
        questManager.AddStars(-3);
        questManager.UpdateQuest(QuestType.UseFuelMagnet, 1);
        UpdateButton(buyFuelMagnet, buyFuelMagnetText);
        Magnet.SetActive(true);
        ActivateGameIcon(magnetGameIcon, true);
    }

    public void BuyExtraLife()
    {   
        isExtraLifeActivated = true;
        SetBool("isExtraLifeActivated", true);
        UpdateReviveButton();
        questManager.AddStars(-6);
        questManager.UpdateQuest(QuestType.UseExtraLife, 1);
        UpdateButton(buyExtraLife, buyExtraLifeText);
        SetExtraLife (1);
        ActivateGameIcon(rebootGameIcon, true);

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
        text.text = "READY";
    }

    void ActivateGameIcon (GameObject icon, bool state)
    {
        icon.SetActive(state);
    }
    

    public void UpdateShopUI()
    {
        int stars = questManager.GetStars();
        print("updateShopUi: " + stars);

        if (stars >= 6)
        {   
            ActivateButton(buyExtraLife, isExtraLifeActivated);
            ActivateButton(buyFuelBoost, isFuelBoostActivated);
            ActivateButton(buyShield, isShieldActivated);
            ActivateButton(buyFuelMagnet, isFuelMagnetActivated);
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
            //reviveButton.GetComponent<Image>().color = new Color (5f/255f, 5f/255f, 5f/255f, 255f/255f);
            //reviveButton.GetComponent<Outline>().enabled = true;
            Invoke (nameof(UpdateReviveButton), 0.5f);
            // UpdateReviveButton();
            ActivateGameIcon(rebootGameIcon, false);
        }
        else if (extraLife == 1)
        {   
            Debug.Log ("Playing Ads");
            AdManager.Instance.isRevived = true;
            AdManager.Instance.ShowAd();
            reviveButton.interactable = false;
            reviveButton.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
            reviveButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color (1f, 1f, 1f, 1f);
            //reviveButton.GetComponent<Outline>().enabled = false;
            //planeScript.Revive(); 
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
            reviveButtonText.text = "Reboot";
        }
        else
        {   
            reviveButtonText.text = "Revive";
            reviveButton.interactable = true;
            reviveButton.GetComponent<Image>().color = new Color (1f, 1f, 1f, 1f);
            //reviveButton.GetComponent<Image>().color = new Color (5f/255f, 5f/255f, 5f/255f, 255f/255f);
            //reviveButton.GetComponent<Outline>().enabled = true;
        }
    }

}
