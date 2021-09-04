﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterParameterUI : MonoBehaviour
{
    [SerializeField] Text m_name;
    [SerializeField] Text m_HP;
    [SerializeField] Text m_AP;
    [SerializeField] Slider m_HPGage;
    [SerializeField] Slider m_APGage;

    public void CreateName(string name)
    {
        m_name.text = name;
    }
    public void CreateParameter(int nowHP, int maxHP, int nowAP, int maxAP)
    {
        m_HP.text = nowHP.ToString("0") + "/" + maxHP.ToString("0");
        m_AP.text = nowAP.ToString("0") + "/" + maxAP.ToString("0");
    }
    public void CreateGage(int nowHP, int maxHP, int nowAP, int maxAP)
    {
        m_HPGage.value = nowHP / (float)maxHP;
        m_APGage.value = nowAP / (float)maxAP;
    }
}
