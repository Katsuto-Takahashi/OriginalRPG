using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PartyManager : MonoBehaviour
{
    [SerializeField, Tooltip("Player側")] 
    List<GameObject> m_characterParty = new List<GameObject>();

    [SerializeField, Tooltip("敵側")] 
    List<GameObject> m_enemyParty = new List<GameObject>();

    public List<GameObject> CharacterParty  => m_characterParty;

    public List<GameObject> EnemyParty => m_enemyParty;

    IntReactiveProperty m_characterCount = new IntReactiveProperty();
    public IReadOnlyReactiveProperty<int> CharacterCount => m_characterCount;
    void Awake()
    {
        m_characterCount.Value = m_characterParty.Count;

        Observable.EveryUpdate().Subscribe(_ => ChengeCount())
            .AddTo(this);
    }

    void ChengeCount()
    {
        m_characterCount.Value = m_characterParty.Count;
    }
}
