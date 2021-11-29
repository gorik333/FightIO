using UnityEngine;

public enum TroopState
{
	Roaming,
	GoingToFood
}


public class TroopAI : MonoBehaviour
{
	[SerializeField]
	private float _moveSpeed;

	[SerializeField]
	private GameObject _testPrefab;

	[SerializeField]
	private TroopState _troopState;

	private TroopMovement _troopMovement;
	private SummonFinder _summonFinder;
	private LevelStageController _levelStageController;

	private Rigidbody _troopRB;

	private Vector3 _currentDestinationPoint;
	private Vector3 _direction;

	private bool _isDestinationPointSet;

	private const float BASE_MOVE_SPEED = 295f;
	private const float DISTANCE_TO_CHANGE_DIRECTION = 2f;


	private void Awake()
	{
		_levelStageController = FindObjectOfType( typeof( LevelStageController ) ) as LevelStageController;
	}


	private void Start()
	{
		_troopState = TroopState.Roaming;

		_troopMovement = GetComponent<TroopMovement>();
		_summonFinder = GetComponentInChildren<SummonFinder>();
		_troopRB = GetComponent<Rigidbody>();

		SetUp();
	}


	private void SetUp()
	{
		if (_moveSpeed == 0)
			_moveSpeed = BASE_MOVE_SPEED;
	}


	private void FixedUpdate()
	{
		if (_levelStageController.GetCurrentStage == LevelStage.CollectingFood)
		{
			if (_troopState == TroopState.Roaming)
			{
				GoToRandomPosition();
				CheckIfDestinationPointReached();
			}
			if (_troopState == TroopState.GoingToFood)
				GoToFood();
		}
	}


	private void GoToFood()
	{
		if (_summonFinder.GetCurrentSummon().GetTransform() == null)
		{
			_direction = ( _summonFinder.GetCurrentSummon().GetTransform().position - transform.position ).normalized;
			_troopMovement.MoveTroopForBoth( _direction, _troopRB, _moveSpeed, false );
		}
		else
			SetRoamingState();
	}


	private void GoToRandomPosition()
	{
		if (_isDestinationPointSet)
		{
			_direction = ( _currentDestinationPoint - transform.position ).normalized;
			_troopMovement.MoveTroopForBoth( _direction, _troopRB, _moveSpeed, false );
		}
		else
			SetDestinationPoint();
	}


	public void SetFoodFindingState()
	{
		_troopState = TroopState.GoingToFood;
	}


	public void SetRoamingState()
	{
		_troopState = TroopState.Roaming;
	}


	private void CheckIfDestinationPointReached()
	{
		if (Vector3.Distance( transform.position, _currentDestinationPoint ) < DISTANCE_TO_CHANGE_DIRECTION)
			SetDestinationPoint();
	}


	private void SetDestinationPoint()
	{
		_currentDestinationPoint = RandomPoint.Instance.GetRandomPoint();
		_isDestinationPointSet = true;
	}


	public float GetMoveSpeed => _moveSpeed;

	public Vector3 GetDestinationPoint => _currentDestinationPoint;

	public Vector3 GetDirection => _direction;
}