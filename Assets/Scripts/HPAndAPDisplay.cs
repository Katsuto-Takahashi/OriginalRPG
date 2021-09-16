﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UniRx;

public class HPAndAPDisplay : MonoBehaviour
{
    [SerializeField] PartyManager m_partyManager = null;
    [SerializeField] private bool m_isDisplay = false;
    [SerializeField] GameObject m_gameObject = null;

    //[SerializeField] CharacterParameters m_characterParameters;

    private GameObject m_ui;
    private List<GameObject> m_gameObjects = new List<GameObject>();
    private int m_memberNumber = 1;
    private int m_beforeMemberNumber;
    private List<CharacterParameterManager> m_parameters = new List<CharacterParameterManager>();

    private void Awake()
    {
        m_memberNumber = m_partyManager.m_charaParty.Count;
        m_beforeMemberNumber = m_memberNumber;
        for (int i = 0; i < m_beforeMemberNumber; i++)
        {
            m_ui = Instantiate(m_gameObject);
            m_ui.transform.SetParent(this.transform, false);
            m_gameObjects.Add(m_ui);
            m_parameters.Add(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>());
            //m_parameters[i].TestHP.Subscribe(_ => ChangeUI());
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
                    m_parameters.Remove(m_parameters[i - 1]);
                }
            }
            else if (m_beforeMemberNumber < m_memberNumber)
            {
                for (int i = 0; i < m_memberNumber - m_beforeMemberNumber; i++)
                {
                    m_ui = Instantiate(m_gameObject);
                    m_ui.transform.SetParent(this.transform, false);
                    m_gameObjects.Add(m_ui);
                    m_parameters.Add(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>());
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
            var ui = m_gameObjects[i].GetComponent<CharacterParameterUI>();
            ui.CreateName(m_parameters[i].CharacterName);
            ui.CreateParameter(m_parameters[i].NowHP,
                m_parameters[i].MaxHP,
                m_parameters[i].NowAP,
                m_parameters[i].MaxAP);
            ui.CreateGage(m_parameters[i].NowHP,
                m_parameters[i].MaxHP,
                m_parameters[i].NowAP,
                m_parameters[i].MaxAP);
            //m_parameters[i].TestHP.Subscribe(_ => ChangeUI());
        }
    }
    public void ChangeUI()
    {
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            var ui = m_gameObjects[i].GetComponent<CharacterParameterUI>();
            ui.CreateParameter(m_parameters[i].NowHP,
                m_parameters[i].MaxHP,
                m_parameters[i].NowAP,
                m_parameters[i].MaxAP);
            ui.CreateGage(m_parameters[i].NowHP,
                m_parameters[i].MaxHP,
                m_parameters[i].NowAP,
                m_parameters[i].MaxAP);
        }
    }
}
