using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TroopSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _allyPrefab;

    [SerializeField]
    private GameObject _playerPrefab;

    private List<AttackAffector> _troop;

    private const float DELAY_BEFORE_GAME_END = 1.5f;

    public static TroopSpawner Instance { get; private set; }

    private int _enemiesCount;
    private int _alliesCount;

    private bool _isEnded;


    private void Awake()
    {
        Instance = this;

        _troop = new List<AttackAffector>();
    }


    public void SetUp(LevelData levelData)
    {
        _enemiesCount = levelData.GetEnemiesCount;
        _alliesCount = levelData.GetAlliesCount;

        _isEnded = false;
    }

    /// <summary>
    /// Calls when game reset, cleans all troops from map and re-create enemy list
    /// </summary>
    public void CleanMapFromTroops()
    {
        for (int i = 0; i < _troop.Count; i++)
            Destroy(_troop[i].gameObject);

        _troop = new List<AttackAffector>();
    }


    public void SpawnTroops()
    {
        Spawn(_alliesCount, _allyPrefab);
        Spawn(_enemiesCount, _enemyPrefab);

        SpawnPlayer();
    }

    /// <summary>
    /// Spawn certain troops
    /// </summary>
    /// <param name="count">Count of troops</param>
    /// <param name="troop">Troop prefab</param>
    private void Spawn(int count, GameObject troop)
    {
        for (int i = 0; i < count; i++)
            _troop.Add(Instantiate(troop, RandomPoint.Instance.GetRandomPoint(), Quaternion.identity, transform).GetComponent<AttackAffector>());
    }


    private void SpawnPlayer()
    {
        Player player = Instantiate(_playerPrefab, RandomPoint.Instance.GetRandomPoint(), Quaternion.identity, transform).GetComponent<Player>();

        _troop.Add(player.GetComponent<AttackAffector>());

        player.transform.position = Vector3.zero;

        Game.Instance.InitializePlayerMovement(player);
    }


    public AttackAffector FindTarget(AttackAffector seeker)
    {
        if (seeker.GetTroopType == TroopType.Ally)
        {
            List<AttackAffector> enemy = GetCertainTroopType(_troop, TroopType.Enemy);

            return GetNearestTarget(enemy, seeker);
        }
        else if (seeker.GetTroopType == TroopType.Enemy)
        {
            List<AttackAffector> ally = GetCertainTroopType(_troop, TroopType.Ally);

            return GetNearestTarget(ally, seeker);
        }

        return null;
    }


    private AttackAffector GetNearestTarget(List<AttackAffector> troop, AttackAffector seeker)
    {
        AttackAffector nearestTarget = null;

        float minDistance = float.MaxValue;

        for (int j = 0; j < troop.Count; j++)
        {
            float currentDistance = Vector3.Distance(seeker.transform.position, troop[j].transform.position);

            if (currentDistance < minDistance && !troop[j].IsDead)
            {
                minDistance = currentDistance;
                nearestTarget = troop[j];
            }
        }

        StartCoroutine(EndGameDelay(nearestTarget, seeker));

        return nearestTarget;
    }


    private IEnumerator EndGameDelay(AttackAffector nearestTarget, AttackAffector seeker)
    {
        if (nearestTarget == null && !_isEnded)
        {
            _isEnded = true;

            yield return new WaitForSeconds(DELAY_BEFORE_GAME_END);

            if (seeker.GetTroopType == TroopType.Ally)
                Game.Instance.onFinish();
            else if (seeker.GetTroopType == TroopType.Enemy)
                Game.Instance.onGameOver();
        }
    }


    public void StartTroopsFighting()
    {
        for (int i = 0; i < _troop.Count; i++)
            _troop[i].StartFighting();

        CalcTroopsStartPosition();
    }


    private void CalcTroopsStartPosition()
    {
        List<AttackAffector> ally = GetCertainTroopType(_troop, TroopType.Ally);
        List<AttackAffector> enemy = GetCertainTroopType(_troop, TroopType.Enemy);

        List<Vector3> allyStartPosition = TriangleGenerator.Instance.GenerateTriangle(ally.Count, TriangleSpawnPosition.RightSide);
        List<Vector3> enemyStartPosition = TriangleGenerator.Instance.GenerateTriangle(enemy.Count, TriangleSpawnPosition.LeftSide);

        MoveTroopsToStartPosition(ally, allyStartPosition);
        MoveTroopsToStartPosition(enemy, enemyStartPosition);
    }


    private void MoveTroopsToStartPosition(List<AttackAffector> troop, List<Vector3> position)
    {
        for (int i = 0; i < troop.Count; i++)
        {
            troop[i].GetComponent<TroopMovement>().MoveToStartPosition(troop[i].transform, position[i]);
        }
    }


    private List<AttackAffector> GetCertainTroopType(List<AttackAffector> troop, TroopType neededTroopType)
    {
        List<AttackAffector> result = new List<AttackAffector>();

        for (int i = 0; i < troop.Count; i++)
        {
            if (troop[i].GetTroopType == neededTroopType)
                result.Add(troop[i]);
            else if (troop[i].GetTroopType == neededTroopType)
                result.Add(troop[i]);
        }

        return result;
    }


    public void AddTakenToArmyTroop(AttackAffector affector)
    {
        if (!_troop.Contains(affector))
            _troop.Add(affector);
    }


    public void RemoveKilledTroop(AttackAffector affector)
    {
        if (_troop.Contains(affector))
            _troop.Remove(affector);
    }
}
