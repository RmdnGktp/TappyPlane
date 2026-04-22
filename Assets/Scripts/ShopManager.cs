using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Fuel Boost")]
    [SerializeField] Button buyFuelBoost;
    bool isFuelBoostActivated;
    [SerializeField] PlaneScript planeScript;


    [Header("General")]
    [SerializeField] GameManagerScript gameManagerScript;
    int stars;

    void Start()
    {
        stars = gameManagerScript.GetValue("stars", 0);
        UpdateShopUI();
    }

    public void BuyFuelBoost()
    {   
        gameManagerScript.SetStars(-3);
        planeScript.setMaxFuel(50);
        buyFuelBoost.interactable = false;
    }

    public 

    void UpdateShopUI()
    {
        
    }

}
