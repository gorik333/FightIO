using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Class references")]
    [SerializeField]
    private LevelStageController _levelStageController;

    [Header("Objects for spawn")]
    [SerializeField]
    private GameObject _foodPrefab;

    [SerializeField]
    private GameObject _neutralPrefab;

    private float _cooldown;
    private float _currentCooldown;

    private int _maxCount;
    private int _currentCount;

    private LevelType _currentLevelType;


    private List<GameObject> _spawnedItemsList;

    public static Spawner Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        _spawnedItemsList = new List<GameObject>();
    }


    void Update()
    {
        if (_levelStageController.GetCurrentStage == LevelStage.CollectingFood)
        {
            if (_currentLevelType == LevelType.CommonLevel)
            {
                SpawnFoodTimer();
            }
            else if (_currentLevelType == LevelType.MakeArmyLevel)
            {
                SpawnNeutralTimer();
            }
        }
    }


    public void SetUp(LevelData levelData)
    {
        _currentLevelType = levelData.GetLevelType;

        _cooldown = levelData.GetSpawnCooldown;
        _maxCount = levelData.GetFoodCount;
        _currentCount = 0;
    }


    private void SpawnFoodTimer()
    {
        _currentCooldown -= Time.deltaTime;

        if (_currentCooldown <= 0 && _currentCount < _maxCount)
        {
            SpawnFood();
            _currentCooldown = _cooldown;
            _currentCount++;
        }
    }


    private void SpawnNeutralTimer()
    {
        _currentCooldown -= Time.deltaTime;

        if (_currentCooldown <= 0 && _currentCount < _maxCount)
        {
            SpawnNeutral();
            _currentCooldown = _cooldown;
            _currentCount++;
        }
    }


    private void SpawnNeutral()
    {
        GameObject neutral = Instantiate(_neutralPrefab, RandomPoint.Instance.GetRandomPoint(), Quaternion.identity, transform);

        _spawnedItemsList.Add(neutral);
    }


    private void SpawnFood()
    {
        GameObject food = Instantiate(_foodPrefab, RandomPoint.Instance.GetRandomPoint(), Quaternion.identity, transform);

        _spawnedItemsList.Add(food);
    }


    public void RemoveSummon(GameObject item )
	{
        _spawnedItemsList.RemoveAt( _spawnedItemsList.IndexOf(item));
	}


    public void ClearMapFromFood()
    {
        for (int i = 0; i < _spawnedItemsList.Count; i++)
        {
            Destroy(_spawnedItemsList[i]);
        }

        for (int i = 0; i < _spawnedItemsList.Count; i++)
        {
            _spawnedItemsList.RemoveAt(i);
        }
    }
}
