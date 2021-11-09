using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInformationUI : MonoBehaviour
{
    [SerializeField] Text m_damageText = null;
    [SerializeField] Text m_criticalDamageText = null;
    [SerializeField] Text m_informationText = null;
    [SerializeField] Text m_levelText = null;
    private bool m_critical = false;

    public bool Critical { get => m_critical; set => m_critical = value; }

    void Start()
    {
        Critical = false;
    }

    void Update()
    {
        
    }
    
    public IEnumerator BattleUIDisplay(int damage, string attackedCharacter, bool critical)
    {
        if (critical)
        {
            m_criticalDamageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            m_criticalDamageText.gameObject.SetActive(false);
            Critical = false;
        }
        m_damageText.gameObject.SetActive(true);
        m_damageText.text += $"{attackedCharacter}は{damage}のダメージをうけた\n";
        yield return new WaitForSeconds(2);
        m_damageText.text = "";
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
    public IEnumerator BattleFinishUI(int check)
    {
        m_informationText.gameObject.SetActive(true);
        if (check < 1)
        {
            yield return new WaitForSeconds(1);
            m_damageText.text = "";
            m_informationText.text = "しょうりした";
        }
        else if (check < 2)
        {
            yield return new WaitForSeconds(1);
            m_damageText.text = "";
            m_informationText.text = "はいぼくした";
        }
        else
        {
            m_informationText.text = "にげだした";
        }
        yield return new WaitForSeconds(3);
        m_informationText.text = " ";
        m_informationText.gameObject.SetActive(false);
    }
    public IEnumerator BattleResultUI(int check)
    {
        m_informationText.gameObject.SetActive(true);
        if (check < 1)
        {
            m_informationText.text = "しょうりした";
        }
        else
        {
            m_informationText.text = "はいぼくした";
        }
        yield return new WaitForSeconds(3);
        m_informationText.text = " ";
        m_informationText.gameObject.SetActive(false);
    }
    public IEnumerator GetExpUI(int exp, string charaName, int level, bool levelUp)
    {
        m_levelText.gameObject.SetActive(true);
        m_levelText.text = $"{exp}の経験値を手に入れた";
        yield return new WaitForSeconds(1);
        m_levelText.text = " ";
        if (levelUp)
        {
            m_levelText.text += $"{charaName}のレベルが{level}にあがった";
            yield return new WaitForSeconds(2);
        }
        m_levelText.gameObject.SetActive(false);
    }
}
