using UnityEngine;


public enum BodyType
{
    Head,
    UpLeg,
    DownLeg,
    UpArm,
    DownArm,
    Stomach
}


public class BodyPartType : MonoBehaviour
{
    [SerializeField]
    private BodyType _bodyType;

    private const float UP_ARM_GROW_FACTOR = 0.6f;
    private const float DOWN_ARM_GROW_FACTOR = 0.2f;

    private const float UP_LEG_GROW_FACTOR = 0.6f;
    private const float DOWN_LEG_GROW_FACTOR = 0.2f;

    private const float HEAD_GROW_FACTOR = 0.8f;

    private const float STOMACH_GROW_FACTOR = 1f;


    public BodyType CurrentBodyType => _bodyType;


    public float GrowFactorCoefficient => GetGrowFactorByBodyType(CurrentBodyType);


    private float GetGrowFactorByBodyType(BodyType bodyType)
    {
        switch (bodyType)
        {
            case BodyType.UpArm:
                return UP_ARM_GROW_FACTOR;
            case BodyType.DownArm:
                return DOWN_ARM_GROW_FACTOR;
            case BodyType.UpLeg:
                return UP_LEG_GROW_FACTOR;
            case BodyType.DownLeg:
                return DOWN_LEG_GROW_FACTOR;
            case BodyType.Head:
                return HEAD_GROW_FACTOR;
            case BodyType.Stomach:
                return STOMACH_GROW_FACTOR;
            default:
                return -1;
        }
    }
}
