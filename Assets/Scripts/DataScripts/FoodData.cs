using UnityEngine;

public class FoodData : MonoBehaviour
{
    [SerializeField]
    private FoodDataObject _foodDataObject;


    public float FoodWeight => _foodDataObject.FoodWeight;


    public float GrowFactor => _foodDataObject.GrowFactor;


    public FoodType CurrentFoodType => _foodDataObject.CurrentFoodType;
}
