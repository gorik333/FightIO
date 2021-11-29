using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LevelStage
{
    Menu,
    CollectingFood,
    PreparationForBattle,
    Battle,
    BattleEnd
}


public class LevelStageController : MonoBehaviour
{
    [SerializeField]
    private LevelStage _currentLevelStage;

    [SerializeField]
    private TroopSpawner _troopSpawner;

    [SerializeField]
    private Game _game;

    [SerializeField]
    private GameUI _gameUI;

    [SerializeField]
    private Transform _fightCenter;

    private const float PREPARATION_STAGE_DURATION = 1.75f;

    private float _collectFoodTimer;

    public static LevelStageController Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        _currentLevelStage = LevelStage.Menu;
    }


    private void Update()
    {
        FoodTimer();
    }


    public void StartLevel(LevelData levelData)
    {
        SetCollectionFoodStage();

        _collectFoodTimer = levelData.GetCollectingFoodStageDuration;
    }


    private void FoodTimer()
    {
        if (_currentLevelStage == LevelStage.CollectingFood)
        {
            _collectFoodTimer -= Time.deltaTime;
            _game.UpdateUITimer((int)_collectFoodTimer);

            if (_collectFoodTimer <= 1) // one second left, and afterthat starts preparation stage
                SetPreparationStage();
        }
    }


    public void SetMenuStage()
    {
        CinemachineExtension.Instance.SetMenuOffset();

        _currentLevelStage = LevelStage.Menu;
    }


    private void SetCollectionFoodStage()
    {
        _currentLevelStage = LevelStage.CollectingFood;

        _gameUI.TurnOnFoodStageUI();
    }


    private void SetPreparationStage()
    {
        _currentLevelStage = LevelStage.PreparationForBattle;

        _gameUI.UpdateUIBattleMsg(_currentLevelStage);

        Spawner.Instance.ClearMapFromFood();

        StartCoroutine(StartBattleStageDelay());
    }


    private void SetBattleStage()
    {
        _currentLevelStage = LevelStage.Battle;

        _gameUI.UpdateUIBattleMsg(_currentLevelStage);

        CinemachineExtension.Instance.SetFollow(_fightCenter, GameOffset.BattleOffset);
        CinemachineExtension.Instance.ChangeOffset(GameOffset.BattleOffset);

        _troopSpawner.StartTroopsFighting();
    }


    private IEnumerator StartBattleStageDelay()
    {
        yield return new WaitForSeconds(PREPARATION_STAGE_DURATION);

        SetBattleStage();
    }


    public float GetCollectFoodTimer => _collectFoodTimer;

    public LevelStage GetCurrentStage => _currentLevelStage;
}
