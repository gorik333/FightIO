using UnityEngine;

public class Food : MonoBehaviour, IFoundable
{
	[Header( "Common food" )]
	[SerializeField]
	private FoodData[] _commonFoodData;

	[Header( "Ragdoll food" )]
	[SerializeField]
	private FoodData[] _ragdollFoodData;

	private const float RAGDOLL_FOOD_CHANCE = 15; // 15 out of 100 percent

	private FoodType _foodType;

	private float _growFactor;
	private float _foodWeight;


	private void Start()
	{
		RandomFoodType();
	}


	private void RandomFoodType()
	{
		int randomFoodType = Random.Range( 0, 100 );

		if (randomFoodType <= RAGDOLL_FOOD_CHANCE)
		{
			int randomRagdollFood = Random.Range( 0, _ragdollFoodData.Length );
			InitializeFood( _ragdollFoodData[randomRagdollFood] );
		}
		else
		{
			int randomCommonFood = Random.Range( 0, _commonFoodData.Length );
			InitializeFood( _commonFoodData[randomCommonFood] );
		}
	}


	private void InitializeFood( FoodData currentFood )
	{
		_foodType = currentFood.CurrentFoodType;
		_growFactor = currentFood.GrowFactor;
		_foodWeight = currentFood.FoodWeight;

		Instantiate( currentFood.gameObject, transform ); // ??  --  Уже понял
	}


	public void DestroyFood()
	{
		Destroy( gameObject );
	}


	public Transform GetTransform()
	{
		return transform;
	}


	public float GetFoodWeight => _foodWeight;

	public FoodType GetFoodType => _foodType;

	public float GetFoodGrowFactor => _growFactor;
}
