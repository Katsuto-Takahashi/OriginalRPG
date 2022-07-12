using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInformationUI : MonoBehaviour
{
    Text m_damageText;
    Text m_criticalText;
    Text m_informationText;
    Text m_levelText;

    public void SetText(Text damageText, Text criticalText, Text informationText, Text levelText)
    {
        m_damageText = damageText;
        m_criticalText = criticalText;
        m_informationText = informationText;
        m_levelText = levelText;
    }

    public IEnumerator CriticalHit()
    {
        m_criticalText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_criticalText.gameObject.SetActive(false);
    }

    public IEnumerator DamageUIDisplay(int damage, string characterName)
    {
        m_damageText.gameObject.SetActive(true);
        m_damageText.text = " ";
        if (damage > 0)
        {
            m_damageText.text += $"{characterName}は{damage}のダメージをうけた\n";
        }
        else
        {
            m_damageText.text += $"{characterName}はダメージをうけなかった\n";
        }
        yield return new WaitForSeconds(2);
        m_damageText.gameObject.SetActive(false);
    }
    
    public IEnumerator RecoveryUIDisplay(int heal, string characterName, HealPoint healPoint)
    {
        m_damageText.gameObject.SetActive(true);
        m_damageText.text = " ";
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
            m_damageText.text += text;
        }
        yield return new WaitForSeconds(2);
        m_damageText.gameObject.SetActive(false);
    }

    public IEnumerator BattleStartUI(string name, int enemiesNumber)
    {
        m_informationText.gameObject.SetActive(true);
        if (enemiesNumber > 1)
        {
            m_informationText.text = $"{name}たちがあられた";
        }
        else
        {
            m_informationText.text = $"{name}があられた";
        }
        yield return new WaitForSeconds(1);
        m_informationText.text = " ";
        m_informationText.gameObject.SetActive(false);
    }

    public IEnumerator BattleFinishUI(BattleResults result)
    {
        m_informationText.gameObject.SetActive(true);
        m_damageText.text = " ";
        var resultText = "";
        switch (result)
        {
            case BattleResults.Win:
                resultText = "しょうりした";
                break;
            case BattleResults.Lose:
                resultText = "はいぼくした";
                break;
            case BattleResults.Escape:
                resultText = "にげだした";
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(1);
        m_informationText.text = resultText;

        yield return new WaitForSeconds(3);
        m_informationText.gameObject.SetActive(false);
    }

    public IEnumerator LevelUpUI(Character character)
    {
        m_levelText.gameObject.SetActive(true);
        m_levelText.text = " ";

        m_levelText.text += $"{character.Name.Value}のレベルが{character.Level.Value}にあがった";
        yield return new WaitForSeconds(2);
        character.LevelUP.Value = false;
        m_levelText.gameObject.SetActive(false);
    }

    public IEnumerator GetExpUI(int exp, string charaName)
    {
        m_levelText.gameObject.SetActive(true);
        m_levelText.text = " ";
        m_levelText.text = $"{charaName}は{exp}の経験値を手に入れた";
        yield return new WaitForSeconds(1);
        m_levelText.text = " ";
        m_levelText.gameObject.SetActive(false);
    }
}
