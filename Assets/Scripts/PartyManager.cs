using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PartyManager : SingletonMonoBehaviour<PartyManager>
{
    [SerializeField, Tooltip("Player側")] 
    List<Character> m_characterParty = new List<Character>();

    [SerializeField, Tooltip("敵側")] 
    List<Enemy> m_enemyParty = new List<Enemy>();

    public List<Character> CharacterParty  => m_characterParty;

    public List<Enemy> EnemyParty => m_enemyParty;


    ReactiveCollection<GameObject> cp = new ReactiveCollection<GameObject>();
    public ReactiveCollection<GameObject> CharacterP => cp;

    ReactiveCollection<GameObject> ep = new ReactiveCollection<GameObject>();
    public ReactiveCollection<GameObject> EnemyP => ep;
}
