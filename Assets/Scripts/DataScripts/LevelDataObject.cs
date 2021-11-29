using UnityEngine;

[CreateAssetMenu(fileName = "LevelData.asset", menuName = "Ragdoll/Level Configuration")]
public class LevelDataObject : ScriptableObject
{
    public LevelType LevelType;

    public float CollectingFoodStageDuration;
    public float SpawnCooldown;

    public int EnemiesCount;
    public int AlliesCount;
    [Header("Food, ragdoll etc")]
    public int ObjectCount;

    [Range(1, 10)]
    public int IQCoefficient;

    public float FightMoveSpeed;

    [Range(1, 8)]
    public float FieldSize;
}
