using UnityEngine;
using System.Collections;

public class Repulsion : MonoBehaviour
{
    private TroopStats _currentTroopStats;
    private LevelStageController _levelStageController;

    private const float PUNCH_RELOAD = 0.5f;
    private const float PUNCH_STRENGTH = 75f;

    private bool _isCanPunch;


    private void Awake()
    {
        _levelStageController = FindObjectOfType(typeof(LevelStageController)) as LevelStageController;
            
        _currentTroopStats = GetComponentInParent<TroopStats>();
    }


    private void Start()
    {
        _isCanPunch = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TroopStats otherTroopStats) && _levelStageController.GetCurrentStage == LevelStage.CollectingFood)
        {
            StartBeingRepulsed(otherTroopStats);
        }
    }


    private void StartBeingRepulsed(TroopStats otherTroopStats)
    {
        Rigidbody otherRB = otherTroopStats.GetComponent<Rigidbody>();
        Rigidbody currentRB = GetComponentInParent<Rigidbody>();

        float currentWeight = _currentTroopStats.GetCurrentWeight;
        float otherWeight = otherTroopStats.GetCurrentWeight;

        float differenceBeetwenTroops = currentWeight - otherWeight;

        if (differenceBeetwenTroops > 0 && _isCanPunch && otherTroopStats.TroopType != _currentTroopStats.TroopType)
        {
            Vector3 direction = otherRB.position - currentRB.position;

            _isCanPunch = false;
            direction.y = 0;

            otherRB.AddForce(force: direction.normalized * PUNCH_STRENGTH, ForceMode.VelocityChange);
        }

        StartCoroutine(AllowPunchDelay());
    }


    private IEnumerator AllowPunchDelay()
    {
        yield return new WaitForSeconds(PUNCH_RELOAD);

        _isCanPunch = true;
    }
}
