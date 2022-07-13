using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecoverable
{
    void Recover(int value, HealPoint healPoint);
}
