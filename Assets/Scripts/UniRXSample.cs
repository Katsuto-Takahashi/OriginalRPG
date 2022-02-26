using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class UniRXSample : MonoBehaviour
{
    //飛ばす側
    [SerializeField] private BoolReactiveProperty _testBool = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> TestRX => _testBool;
}
