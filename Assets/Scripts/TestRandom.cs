using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandom : MonoBehaviour
{
    List<int> GetRandomIndex(int max)
    {
        List<int> indexNum = new List<int>();
        int num;
        for (int i = 0; i < max; i++)
        {
            num = Random.Range(0, max);
            if (!indexNum.Contains(num))
            {
                indexNum.Add(num);
            }
            else
            {
                i--;
            }
        }
        return indexNum;
    }
}
