using System.Collections.Generic;
using UnityEngine;

public class SummonFinder : MonoBehaviour
{
	[SerializeField]
	private IFoundable _currentSummon;

	private List<IFoundable> _summonInArea;

	private TroopAI _troopAI;
	private SphereCollider _sphereCollider;

	private int _IQCoefficient;

	private const float SPHERE_RADIUS = 0.75f;


	private void Start()
	{
		_summonInArea = new List<IFoundable>();

		_troopAI = GetComponentInParent<TroopAI>();

		_sphereCollider = GetComponent<SphereCollider>();

		SetSphereRadius();
		SetIQCoefficient();
	}


	private void SetSphereRadius()
	{
		_sphereCollider.radius = SPHERE_RADIUS;
	}


	private void SetIQCoefficient()
	{
		_IQCoefficient = Game.Instance.GetCurrentLevelData.GetIQCoefficient;

		_sphereCollider.radius *= _IQCoefficient;
	}


	private void OnTriggerEnter( Collider other )
	{
		if (other.GetComponentInParent<IFoundable>() != null)
		{
			IFoundable summon = other.GetComponentInParent<IFoundable>();
			if (!_summonInArea.Contains( summon ))
			{
				_summonInArea.Add( summon );
				ChooseNearestFood();
			}
		}
	}


	private void OnTriggerExit( Collider other )
	{
		if (other.GetComponentInParent<IFoundable>() != null)
		{
			IFoundable summon = other.GetComponentInParent<IFoundable>();
			if (_summonInArea.Contains( summon ))
			{
				_summonInArea.Remove( summon );
				ChooseNearestFood();
			}
		}
	}


	private void ChooseNearestFood()
	{
		float minDistance = float.MaxValue;
		IFoundable nearestFood = null;

		for (int i = 0; i < _summonInArea.Count; i++)
		{
			if (_summonInArea[i] == null)
			{
				RemoveEatenSummon( _summonInArea[i] );

				continue;
			}

			float currentDist = Vector3.Distance( transform.position, _summonInArea[i].GetTransform().position);

			if (currentDist < minDistance)
			{
				nearestFood = _summonInArea[i];
				minDistance = currentDist;
			}
		}

		if (nearestFood != null)
		{
			_currentSummon = nearestFood;
			_troopAI.SetFoodFindingState();
		}
		else
			_troopAI.SetRoamingState();
	}


	public void RemoveEatenSummon( IFoundable summon )
	{
		if (_summonInArea.Contains( summon ))
		{
			_summonInArea.Remove( summon );

			ChooseNearestFood();
		}
	}


	public IFoundable GetCurrentSummon()
	{
		if (_currentSummon != null)
			return _currentSummon;
		else
			return null;
	}
}
