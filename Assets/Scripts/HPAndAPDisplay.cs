using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPAndAPDisplay : MonoBehaviour
{
    [SerializeField] PartyManager m_partyManager = null;
    [SerializeField] private bool m_isDisplay = false;
    [SerializeField] GameObject m_gameObject = null;
    private GameObject m_ui;
    private List<GameObject> m_gameObjects = new List<GameObject>();
    private int m_memberNumber = 1;
    private int m_beforeMemberNumber;

    private void Awake()
    {
        m_memberNumber = m_partyManager.m_charaParty.Count;
        m_beforeMemberNumber = m_memberNumber;
        for (int i = 0; i < m_beforeMemberNumber; i++)
        {
            m_ui = Instantiate(m_gameObject);
            m_ui.transform.SetParent(this.transform, false);
            m_gameObjects.Add(m_ui);
        }
    }

    void Start()
    {
        CreateUI();
    }

    void Update()
    {
        m_memberNumber = m_partyManager.m_charaParty.Count;
        if (m_beforeMemberNumber != m_memberNumber)
        {
            Debug.Log("人数変化");
            
            if (m_beforeMemberNumber > m_memberNumber)
            {
                for (int i = m_beforeMemberNumber; i > m_memberNumber; i--)
                {
                    Destroy(transform.GetChild(i - 1).gameObject);
                    m_gameObjects.Remove(m_gameObjects[i - 1]);
                }
            }
            else if (m_beforeMemberNumber < m_memberNumber)
            {
                for (int i = 0; i < m_memberNumber - m_beforeMemberNumber; i++)
                {
                    m_ui = Instantiate(m_gameObject);
                    m_ui.transform.SetParent(this.transform, false);
                    m_gameObjects.Add(m_ui);
                }
            }
            CreateUI();
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

    void CreateUI()
    {
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            m_gameObjects[i].GetComponent<CharacterParameterUI>()
                .CreateName(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().CharacterName);
            m_gameObjects[i].GetComponent<CharacterParameterUI>()
                .CreateParameter(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().NowHP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().MaxHP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().NowAP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().MaxAP);
            m_gameObjects[i].GetComponent<CharacterParameterUI>()
                .CreateGage(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().NowHP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().MaxHP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().NowAP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().MaxAP);
        }
    }
    public void ChangeUI()
    {
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            m_gameObjects[i].GetComponent<CharacterParameterUI>()
                .CreateParameter(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().NowHP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().MaxHP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().NowAP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().MaxAP);
            m_gameObjects[i].GetComponent<CharacterParameterUI>()
                .CreateGage(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().NowHP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().MaxHP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().NowAP,
                m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().MaxAP);
        }
    }
}
