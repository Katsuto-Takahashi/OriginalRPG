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
    Text m_allBattleLog = null;
    [SerializeField]
    Text m_battleLog = null;
    [SerializeField]
    Text m_criticalDamageText = null;
    [SerializeField]
    Text m_levelText = null;

    BattleInformationUI battle;
    [SerializeField]
    GameObject BattleInformationObject = null;

    HPAndAPDisplay m_hpap;
    [SerializeField]
    GameObject m_prefabHPAndAPUI = null;
    [SerializeField]
    GameObject m_HPAndAP = null;

    /// <summary>メニューUIの表示を切り替える</summary>
    /// <param name="active">表示するかどうか</param>
    public void DisplayMenu(bool active)
    {
        m_MenuPanel.SetActive(active);
    }

    /// <summary>最初のバトルコマンドを表示する</summary>
    public void DisplayFirstBattleCommandPanel()
    {
        m_battleCommandPanels[0].SetActive(true);
    }

    /// <summary>全てのバトルコマンドを非表示にする</summary>
    /// <param name="second">非表示にするまでの秒数</param>
    public void CloseBattleCommandPanel(float second)
    {
        StartCoroutine(CloseBattleCommandPanelImp(second));
    }

    IEnumerator CloseBattleCommandPanelImp(float second)
    {
        yield return new WaitForSeconds(second);
        foreach (var item in m_battleCommandPanels)
        {
            item.SetActive(false);
        }
    }

    public void Initialize()
    {
        CreateHPAndAPUIObject(GameManager.Instance.MaxPartyCount);

        m_hpap = new HPAndAPDisplay(m_HPAndAP);
        battle = new BattleInformationUI(m_criticalDamageText, m_levelText, m_battleLog);
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

    public void ChangeInformationActive(float scond)
    {
        StartCoroutine(ChengeInformationActiveImp(scond));
    }

    IEnumerator ChengeInformationActiveImp(float scond)
    {
        yield return new WaitForSeconds(scond);
        BattleInformationObject.SetActive(!BattleInformationObject.activeSelf);
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

    public void AllLog(string log)
    {
        m_allBattleLog.text += log;
    }

    void CreateHPAndAPUIObject(int count)
    {
        GameObject obj;
        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(m_prefabHPAndAPUI);
            obj.transform.SetParent(m_HPAndAP.transform, false);
            obj.SetActive(false);
        }
    }

    public void CreateHPAndAPUI(Character character)
    {
        m_hpap.Create(character);
    }

    public void DeleteHPAndAPUI(Character character)
    {
        m_hpap.Delete(character);
    }
}
