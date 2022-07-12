using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>, IManagable
{
    [SerializeField]
    GameObject m_MenuPanel = null;

    [SerializeField]
    List<GameObject> m_battleCommandPanels = new List<GameObject>();

    [SerializeField]
    Text m_damageText = null;
    [SerializeField]
    Text m_criticalDamageText = null;
    [SerializeField]
    Text m_informationText = null;
    [SerializeField]
    Text m_levelText = null;
    [SerializeField]
    BattleInformationUI battle = null;

    /// <summary>メニューUIの表示を切り替える</summary>
    /// <param name="active">表示するかどうか</param>
    public void DisplayMenu(bool active)
    {
        m_MenuPanel.SetActive(active);
    }

    /// <summary>最初のバトルコマンドの表示を切り替える</summary>
    /// <param name="active">表示するかどうか</param>
    public void DisplayFirstBattleCommandPanel(bool active)
    {
        m_battleCommandPanels[0].SetActive(active);
    }

    /// <summary>全てのバトルコマンドを非表示にする</summary>
    public void CloseBattleCommandPanel()
    {
        foreach (var item in m_battleCommandPanels)
        {
            item.SetActive(false);
        }
    }

    public void Initialize()
    {
        battle.SetText(m_damageText, m_criticalDamageText, m_informationText, m_levelText);
    }

    public void Recovery(int heal, string characterName, HealPoint healPoint)
    {
        StartCoroutine(battle.RecoveryUIDisplay(heal, characterName, healPoint));
    }
    
    public void Damage(int damage, string characterName)
    {
        StartCoroutine(battle.DamageUIDisplay(damage, characterName));
    }
    
    public void CriticalHit()
    {
        StartCoroutine(battle.CriticalHit());
    }

    public void ChengeInformationActive()
    {
        battle.gameObject.SetActive(!battle.gameObject.activeSelf);
    }

    public void BattleStart(string enemyName, int enemiesNumber)
    {
        StartCoroutine(battle.BattleStartUI(enemyName, enemiesNumber));
    }

    public void BattleFinish(BattleResults result)
    {
        StartCoroutine(battle.BattleFinishUI(result));
    }

    public void GetExp(int exp, string characterName)
    {
        StartCoroutine(battle.GetExpUI(exp, characterName));
    }

    public void LevelUp(Character character)
    {
        StartCoroutine(battle.LevelUpUI(character));
    }
}
