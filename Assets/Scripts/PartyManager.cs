using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PartyManager", menuName = "PartyManager")]
public class PartyManager : ScriptableObject
{
    [SerializeField]
    private List<GameObject> partyMembers = null;

    public List<GameObject> GetAllyGameObject()
    {
        return partyMembers;
    }
}
