using UnityEngine;

public enum QuestType
{
    TotalDistance,
    ReachDistance,
    AvoidEnemies,
    PlayGames,
    CollectFuel,
    AchieveHighScore, 
    UseShield,
    UseFuelMagnet,
    UseExtraLife,
    UseFuelBoost
}

public enum QuestProgressType
{
    SingleRun,
    Cumulative
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable Objects/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public string questName;
    public QuestType questType;

    [Header("Progress")]
    public QuestProgressType progressType;

    [Header("Quest Values")]
    public int targetValue;
    public int reward;
    public bool isDaily;
}
