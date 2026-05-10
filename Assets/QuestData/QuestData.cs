using UnityEngine;

public enum QuestType
{
    TotalDistance,
    ReachDistance,
    AvoidEnemies,
    PlayGames,
    CollectFuel,
    AchieveHighScore
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable Objects/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public string questName;
    public QuestType questType;

    [Header("Quest")]
    public int targetValue;
    public int reward;
    public bool isDaily;
}
