using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class QuestManager : MonoBehaviour
{
    const int MaxActiveQuests = 3;

    const string LastDateKey = "QuestLastDate";
    const string CompletedQuestCountKey = "QuestCompletedCount";

    [Header("All Available Quests")]
    public List<QuestData> questDatabase;

    [Header("Active Quests")]
    public List<Quest> activeQuests = new List<Quest>();

    List<int> completedQuestIndexes = new List<int>();
    
    [Header("Update Quests UI")]
    [SerializeField] TextMeshProUGUI questTracker;
    [SerializeField] TextMeshProUGUI[] QuestNameText;
    [SerializeField] TextMeshProUGUI[] QuestRewardText;
    [SerializeField] Image[] QuestImage;
    [SerializeField] Image[] RewardImage;
    
    [Header("Update Stars")]
    int stars;
    [SerializeField] TextMeshProUGUI starsOnQuestLog;
    [SerializeField] TextMeshProUGUI starsOnShop;
    [SerializeField] ShopManager shopManager;

    void Awake()
    {
        stars = PlayerPrefs.GetInt ("stars", 0);
        UpdateStarsUI(stars); 
    }

    void Start()
    {
        string today = System.DateTime.Now.ToString("yyyyMMdd");
        string lastDate = PlayerPrefs.GetString(LastDateKey, "");


        if (today != lastDate)
        {
            StartNewDay(today);
        }
        else
        {
            ContinueToday();
        }

        UpdateQuestUI();
    }

    void StartNewDay(string today)
    {
        activeQuests.Clear();
        completedQuestIndexes.Clear();

        PlayerPrefs.SetString(LastDateKey, today);
        ClearSavedActiveQuests();
        ClearSavedCompletedQuests();

        FillEmptyQuestSlots();
        SaveQuests();
    }

    void ContinueToday()
    {
        activeQuests.Clear();
        completedQuestIndexes.Clear();

        LoadCompletedQuests();
        LoadActiveQuests();

        FillEmptyQuestSlots();
        SaveQuests();
    }

    void FillEmptyQuestSlots()
    {
        while (activeQuests.Count < MaxActiveQuests)
        {
            QuestData randomQuestData = GetRandomAvailableQuest();

            if (randomQuestData == null)
            {
                return;
            }

            activeQuests.Add(CreateQuest(randomQuestData));
        }
    }

    QuestData GetRandomAvailableQuest()
    {
        List<QuestData> availableQuests = new List<QuestData>();

        foreach (QuestData data in questDatabase)
        {
            if (data == null)
            {
                continue;
            }

            if (IsQuestActive(data) || IsQuestCompletedToday(data))
            {
                continue;
            }

            availableQuests.Add(data);
        }

        if (availableQuests.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, availableQuests.Count);
        return availableQuests[randomIndex];
    }

    public void UpdateQuest(QuestType type, int amount)
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            Quest quest = activeQuests[i];

            if (quest.data.questType != type)
            {
                continue;
            }

            if (quest.isCompleted)
            {
                continue;
            }

            if (quest.data.progressType == QuestProgressType.SingleRun)
            {
                if (amount >= quest.data.targetValue)
                {
                    CompleteQuest(quest);
                }

                continue;
            }

            if (quest.data.progressType == QuestProgressType.Cumulative)
            {
                quest.currentValue += amount;
                Debug.Log(quest.data.questName + ": " + quest.currentValue);


                if (quest.currentValue >= quest.data.targetValue)
                {
                    CompleteQuest(quest);
                }
                else
                {   
                    UpdateQuestTracker(quest, quest.isCompleted);
                    SaveQuests();
                }
            }

            
        }
    }

    void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
        UpdateQuestTracker(quest, quest.isCompleted);

        int questIndex = GetQuestIndex(quest.data);
        if (questIndex >= 0 && !completedQuestIndexes.Contains(questIndex))
        {
            completedQuestIndexes.Add(questIndex);
        }

        Debug.Log("Quest Complete: " + quest.data.questName);
        AddStars(quest.data.reward);

        activeQuests.Remove(quest);
        // FillEmptyQuestSlots();
        SaveQuests();
    }

    Quest CreateQuest(QuestData data)
    {
        Quest newQuest = new Quest();
        newQuest.data = data;
        return newQuest;
    }

    bool IsQuestActive(QuestData data)
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.data == data)
            {
                return true;
            }
        }

        return false;
    }

    bool IsQuestCompletedToday(QuestData data)
    {
        int questIndex = GetQuestIndex(data);
        return completedQuestIndexes.Contains(questIndex);
    }

    int GetQuestIndex(QuestData data)
    {
        for (int i = 0; i < questDatabase.Count; i++)
        {
            if (questDatabase[i] == data)
            {
                return i;
            }
        }

        return -1;
    }

    void SaveQuests()
    {
        SaveActiveQuests();
        SaveCompletedQuests();
        PlayerPrefs.Save();
    }

    void SaveActiveQuests()
    {
        for (int i = 0; i < MaxActiveQuests; i++)
        {
            if (i >= activeQuests.Count)
            {
                PlayerPrefs.DeleteKey(GetActiveQuestIndexKey(i));
                PlayerPrefs.DeleteKey(GetActiveQuestValueKey(i));
                continue;
            }

            Quest quest = activeQuests[i];
            int questIndex = GetQuestIndex(quest.data);

            PlayerPrefs.SetInt(GetActiveQuestIndexKey(i), questIndex);
            PlayerPrefs.SetInt(GetActiveQuestValueKey(i), quest.currentValue);
        }
    }

    void LoadActiveQuests()
    {
        for (int i = 0; i < MaxActiveQuests; i++)
        {
            if (!PlayerPrefs.HasKey(GetActiveQuestIndexKey(i)))
            {
                continue;
            }

            int questIndex = PlayerPrefs.GetInt(GetActiveQuestIndexKey(i));
            int currentValue = PlayerPrefs.GetInt(GetActiveQuestValueKey(i), 0);

            if (questIndex < 0 || questIndex >= questDatabase.Count)
            {
                continue;
            }

            QuestData data = questDatabase[questIndex];

            if (data == null || IsQuestActive(data) || IsQuestCompletedToday(data))
            {
                continue;
            }

            Quest quest = CreateQuest(data);
            quest.currentValue = currentValue;
            activeQuests.Add(quest);
        }
    }

    void SaveCompletedQuests()
    {
        PlayerPrefs.SetInt(CompletedQuestCountKey, completedQuestIndexes.Count);

        for (int i = 0; i < completedQuestIndexes.Count; i++)
        {
            PlayerPrefs.SetInt(GetCompletedQuestIndexKey(i), completedQuestIndexes[i]);
        }
    }

    void LoadCompletedQuests()
    {
        int completedQuestCount = PlayerPrefs.GetInt(CompletedQuestCountKey, 0);

        for (int i = 0; i < completedQuestCount; i++)
        {
            int questIndex = PlayerPrefs.GetInt(GetCompletedQuestIndexKey(i), -1);

            if (questIndex >= 0 && !completedQuestIndexes.Contains(questIndex))
            {
                completedQuestIndexes.Add(questIndex);
            }
        }
    }

    void ClearSavedActiveQuests()
    {
        for (int i = 0; i < MaxActiveQuests; i++)
        {
            PlayerPrefs.DeleteKey(GetActiveQuestIndexKey(i));
            PlayerPrefs.DeleteKey(GetActiveQuestValueKey(i));
        }
    }

    void ClearSavedCompletedQuests()
    {
        int completedQuestCount = PlayerPrefs.GetInt(CompletedQuestCountKey, 0);

        for (int i = 0; i < completedQuestCount; i++)
        {
            PlayerPrefs.DeleteKey(GetCompletedQuestIndexKey(i));
        }

        PlayerPrefs.DeleteKey(CompletedQuestCountKey);
    }

    string GetActiveQuestIndexKey(int slot)
    {
        return "QuestActiveIndex" + slot;
    }

    string GetActiveQuestValueKey(int slot)
    {
        return "QuestActiveValue" + slot;
    }

    string GetCompletedQuestIndexKey(int slot)
    {
        return "QuestCompletedIndex" + slot;
    }

    public void AddStars(int amount)
    {
        stars += amount;
        Debug.Log("Reward: " + stars + " stars");
        PlayerPrefs.SetInt("stars", stars);
        PlayerPrefs.Save();
        UpdateStarsUI(stars);
    }

    void UpdateQuestTracker (Quest quest, bool completed)
    {
        
        if (completed)
        {
            questTracker.text = 
                $"{quest.data.questName}\n" +
                $"<color=green>Quest Completed!</color>";
        }
        else
        {
            questTracker.text = 
                $"{quest.data.questName}\n" +
                $"{quest.currentValue}/{quest.data.targetValue}";
        }

        StartCoroutine(FadeQuestTracker());
    }

    IEnumerator FadeQuestTracker()
    {
        // RESET
        Color color = questTracker.color;
        questTracker.color = new Color(color.r, color.g, color.b, 1f);

        yield return new WaitForSeconds(2f);
        
        // FADE OUT
        float alpha = 1f;
        float fadeSpeed = 2f;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            questTracker.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // CLEANUP
        questTracker.text = string.Empty;
    }

    void UpdateQuestUI ()
    {
        for (int i = 0; i < QuestNameText.Length; i++)
        {
            if (i >= activeQuests.Count)
            {
                QuestNameText[i].text = string.Empty;
                QuestRewardText[i].text = string.Empty;
                QuestImage[i].enabled = false;
                RewardImage[i].enabled = false;
                continue;
            }

            Quest quest = activeQuests[i];
            QuestNameText[i].text = quest.data.questName;
            QuestRewardText[i].text = quest.data.reward.ToString();
        }

    }

    void UpdateStarsUI(int value)
    {
        starsOnQuestLog.text = value.ToString();
        starsOnShop.text = value.ToString();
        shopManager.UpdateShopUI();
    }

     public int GetStars()
    {
        return stars;
    }

    [ContextMenu ("Reset")]
    void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    } 
}
