using UnityEngine;

public class TakeSpawnedObject : MonoBehaviour
{
    [SerializeField]
    private bool _isPlayer;

    private TroopStats _troopStats;
    private SummonFinder _summonFinder;


    private void Start()
    {
        _summonFinder = transform.parent.GetComponentInChildren<SummonFinder>();

        _troopStats = GetComponentInParent<TroopStats>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (LevelStageController.Instance.GetCurrentStage == LevelStage.CollectingFood)
        {
            CheckIfFood(other);
            CheckIfNeutral(other);
        }
    }


    private void CheckIfFood(Collider other)
    {
        if (other.GetComponentInParent<Food>() != null)
        {
            Food food = other.GetComponentInParent<Food>();

            _troopStats.Eat(food);
            RemoveEatenSummon(food);

            food.DestroyFood();

            if (_isPlayer)
                GameUI.Instance.UpdateUIAddEatenFood();
        }
    }


    private void CheckIfNeutral(Collider other)
    {
        if (other.GetComponentInParent<Neutral>() != null)
        {
            Neutral neutral = other.GetComponentInParent<Neutral>();

            RemoveEatenSummon(neutral);

            neutral.TakeToArmy(transform.parent, _troopStats.TroopType);
        }
    }


    private void RemoveEatenSummon(IFoundable summon)
    {
        if (_summonFinder != null)
            _summonFinder.RemoveEatenSummon(summon);
    }
}
