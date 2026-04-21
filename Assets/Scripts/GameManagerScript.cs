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
    bool isNewDistanceQuest;

    [Header("Enemy Quest")]
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] GameObject enemyNewText;
    [SerializeField] int targetEnemy;
    [SerializeField] int enemyIncrement;
    bool isNewEnemyQuest;
    int currentAmount;

    [Header("Awards")]
    [SerializeField] TextMeshProUGUI starsText;
    [SerializeField] TextMeshProUGUI starsOnShop;
    int stars = 0;

    void Start()
    {   
        // Distance Quest
        targetDistance = GetValue("targetDistance", targetDistance);
        isNewDistanceQuest = GetValue("isNewDistanceQuest", 1) == 1;
        // Enemy Quest
        targetEnemy = GetValue("targetAmount", targetEnemy);
        isNewEnemyQuest = GetValue("isNewEnemyQuest", 1) == 1;
        currentAmount = GetValue("currentAmount", targetEnemy);
        // Stars
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

            isNewDistanceQuest = true;
            SetValue("isNewDistanceQuest", isNewDistanceQuest ? 1:0);
            // ? means if -> If isNewDistanceQuest is true, the value is 1, if it is false, the value is 0

            GetAwards(3);
        }
    }

    public void UpdateEnemyQuest (int amount)
    {   
        currentAmount -= amount;

        if (currentAmount <= 0)
        {
            Debug.Log ("Enemy Quest Completed!");
            targetEnemy += enemyIncrement;
            SetValue("targetEnemy", targetEnemy);
            SetValue("currentAmount", targetEnemy);

            isNewEnemyQuest = true;
            SetValue("isNewEnemyQuest", isNewEnemyQuest ? 1:0);

            GetAwards(3);
        }
        else
        {
            SetValue("currentAmount", currentAmount);
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
        // Distance Quest
        distanceText.text = "Fly " + targetDistance.ToString() + " meters";
        distanceNewText.SetActive(isNewDistanceQuest);
        // Enemy Quest
        enemyText.text = "Kill " + currentAmount.ToString() + " enemies";
        enemyNewText.SetActive(isNewEnemyQuest);
        // Stars
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
        SetValue("isNewDistanceQuest", 0);
        SetValue("isNewEnemyQuest", 0);
    }

    [ContextMenu ("Reset")]
    void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    } 

}
