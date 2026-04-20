using UnityEngine;
using TMPro;
using System;

public class GameManagerScript : MonoBehaviour
{
    [Header("Distance Quest")]
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] GameObject distanceNewText;
    [SerializeField] int targetDistance;
    [SerializeField] int distanceIncrement;
    bool isNewDistance;


    [Header("Awards")]
    [SerializeField] TextMeshProUGUI starsText;
    [SerializeField] TextMeshProUGUI starsOnShop;
    int stars = 0;

    void Start()
    {   
        
        targetDistance = GetValue("targetDistance", targetDistance);
        isNewDistance = GetValue("isNewDistance", 0) == 1;
        
        stars = GetValue("stars", stars);
        UpdateUI();
    } 

    public void UpdateDistanceQuest (int distance)
    {
        if (distance >= targetDistance)
        {
            Debug.Log ("Distance Quest Completed!");
            targetDistance += distanceIncrement;
            SetValue("targetDistance", targetDistance);

            isNewDistance = true;
            SetValue("isNewDistance", isNewDistance ? 1:0);
            // ? means if -> If isNewDistance is true, the value is 1, if it is false, the value is 0

            GetAwards(3);
        }
    }

   void GetAwards (int value)
    {
        stars += value;
        SetValue("stars", stars);
        Debug.Log (stars);
    }

    void UpdateUI ()
    {
        distanceText.text = "Fly " + targetDistance.ToString() + " meters";
        distanceNewText.SetActive(isNewDistance);

        starsText.text = stars.ToString();
        starsOnShop.text = stars.ToString();

    }

    void SetValue(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
        PlayerPrefs.Save();
    }

    int GetValue (string name, int value)
    {
        return PlayerPrefs.GetInt (name, value);
    }

    public void ResetUI()
    {
        SetValue("isNewDistance", 0);
    }

    [ContextMenu ("Reset")]
    void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    } 

}
