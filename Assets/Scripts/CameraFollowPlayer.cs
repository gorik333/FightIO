using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _player;


    private void Start()
    {
        if (_player == null)
        {
            Player player = FindObjectOfType(typeof(Player)) as Player;

            _player = player.transform;
        }
    }


    void LateUpdate()
    {
        FollowPlayer();
    }


    private void FollowPlayer()
    {
        Vector3 newPosition;

        newPosition.x = _player.position.x;
        newPosition.y = transform.position.y;
        newPosition.z = _player.position.z;

        transform.position = newPosition;
    }
}
