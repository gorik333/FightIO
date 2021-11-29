using UnityEngine;

public class Neutral : MonoBehaviour, IFoundable
{
	[SerializeField]
	private TroopMovement _troopMovement;

	[SerializeField]
	private TroopStats _troopStats;

	[SerializeField]
	private AttackAffector _attackAffector;

	[SerializeField]
	private Rigidbody _RB;

	[SerializeField]
	private SkinnedMeshRenderer _skinnedMeshRenderer;

	[SerializeField]
	private Material _enemyMaterial;

	[SerializeField]
	private Material _allyMaterial;

	private Transform _follower;

	private const float MOVE_SPEED = 315f;
	private const float FOLLOW_DISTANCE_OFFSET = 0.1f;

	private bool _isTaken;


	private void FixedUpdate()
	{
		if (_follower != null)
			Move();
	}


	private void Move()
	{
		if (Vector3.Distance( _follower.position, transform.position ) > FOLLOW_DISTANCE_OFFSET)
		{
			Vector3 direction = ( _follower.position - transform.position ).normalized;
			_troopMovement.MoveTroopForBoth( direction, _RB, MOVE_SPEED, false );
		}
	}


	public void TakeToArmy( Transform transform, TroopType troopType )
	{
		if (!_isTaken && troopType != TroopType.Neutral)
		{
			_follower = transform;
			_isTaken = true;
			_troopStats.TroopType = troopType;

			switch (troopType)
			{
				case TroopType.Ally:
					_skinnedMeshRenderer.material = _allyMaterial;
					break;
				case TroopType.Enemy:
					_skinnedMeshRenderer.material = _enemyMaterial;
					break;
			}

			TroopSpawner.Instance.AddTakenToArmyTroop( GetComponent<AttackAffector>() );
		}
	}


	public Transform GetTransform()
	{
		return transform;
	}
}
