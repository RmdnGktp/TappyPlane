using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] List<QuestData> questDatabase;
    [SerializeField] List<Quest> activeQuests = new List<Quest>();


    void Start()
    {
        LoadQuests();
        CheckDailyReset();
    }

    void LoadQuests()
    {
        foreach (QuestData data in questDatabase)
        {
            Quest newQuest = new Quest();

            newQuest.data = data;

            activeQuests.Add(newQuest);
        }
    }

    public void UpdateQuest(QuestType type, int amount)
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.data.questType == type)
            {
                if (quest.isCompleted)
                continue;

                quest.currentValue += amount;
                Debug.Log (quest.data.questName + ": " + quest.currentValue);

                if (quest.currentValue >= quest.data.targetValue)
                {
                    CompleteQuest(quest);
                }
            }
        }
    }
    
    void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
        Debug.Log("Quest Complete: " + quest.data.questName);
        AddStars(quest.data.reward);
    }

    void AddStars(int amount)
    {
        int stars = 0;
        stars += amount;
        Debug.Log ("Reward: " + stars + " stars");
    }

    void CheckDailyReset()
    {
        string today = System.DateTime.Now.ToString("yyyyMMdd");
        string lastDate = PlayerPrefs.GetString("LastDate", "");

        if (today != lastDate)
        {
            foreach (Quest quest in activeQuests)
            {
                if (quest.data.isDaily)
                {
                    quest.currentValue = 0;
                    quest.isCompleted = false;
                }
            }

            PlayerPrefs.SetString("LastDate", today);
        }
    }
}
