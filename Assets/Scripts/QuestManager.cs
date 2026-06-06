using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    const int MaxActiveQuests = 10;

    const string LastDateKey = "QuestLastDate";

    [Header("All Available Quests")]
    public List<QuestData> questDatabase;

    [Header("Active Quests")]
    public List<Quest> activeQuests = new List<Quest>();
    
    [Header("Update Quests UI")]
    [SerializeField] TextMeshProUGUI questTracker;
    [SerializeField] TextMeshProUGUI[] QuestNameText;
    [SerializeField] TextMeshProUGUI[] QuestRewardText;
    [SerializeField] Image[] QuestImage;
    [SerializeField] TextMeshProUGUI[] QuestProgressText;
    private float highScore;
    
    [Header("Update Stars")]
    int stars;
    [SerializeField] TextMeshProUGUI starsOnQuestLog;
    [SerializeField] TextMeshProUGUI starsOnShop;
    [SerializeField] ShopManager shopManager;
    private AudioManager audioManager;

    void Awake()
    {
        stars = PlayerPrefs.GetInt ("stars", 0);
        UpdateStarsUI(stars); 
        highScore = PlayerPrefs.GetFloat("maxDistance", 0);
    }

    void Start()
    {   
        audioManager = FindFirstObjectByType<AudioManager>();
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

        PlayerPrefs.SetString(LastDateKey, today);
        ClearSavedActiveQuests();

        FillEmptyQuestSlots();
        SaveQuests();
    }

    void ContinueToday()
    {
        activeQuests.Clear();

        LoadActiveQuests();

        FillEmptyQuestSlots();
        SaveQuests();
    }

    void FillEmptyQuestSlots()
    {
        while (activeQuests.Count < MaxActiveQuests)
        {
            QuestData randomQuestData = GetRandomQuest();

            if (randomQuestData == null)
            {
                return;
            }

            activeQuests.Add(CreateQuest(randomQuestData));
        }
    }

    QuestData GetRandomQuest()
    {
        List<QuestData> availableQuests = new List<QuestData>();

        foreach (QuestData data in questDatabase)
        {
            if (data == null)
            {
                continue;
            }

            if (IsQuestActive(data))
            {
                continue;
            }

            if (data.questType == QuestType.AchieveHighScore)
            {   
                if (highScore == 0)
                {   
                    print ("No highscore quest!");
                    continue;
                }
                else
                {
                    data.targetValue = Mathf.RoundToInt(highScore); 
                    print(data.targetValue);
                }
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
        audioManager.PlayQuestCompletedSFX();

        Debug.Log("Quest Complete: " + quest.data.questName);
        AddStars(quest.data.reward);

        UpdateQuestUI();
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
                PlayerPrefs.DeleteKey(GetActiveQuestCompletedKey(i));
                continue;
            }

            Quest quest = activeQuests[i];
            int questIndex = GetQuestIndex(quest.data);

            PlayerPrefs.SetInt(GetActiveQuestIndexKey(i), questIndex);
            PlayerPrefs.SetInt(GetActiveQuestValueKey(i), quest.currentValue);
            PlayerPrefs.SetInt(GetActiveQuestCompletedKey(i), quest.isCompleted ? 1 : 0);
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
            bool isCompleted = PlayerPrefs.GetInt(GetActiveQuestCompletedKey(i), 0) == 1;

            if (questIndex < 0 || questIndex >= questDatabase.Count)
            {
                continue;
            }

            QuestData data = questDatabase[questIndex];

            if (data == null || IsQuestActive(data))
            {
                continue;
            }

            Quest quest = CreateQuest(data);
            quest.currentValue = currentValue;
            quest.isCompleted = isCompleted;
            activeQuests.Add(quest);
        }
    }

    void ClearSavedActiveQuests()
    {
        for (int i = 0; i < MaxActiveQuests; i++)
        {
            PlayerPrefs.DeleteKey(GetActiveQuestIndexKey(i));
            PlayerPrefs.DeleteKey(GetActiveQuestValueKey(i));
            PlayerPrefs.DeleteKey(GetActiveQuestCompletedKey(i));
        }
    }

    string GetActiveQuestIndexKey(int slot)
    {
        return "QuestActiveIndex" + slot;
    }

    string GetActiveQuestValueKey(int slot)
    {
        return "QuestActiveValue" + slot;
    }

    string GetActiveQuestCompletedKey(int slot)
    {
        return "QuestActiveCompleted" + slot;
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

        yield return new WaitForSeconds(1.0f);
        
        // FADE OUT
        float alpha = 1f;
        float fadeSpeed = 2.0f;

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
                SetQuestSlotCompletedColor(i, false);
                continue;
            }

            Quest quest = activeQuests[i];

            if (quest.isCompleted)
            {
                QuestProgressText[i].text = "Completed!";
                SetQuestSlotCompletedColor(i, true);
            }
            else
            {
                QuestProgressText[i].text = $"> {quest.currentValue}/{quest.data.targetValue}";
                SetQuestSlotCompletedColor(i, false);
            }

            QuestNameText[i].text = quest.data.questName;
            QuestRewardText[i].text = quest.data.reward.ToString();
        }

    }

    void SetQuestSlotCompletedColor(int index, bool completed)
    {
        if (index >= QuestImage.Length)
        {
            return;
        }

        if (completed)
        {
            QuestImage[index].color = new Color(1f, 1f, 1f, 1f);
            QuestNameText[index].color = new Color(5f/225f, 5f/255f, 5f/255f, 255f/255f);
            QuestProgressText[index].color = new Color(5f/225f, 5f/255f, 5f/255f, 255f/255f);
        }
        else
        {
            QuestImage[index].color = new Color(5f/225f, 5f/255f, 5f/255f, 255f/255f);
            QuestNameText[index].color = new Color(1f, 1f, 1f, 1f);
            QuestProgressText[index].color = new Color(1f, 1f, 1f, 1f);
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
