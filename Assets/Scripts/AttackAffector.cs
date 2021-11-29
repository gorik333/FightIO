using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AttackAffector : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private AttackAffector _currentTarget;

    private TroopStats _troopStats;
    private TroopMovement _troopMovement;
    private AnimatorController _animatorController;
    private TroopParticlesController _troopParticlesController;

    private const float TIME_BEETWEN_ATTACK = 0.65f;
    private const float DAMAGE = 2.5f;
    private const float STOP_DISTANCE = 1.5f;

    private TroopType _troopType;

    private float _fightMoveSpeed;

    private int _previousAnimID;
    private int _currentAnimID;

    private bool _isDead;


    private void Start()
    {
        _troopStats = GetComponent<TroopStats>();
        _troopMovement = GetComponent<TroopMovement>();

        _animatorController = GetComponentInChildren<AnimatorController>();
        _troopParticlesController = GetComponentInChildren<TroopParticlesController>();

        SetUp();
    }


    private void SetUp()
    {
        _previousAnimID = -1;
    }


    public void StartFighting()
    {
        _troopType = _troopStats.TroopType;

        _fightMoveSpeed = Game.Instance.GetCurrentLevelData.GetFightMoveSpeed;

        _currentHealth = _troopStats.GetCurrentWeight;

        _animatorController.StartStandUpAnim();

        StartCoroutine(StartSearchDelay());
    }


    private IEnumerator StartSearchDelay()
    {
        yield return new WaitForSeconds(_animatorController.GetCurrentAnimDuration() + 0.5f);

        FindNearestTarget();
    }


    private IEnumerator MovingToTarget()
    {
        _animatorController.StartAnim(AnimationType.FastRun);

        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (_currentTarget == null || _currentTarget.IsDead)
            {
                FindNearestTarget();

                break;
            }
            if (_currentTarget != null)
            {
                _animatorController.transform.DOLocalRotate(Vector3.zero, 0.4f); // 0.1f default
                _troopMovement.MoveTroopTransform(transform, _currentTarget.transform.position, _fightMoveSpeed);
            }

            if (!CheckDistanceBetweenTargets())
            {
                _animatorController.StopAnim(AnimationType.FastRun);

                StopAllCoroutines();
                StartCoroutine(AttackWithDelay());

                break;
            }
        }
    }


    private bool CheckDistanceBetweenTargets()
    {
        float distance = Vector3.Distance(transform.position, _currentTarget.transform.position);

        if (distance >= STOP_DISTANCE)
        {

            return true;
        }

        return false;
    }


    public void FindNearestTarget()
    {
        _currentTarget = TroopSpawner.Instance.FindTarget(this);

        if (_currentTarget != null && !_currentTarget.IsDead)
        {
            if (CheckDistanceBetweenTargets())
            {
                StopAllCoroutines();
                StartCoroutine(MovingToTarget());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(AttackWithDelay());
            }
        }
        else if (_currentTarget == null && !_isDead)
        {
            _animatorController.StartAnim(AnimationType.Victory);
        }
    }


    private IEnumerator AttackWithDelay()
    {
        _animatorController.StopAnim(AnimationType.FastRun);

        while (true)
        {
            RotateToEnemy();

            StartRandomPunchAnim();

            if (CheckDistanceBetweenTargets())
            {
                StopAllCoroutines();
                StartCoroutine(MovingToTarget());
            }

            yield return new WaitForSeconds(TIME_BEETWEN_ATTACK);

            if (_currentTarget != null)
                _currentTarget.TakeDamage(DAMAGE);

            if (_currentTarget == null || _currentTarget.IsDead)
            {
                FindNearestTarget();

                break;
            }
        }
    }


    private void RotateToEnemy()
    {
        if (_currentTarget != null)
        {
            _troopMovement.RotateToEnemyTransform(transform, _currentTarget.transform);
            _animatorController.transform.DOLocalRotate(Vector3.zero, 0.2f); // 0.1f
        }
    }


    private void StartRandomPunchAnim() // move to animator controller
    {
        _previousAnimID = _currentAnimID;

        while (true)
        {
            _currentAnimID = Random.Range(0, AnimatorController.PUNCH_ANIM_COUNT);

            if (_currentAnimID != _previousAnimID)
                break;
        }

        switch (_currentAnimID)
        {
            case 0:
                _animatorController.StartAnim(AnimationType.RightPunch);
                break;
            case 1:
                _animatorController.StartAnim(AnimationType.LeftPunch);
                break;
            case 2:
                _animatorController.StartAnim(AnimationType.CrossPunch);
                break;
            case 3:
                _animatorController.StartAnim(AnimationType.RightLegKick);
                break;
            default:
                Debug.LogWarning("Punch count too big");
                _animatorController.StartAnim(AnimationType.RightPunch);
                break;
        }
    }


    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        CheckIfDied();
    }


    private void CheckIfDied()
    {
        if (_currentHealth <= 0f)
        {
            StopAllCoroutines();

            Die();
        }
    }


    private void Die()
    {
        _animatorController.StartAnim(AnimationType.Death);
        _troopParticlesController.SpawnDeathParticles(_animatorController.GetCurrentAnimDuration());
        _isDead = true;

        TroopSpawner.Instance.RemoveKilledTroop(this);

        Destroy(this);

        Destroy(gameObject, _animatorController.GetCurrentAnimDuration() + 0.1f);
    }


    public TroopType GetTroopType => _troopType;

    public float GetCurrentHealth => _currentHealth;

    public bool IsDead => _isDead;
}
