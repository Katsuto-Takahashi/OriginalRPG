using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PartyManager : SingletonMonoBehaviour<PartyManager>
{
    [SerializeField, Tooltip("Player側")] 
    List<GameObject> m_characterParty = new List<GameObject>();

    [SerializeField, Tooltip("敵側")] 
    List<GameObject> m_enemyParty = new List<GameObject>();

    public List<GameObject> CharacterParty  => m_characterParty;

    public List<GameObject> EnemyParty => m_enemyParty;


    ReactiveCollection<GameObject> cp = new ReactiveCollection<GameObject>();
    public ReactiveCollection<GameObject> CharacterP => cp;

    ReactiveCollection<GameObject> ep = new ReactiveCollection<GameObject>();
    public ReactiveCollection<GameObject> EnemyP => ep;
}
