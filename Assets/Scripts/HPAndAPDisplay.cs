using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HPAndAPDisplay : MonoBehaviour
{
    [SerializeField] 
    PartyManager m_partyManager = null;

    [SerializeField] 
    bool m_isDisplay = false;

    [SerializeField] 
    GameObject m_gameObject = null;

    GameObject m_ui;

    List<GameObject> m_gameObjects = new List<GameObject>();

    int m_memberNumber = 1;

    int m_beforeMemberNumber;

    List<Character> m_cm = new List<Character>();

    void Awake()
    {
        m_memberNumber = m_partyManager.CharacterParty.Count;
        m_beforeMemberNumber = m_memberNumber;
        for (int i = 0; i < m_beforeMemberNumber; i++)
        {
            m_ui = Instantiate(m_gameObject);
            m_ui.transform.SetParent(transform, false);
            m_gameObjects.Add(m_ui);
            m_cm.Add(m_partyManager.CharacterParty[i].GetComponent<Character>());
        }
    }

    void Start()
    {
        
        Create();
        Observable.EveryUpdate().Subscribe(_ => OnUpdate())
            .AddTo(this);
        
    }

    void OnUpdate()
    {
        m_memberNumber = m_partyManager.CharacterParty.Count;
        if (m_beforeMemberNumber != m_memberNumber)
        {
            Debug.Log("人数変化");
            
            if (m_beforeMemberNumber > m_memberNumber)
            {
                for (int i = m_beforeMemberNumber; i > m_memberNumber; i--)
                {
                    Destroy(transform.GetChild(i - 1).gameObject);
                    m_gameObjects.Remove(m_gameObjects[i - 1]);
                    m_cm.Remove(m_cm[i - 1]);
                }
            }
            else if (m_beforeMemberNumber < m_memberNumber)
            {
                for (int i = 0; i < m_memberNumber - m_beforeMemberNumber; i++)
                {
                    m_ui = Instantiate(m_gameObject);
                    m_ui.transform.SetParent(transform, false);
                    m_gameObjects.Add(m_ui);
                    m_cm.Add(m_partyManager.CharacterParty[i].GetComponent<Character>());
                }
            }

            Create();
            m_beforeMemberNumber = m_memberNumber;
        }

        if (m_isDisplay)
        {
            for (int i = 0; i < m_beforeMemberNumber; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < m_beforeMemberNumber; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void Chenge()
    {
        Debug.Log("チェンジ");

        for (int i = 0; i < m_cm.Count; i++)
        {
            var ui = m_gameObjects[i].GetComponent<CharacterParameterUI>();
            ui.CreateName(m_cm[i].Name.Value);
            ui.CreateParameter(
                m_cm[i].HP.Value,
                m_cm[i].MaxHP.Value,
                m_cm[i].AP.Value,
                m_cm[i].MaxAP.Value
                );
        }
    }

    void Create()
    {
        for (int i = 0; i < m_cm.Count; i++)
        {
            m_cm[i].HP.DistinctUntilChanged().Subscribe(_ => Chenge()).AddTo(m_gameObjects[i]);
            m_cm[i].MaxHP.DistinctUntilChanged().Subscribe(_ => Chenge()).AddTo(m_gameObjects[i]);
            m_cm[i].AP.DistinctUntilChanged().Subscribe(_ => Chenge()).AddTo(m_gameObjects[i]);
            m_cm[i].MaxAP.DistinctUntilChanged().Subscribe(_ => Chenge()).AddTo(m_gameObjects[i]);
        }
    }
}
