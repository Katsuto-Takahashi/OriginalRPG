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

    List<CharacterManager> m_cm = new List<CharacterManager>();

    void Awake()
    {
        m_memberNumber = m_partyManager.CharacterParty.Count;
        m_beforeMemberNumber = m_memberNumber;
        for (int i = 0; i < m_beforeMemberNumber; i++)
        {
            m_ui = Instantiate(m_gameObject);
            m_ui.transform.SetParent(transform, false);
            m_gameObjects.Add(m_ui);
            m_cm.Add(m_partyManager.CharacterParty[i].GetComponent<CharacterManager>());
        }
    }

    void Start()
    {
        Chenge();
    }

    void Update()
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
                    m_cm.Add(m_partyManager.CharacterParty[i].GetComponent<CharacterManager>());
                }
            }

            Chenge();
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

    void Create()
    {
        Debug.Log("チェンジ");

        for (int i = 0; i < m_cm.Count; i++)
        {
            var ui = m_gameObjects[i].GetComponent<CharacterParameterUI>();
            ui.CreateName(m_cm[i].Character.Name.Value);
            ui.CreateParameter(
                m_cm[i].Character.HP.Value,
                m_cm[i].Character.MaxHP.Value,
                m_cm[i].Character.AP.Value,
                m_cm[i].Character.MaxAP.Value
                );
        }
    }

    void Chenge()
    {
        for (int i = 0; i < m_cm.Count; i++)
        {
            m_cm[i].Character.HP.DistinctUntilChanged().Subscribe(_ => Create()).AddTo(m_gameObjects[i]);
            m_cm[i].Character.MaxHP.DistinctUntilChanged().Subscribe(_ => Create()).AddTo(m_gameObjects[i]);
            m_cm[i].Character.AP.DistinctUntilChanged().Subscribe(_ => Create()).AddTo(m_gameObjects[i]);
            m_cm[i].Character.MaxAP.DistinctUntilChanged().Subscribe(_ => Create()).AddTo(m_gameObjects[i]);
        }
    }
}
