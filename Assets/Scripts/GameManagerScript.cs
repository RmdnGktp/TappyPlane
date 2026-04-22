using UnityEngine;
using TMPro;
using System;

public class GameManagerScript : MonoBehaviour
{
    [Header("Distance Quest")]
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] GameObject distanceNewText;
    [SerializeField] int targetDistance;
    [SerializeField] TextMeshProUGUI awardsTextDQ;
    int starsDQ;
    bool isNewDistanceQuest;

    [Header("Enemy Quest")]
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] GameObject enemyNewText;
    [SerializeField] int targetEnemy;
    bool isNewEnemyQuest;
    int currentAmount;
    bool isEnemyQuestCompleted;

    [Header("Game Quest")]
    [SerializeField] TextMeshProUGUI gameText;
    [SerializeField] GameObject gameNewText;
    [SerializeField] int targetPlayedGame;
    int increment = 3;
    bool isNewGameQuest;
    int currentPlayedGameCount;
    bool isGameQuestCompleted;
    [SerializeField] TextMeshProUGUI awardsTextGQ;

    [Header("Awards")]
    [SerializeField] TextMeshProUGUI starsText;
    [SerializeField] TextMeshProUGUI starsOnShop;
    int stars = 0;

    void Start()
    {   
        // Distance Quest
        targetDistance = GetValue("targetDistance", targetDistance);
        isNewDistanceQuest = GetValue("isNewDistanceQuest", 1) == 1;
        starsDQ = GetValue ("starsDQ", 3);

        // Enemy Quest
        targetEnemy = GetValue("targetEnemy", targetEnemy);
        isNewEnemyQuest = GetValue("isNewEnemyQuest", 1) == 1;
        currentAmount = GetValue("currentAmount", targetEnemy);
        isEnemyQuestCompleted = false;

        // Played Game Quest
        targetPlayedGame = GetValue("targetPlayedGame", targetPlayedGame);
        isNewGameQuest = GetValue("isNewGameQuest", 1) == 1;
        currentPlayedGameCount = GetValue("currentPlayedGameCount", targetPlayedGame);
        isGameQuestCompleted = false;
        
        // Stars
        stars = GetValue("stars", stars);
        UpdateUI();
    } 

    public void UpdateDistanceQuest (int distance)
    {
        if (distance >= targetDistance)
        {
            Debug.Log ("Distance Quest Completed!");
            targetDistance = Mathf.RoundToInt(targetDistance * 1.3f);;
            SetValue("targetDistance", targetDistance);

            starsDQ = Mathf.FloorToInt(starsDQ * 1.2f);
            SetValue("starsDQ", starsDQ);
            print ("StarsDQ: " + starsDQ);

            isNewDistanceQuest = true;
            SetValue("isNewDistanceQuest", isNewDistanceQuest ? 1:0);
            // ? means if -> If isNewDistanceQuest is true, the value is 1, if it is false, the value is 0

            SetStars(3);
        }

    }

    public void UpdateEnemyQuest (int amount)
    {   

       if (isEnemyQuestCompleted) return;

        currentAmount -= amount;

        if (currentAmount <= 0)
        {
            Debug.Log ("Enemy Quest Completed!");
            isEnemyQuestCompleted = true;
            targetEnemy = Mathf.RoundToInt(targetEnemy * 1.3f);
            SetValue("targetEnemy", targetEnemy);
            SetValue("currentAmount", targetEnemy);
            
            isNewEnemyQuest = true;
            SetValue("isNewEnemyQuest", isNewEnemyQuest ? 1:0);

            SetStars(3);
        }
        else
        {
            SetValue("currentAmount", currentAmount);
        }

    }

    public void UpdateGameQuest (int value)
    {
        if (isGameQuestCompleted) return;

        currentPlayedGameCount -= value;
        if (currentPlayedGameCount <= 0)
        {
            Debug.Log ("Game Quest Completed!");
            isGameQuestCompleted = true;
            SetStars(targetPlayedGame);
            targetPlayedGame += increment;
            SetValue("targetPlayedGame", targetPlayedGame);
            SetValue("currentPlayedGameCount", targetPlayedGame);

            isNewGameQuest = true;
            SetValue("isNewGameQuest", isNewGameQuest ? 1:0);
        }
        else
        {
            SetValue("currentPlayedGameCount", currentPlayedGameCount);
        }
    }

   public void SetStars (int value)
    {
        stars += value;
        SetValue("stars", stars);
        //Debug.Log (stars);
        UpdateUI();
    }

    void UpdateUI ()
    {
        // Distance Quest
        distanceText.text = "Fly " + targetDistance.ToString() + " meters";
        distanceNewText.SetActive(isNewDistanceQuest);
        awardsTextDQ.text = starsDQ.ToString();

        // Enemy Quest
        enemyText.text = "Kill " + currentAmount.ToString() + " enemies";
        enemyNewText.SetActive(isNewEnemyQuest);

        // Played Game Quest
        gameText.text = "Play " + currentPlayedGameCount.ToString() + " games";
        gameNewText.SetActive(isNewGameQuest);
        awardsTextGQ.text = targetPlayedGame.ToString();

        // Stars
        starsText.text = stars.ToString();
        starsOnShop.text = stars.ToString();

    }

    void SetValue(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
        PlayerPrefs.Save();
    }

    public int GetValue (string name, int value)
    {
        return PlayerPrefs.GetInt (name, value);
    }

    public void ResetUI()
    {
        SetValue("isNewDistanceQuest", 0);
        SetValue("isNewEnemyQuest", 0);
        SetValue("isNewGameQuest", 0);
    }

    [ContextMenu ("Reset")]
    void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    } 

}
