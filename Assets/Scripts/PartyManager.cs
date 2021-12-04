using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField, Tooltip("Player側")] 
    List<GameObject> m_characterParty = new List<GameObject>();

    [SerializeField, Tooltip("敵側")] 
    List<GameObject> m_enemyParty = new List<GameObject>();

    public List<GameObject> CharacterParty  => m_characterParty;

    public List<GameObject> EnemyParty => m_enemyParty;
}
