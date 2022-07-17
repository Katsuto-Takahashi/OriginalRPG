using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInformationUI
{
    Text m_criticalText;
    Text m_levelText;
    Text m_battleLog;

    public BattleInformationUI(Text criticalText, Text levelText, Text battleLog)
    {
        m_criticalText = criticalText;
        m_levelText = levelText;
        m_battleLog = battleLog;
    }

    public IEnumerator CriticalHit()
    {
        m_criticalText.gameObject.SetActive(true);
        m_battleLog.text += m_criticalText.text + "\n";
        yield return new WaitForSeconds(0.5f);
        m_criticalText.gameObject.SetActive(false);
    }

    public IEnumerator DamageUIDisplay(int damage, string characterName)
    {
        m_battleLog.gameObject.SetActive(true);
        if (damage > 0)
        {
            m_battleLog.text += $"{characterName}は{damage}のダメージをうけた\n";
        }
        else
        {
            m_battleLog.text += $"{characterName}はダメージをうけなかった\n";
        }
        yield return new WaitForSeconds(2);
        m_battleLog.gameObject.SetActive(false);
    }
    
    public IEnumerator RecoveryUIDisplay(int heal, string characterName, HealPoint healPoint)
    {
        m_battleLog.gameObject.SetActive(true);
        string text;

        switch (healPoint)
        {
            case HealPoint.HP:
                text = $"{characterName}のHPが{heal}かいふくした\n";
                break;
            case HealPoint.AP:
                text = $"{characterName}のAPが{heal}かいふくした\n";
                break;
            default:
                text = "";
                break;
        }

        if (heal > 0)
        {
            m_battleLog.text += text;
        }
        yield return new WaitForSeconds(2);
        m_battleLog.gameObject.SetActive(false);
    }

    public IEnumerator BattleStartUI(string name, int enemiesNumber)
    {
        m_battleLog.gameObject.SetActive(true);
        if (enemiesNumber > 1)
        {
            m_battleLog.text += $"{name}たちがあらわれた\n";
        }
        else
        {
            m_battleLog.text += $"{name}があらわれた\n";
        }
        yield return new WaitForSeconds(1);
        m_battleLog.gameObject.SetActive(false);
    }

    public IEnumerator BattleFinishUI(BattleResults result)
    {
        m_battleLog.gameObject.SetActive(true);
        var resultText = "";
        switch (result)
        {
            case BattleResults.Win:
                resultText = "しょうりした\n";
                break;
            case BattleResults.Lose:
                resultText = "はいぼくした\n";
                break;
            case BattleResults.Escape:
                resultText = "にげだした\n";
                break;
            default:
                break;
        }

        m_battleLog.text += resultText;
        yield return new WaitForSeconds(3);
        m_battleLog.gameObject.SetActive(false);
    }

    public IEnumerator LevelUpUI(Character character)
    {
        m_levelText.gameObject.SetActive(true);
        m_levelText.text += $"{character.Name.Value}のレベルが{character.Level.Value}にあがった\n";
        m_battleLog.text += m_levelText.text;
        yield return new WaitForSeconds(2);
        character.LevelUP.Value = false;
        m_levelText.gameObject.SetActive(false);
    }

    public IEnumerator GetExpUI(int exp, string charaName)
    {
        m_levelText.gameObject.SetActive(true);
        m_levelText.text = $"{charaName}は{exp}の経験値を手に入れた\n";
        m_battleLog.text += m_levelText.text;
        yield return new WaitForSeconds(1);
        m_levelText.gameObject.SetActive(false);
    }
}
