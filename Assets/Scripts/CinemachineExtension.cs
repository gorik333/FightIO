using UnityEngine;
using System.Collections;
using Cinemachine;


public enum GameOffset
{
    CollectFoodOffset = 22,
    MenuOffset = 11,
    BattleOffset = 21
}


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineExtension : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;

    [SerializeField]
    private CinemachineTransposer _cinemachineTransposer;

    private const float SPEED_MULTIPLIER = 6f;
    private const float FIGHT_Z_OFFSET = -12f;
    private const float MENU_Z_OFFSET = -5f;
    private const float COLLECT_FOOD_Z_OFFSET = -8f;

    public static CinemachineExtension Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }


    public void SetFollow(Transform target, GameOffset offset)
    {
        _virtualCamera.Follow = target.transform;

        StartCoroutine(FollowOffsetChange(offset));
    }


    public void ChangeOffset(GameOffset offset)
    {
        StartCoroutine(FollowOffsetChange(offset));
    }


    public void SetMenuOffset()
    {
        int offset = (int)GameOffset.MenuOffset;

        _cinemachineTransposer.m_FollowOffset.y = offset;

        transform.position = new Vector3(0f, offset, 0f);
    }


    private IEnumerator FollowOffsetChange(GameOffset offset)
    {
        ChangeZOffset(offset);

        float currentOffset = _cinemachineTransposer.m_FollowOffset.y;

        int targetOffset = (int)offset;

        while (true)
        {
            if (currentOffset < targetOffset)
            {
                currentOffset += Time.deltaTime * SPEED_MULTIPLIER;

                if (targetOffset <= currentOffset)
                    break;
            }
            else if (currentOffset >= targetOffset)
            {
                currentOffset -= Time.deltaTime * SPEED_MULTIPLIER;

                if (targetOffset >= currentOffset)
                    break;
            }

            _cinemachineTransposer.m_FollowOffset.y = currentOffset;

            yield return new WaitForEndOfFrame();
        }
    }


    private void ChangeZOffset(GameOffset offset)
    {
        switch (offset)
        {
            case GameOffset.BattleOffset:
                _cinemachineTransposer.m_FollowOffset.z = FIGHT_Z_OFFSET;
                break;
            case GameOffset.MenuOffset:
                _cinemachineTransposer.m_FollowOffset.z = MENU_Z_OFFSET;
                break;
            case GameOffset.CollectFoodOffset:
                _cinemachineTransposer.m_FollowOffset.z = COLLECT_FOOD_Z_OFFSET;
                break;
        }
    }
}
