using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private int _num;


    private void Update()
    {
        CalcTriangle(_num);
    }


    public int CalcTriangle(int n)
    {
        int rows = 0;
        int temp = 0;

        for (int i = 0; i < n; i++)
        {
            temp += i;

            if (temp >= n)
                break;
            else
                rows++;
        }

        return rows;
    }
}
