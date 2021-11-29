using UnityEngine;
using TMPro;

public class CurrentWeight : MonoBehaviour
{
    [SerializeField]
    private Transform _troopTransform;

    [SerializeField]
    private TextMeshProUGUI _textMeshPro;


    void LateUpdate()
    {
        Follow();
    }


    public void UpdateWeightInfoText(float weight)
    {
        _textMeshPro.text = weight.ToString();
    }


    private void Follow()
    {
        Vector3 newPosition;

        newPosition.x = _troopTransform.position.x;
        newPosition.y = transform.position.y;
        newPosition.z = _troopTransform.position.z;

        transform.position = newPosition;
    }
}
