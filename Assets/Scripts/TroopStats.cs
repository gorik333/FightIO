using UnityEngine;

public enum TroopType
{
    Ally,
    Enemy,
    Neutral
}


public class TroopStats : MonoBehaviour
{
    [SerializeField]
    private TroopType _troopType;

    [SerializeField]
    private float _currentWeightNumber;

    [SerializeField]
    private Transform _growModel;


    public void Eat(Food food)
    {
        IncreaseWeight(food.GetFoodWeight);
        Grow(food.GetFoodGrowFactor);
    }


    private void IncreaseWeight(float weight)
    {
        _currentWeightNumber += weight;
    }


    private void Grow(float growFactor)
    {
        BoxCollider[] boxCollider = _growModel.GetComponentsInChildren<BoxCollider>(); // find and re-size
        SphereCollider[] sphereCollider = _growModel.GetComponentsInChildren<SphereCollider>();
        CapsuleCollider[] capsuleCollider = _growModel.GetComponentsInChildren<CapsuleCollider>();

        ResizeBoxColliders(boxCollider, growFactor);
        ResizeCapsuleColliders(capsuleCollider, growFactor);
        ResizeSphereColliders(sphereCollider, growFactor);

        //GrowByBodyType(growFactor);

        _growModel.localScale *= growFactor;

        //TestTest(growFactor);
    }


    private void GrowByBodyType(float growFactor)
    {
        BodyPartType[] bodyPartType = _growModel.GetComponentsInChildren<BodyPartType>();

        for (int i = 0; i < bodyPartType.Length; i++)
        {
            bodyPartType[i].transform.localScale *= (growFactor * bodyPartType[i].GrowFactorCoefficient);
        }
    }


    private void TestTest(float growFactor)
    {
        Transform[] bodyTransform = _growModel.GetComponentsInChildren<Transform>();

        bool temp = true;

        for (int i = 0; i < bodyTransform.Length; i++)
        {
            if (bodyTransform[i].TryGetComponent(out BodyPartType bodyPartType))
            {
                bodyPartType.transform.localScale *= 1 + ((growFactor - 1) * bodyPartType.GrowFactorCoefficient); // 1.05

                //Debug.Log(1 + ((growFactor - 1) * bodyPartType.GrowFactorCoefficient));

                if (i + 1 < bodyTransform.Length && bodyTransform[i + 1].GetComponent<BodyPartType>() == null && temp)
                {
                    float previousGrowFactorCoefficient = bodyTransform[i + 1].localScale.x / bodyTransform[i].localScale.x;

                    if (bodyTransform[i + 1].name == "mixamorig:LeftHand")
                        Debug.Log(previousGrowFactorCoefficient);

                    bodyTransform[i + 1].localScale = new Vector3(previousGrowFactorCoefficient, previousGrowFactorCoefficient, previousGrowFactorCoefficient);

                    temp = false;

                    // continue
                    // if first time change scale,
                    // if second time change nothing.
                }
                if (i + 1 < bodyTransform.Length && bodyTransform[i + 1].GetComponent<BodyPartType>() != null)
                {
                    float previousGrowFactorCoefficient = bodyTransform[i + 1].localScale.x / bodyTransform[i].localScale.x;

                    temp = true;

                    bodyTransform[i + 1].localScale /= previousGrowFactorCoefficient;

                    BodyPartType currentPartType = bodyTransform[i + 1].GetComponent<BodyPartType>();

                    // мне нужно сначала убрать скейл от объекта родителя, потом уже умножать на коэффициент

                    var currentGrowFactorCoefficient = currentPartType.GrowFactorCoefficient;

                    bodyTransform[i + 1].localScale *= currentGrowFactorCoefficient;

                    // change scale with propotional to previous
                }
            }
        }
    }


    //private void Check(int i, bodyTransformLength, Transform bodyTransform)
    //{
    //    if (i + 1 < bodyTransforms.Length && bodyTransforms[i + 1].GetComponent<BodyPartType>() != null)
    //    {

    //    }
    //}


    private void ResizeCapsuleColliders(CapsuleCollider[] collider, float growFactor)
    {
        for (int i = 0; i < collider.Length; ++i)
        {
            collider[i].radius /= growFactor;
        }
    }



    private void ResizeBoxColliders(BoxCollider[] collider, float growFactor)
    {
        for (int i = 0; i < collider.Length; ++i)
        {
            collider[i].size /= growFactor;
        }

    }


    private void ResizeSphereColliders(SphereCollider[] collider, float growFactor)
    {
        for (int i = 0; i < collider.Length; i++)
        {
            collider[i].radius /= growFactor;
        }
    }



    public float GetCurrentWeight => _currentWeightNumber;

    public TroopType TroopType { get => _troopType; set => _troopType = value; }
}
