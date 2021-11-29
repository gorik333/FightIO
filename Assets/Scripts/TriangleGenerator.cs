using UnityEngine;
using System.Collections.Generic;

public enum TriangleSpawnPosition
{
    RightSide,
    LeftSide
}


public class TriangleGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform[] _startPositions;

    public static TriangleGenerator Instance { get; private set; }

    private const float FULL_DISTANCE = 2.75f;
    private const float HALF_DISTANCE = 1.375f;
    private const float BETWEEN_ROWS_DISTANCE = 0.75f;


    private void Awake()
    {
        Instance = this;
    }


    private int CalcTriangle(int troopCount)
    {
        int rows = 0;

        for (int i = 0; i < troopCount; ++i)
        {
            if (troopCount > rows)
                rows++;
            else
                break;

            troopCount -= i;
        }

        return rows;
    }


    public List<Vector3> GenerateTriangle(int troopCount, TriangleSpawnPosition side)
    {
        int rows = CalcTriangle(troopCount) + 1;

        List<Vector3> position = new List<Vector3>();

        int startPoint = rows / 2;

        int countSpawned = 0;
        int rowsReached = 0;

        Vector3 startPosition = Vector3.zero;

        if (TriangleSpawnPosition.RightSide == side)
            startPosition = _startPositions[0].position;
        else if (TriangleSpawnPosition.LeftSide == side)
            startPosition = _startPositions[1].position;

        Vector3 resultPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + (-startPoint * FULL_DISTANCE));

        for (int i = 0; i < troopCount; i++)
        {
            position.Add(resultPosition);

            countSpawned++;

            resultPosition.z += FULL_DISTANCE;

            if (countSpawned >= rows)
            {
                rows--;
                countSpawned = 0;
                rowsReached++;

                if (side == TriangleSpawnPosition.RightSide)
                    resultPosition.x += BETWEEN_ROWS_DISTANCE;
                else if (side == TriangleSpawnPosition.LeftSide)
                    resultPosition.x -= BETWEEN_ROWS_DISTANCE;

                resultPosition.z = startPosition.z + (-startPoint * FULL_DISTANCE) + (HALF_DISTANCE * rowsReached);
            }
        }

        return position;
    }
}
