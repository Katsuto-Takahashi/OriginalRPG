using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] List<GameObject> m_characterParty = new List<GameObject>();

    [SerializeField] List<GameObject> m_enemyParty = new List<GameObject>();

    public List<GameObject> CharacterParty { get => m_characterParty;}

    public List<GameObject> EnemyParty { get => m_enemyParty; }
}
