using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private TroopMovement _troopMovement;

    [SerializeField]
    private float _moveSpeed;

    private Rigidbody _playerRB;

    private LevelStageController _stageController;

    private Vector3 _direction;

    private float _multiplier;


    private void Awake()
    {
        _stageController = FindObjectOfType(typeof(LevelStageController)) as LevelStageController;
    }


    void Start()
    {
        SetUp();

        SetCinemachineFollow();
    }


    private void SetCinemachineFollow()
    {
        CinemachineExtension.Instance.SetFollow(transform, GameOffset.CollectFoodOffset);
    }


    private void SetUp()
    {
        if (_moveSpeed == 0)
            _moveSpeed = 1;

        _playerRB = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if (_stageController.GetCurrentStage == LevelStage.CollectingFood)
            _troopMovement.MoveTroopForBoth(_direction, _playerRB, _moveSpeed * _multiplier, true);
    }


    public void ResetMovement()
    {
        _direction = Vector3.zero;
        _multiplier = 0;
    }


    public void Move(Vector2 direction, float distanceMultiplier)
    {
        _direction = new Vector3(direction.x, 0, direction.y);
        _multiplier = distanceMultiplier;
    }
}
