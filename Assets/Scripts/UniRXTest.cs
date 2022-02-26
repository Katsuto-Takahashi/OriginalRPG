using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UniRXTest : MonoBehaviour
{
    UniRXSample uniRX;
    void Start()
    {
        uniRX = GetComponent<UniRXSample>();
        uniRX.TestRX
            //.Where(x => x is true)
            .Subscribe(x => DoTest(x));
    }
    void DoTest(bool a)
    {
        Debug.Log(a);
    }
}
