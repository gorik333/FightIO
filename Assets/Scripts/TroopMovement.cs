using UnityEngine;
using DG.Tweening;

public class TroopMovement : MonoBehaviour
{
    private const float STAY_TURNING_SPEED = 0.4f;
    private const float RUN_TURNING_SPEED = 0.5f;
    private const float TIME_TO_RUN_TO_START_POSITION = 0.4f;


    public void MoveTroopForBoth(Vector3 direction, Rigidbody RB, float moveSpeed, bool isPlayer)
    {
        Vector3 directionChanged;

        if (isPlayer)
            directionChanged = new Vector3(direction.x * moveSpeed * Time.fixedDeltaTime, 0, direction.z * moveSpeed * Time.fixedDeltaTime);
        else
            directionChanged = new Vector3(direction.x * moveSpeed * Time.fixedDeltaTime, 0, direction.z * moveSpeed * Time.fixedDeltaTime);

        RB.AddForce(force: directionChanged, ForceMode.VelocityChange);
    }


    public void MoveTroopTransform(Transform troopTransform, Vector3 destinationPosition, float moveSpeed)
    {
        troopTransform.position = Vector3.MoveTowards(troopTransform.position, destinationPosition, moveSpeed * Time.fixedDeltaTime);

        troopTransform.DOLookAt(destinationPosition, RUN_TURNING_SPEED);
    }


    public void RotateToEnemyTransform(Transform troopTransform, Transform target)
    {
        troopTransform.DOLookAt(target.position, STAY_TURNING_SPEED);
    }


    public void MoveToStartPosition(Transform troopTransform, Vector3 destinationPosition)
    {
        troopTransform.DOMove(destinationPosition, TIME_TO_RUN_TO_START_POSITION);
    }
}
