using UnityEngine;

public class RandomPoint : MonoBehaviour
{
    [SerializeField]
    private Transform _planeTransform;

    private const int PLANE_MULTIPLIER = 10;

    private const float TERRITORY_DIVIDER = 2.3f;

    public static RandomPoint Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        if (_planeTransform == null)
            _planeTransform = transform;
    }


    public Vector3 GetRandomPoint()
    {
        Vector3 size = _planeTransform.localScale * PLANE_MULTIPLIER;

        Vector3 pointPos = _planeTransform.position + 
            new Vector3(Random.Range(-size.x / TERRITORY_DIVIDER, size.x / TERRITORY_DIVIDER), 
            _planeTransform.position.y, Random.Range(-size.z / TERRITORY_DIVIDER, size.z / TERRITORY_DIVIDER));

        return pointPos;
    }
}
