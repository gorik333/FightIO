using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField]
    private LevelDataObject _levelDataObject;


    public LevelType GetLevelType => _levelDataObject.LevelType;

    public float GetCollectingFoodStageDuration => _levelDataObject.CollectingFoodStageDuration;

    public float GetFightMoveSpeed => _levelDataObject.FightMoveSpeed;

    public float GetSpawnCooldown => _levelDataObject.SpawnCooldown;

    public int GetEnemiesCount => _levelDataObject.EnemiesCount;

    public int GetAlliesCount => _levelDataObject.AlliesCount;

    public int GetFoodCount => _levelDataObject.ObjectCount;

    public int GetIQCoefficient => _levelDataObject.IQCoefficient;

    public float GetFieldSize => _levelDataObject.FieldSize;
}
