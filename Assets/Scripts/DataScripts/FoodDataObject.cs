using UnityEngine;

[CreateAssetMenu(fileName = "FoodData.asset", menuName = "Ragdoll/Food Configuration")]
public class FoodDataObject : ScriptableObject
{
    public FoodType CurrentFoodType;

    public float FoodWeight;

    public float GrowFactor;
}
