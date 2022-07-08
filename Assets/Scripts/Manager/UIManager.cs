using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField]
    GameObject m_MenuPanel = null;

    [SerializeField]
    List<GameObject> m_battleCommandPanels = new List<GameObject>();
    //GameObject m_battleCommandPanel = null;

    /// <summary>メニューUIの表示を切り替える</summary>
    /// <param name="active">表示するかどうか</param>
    public void DisplayMenu(bool active)
    {
        m_MenuPanel.SetActive(active);
    }

    /// <summary>バトルコマンドの表示を切り替える</summary>
    /// <param name="active">表示するかどうか</param>
    public void DisplayBattleCommandPanel(bool active)
    {
        //bool activeCheck = false;

        //for (int i = 0; i < m_battleCommandPanels.Count; i++)
        //{
        //    if (m_battleCommandPanels[i].activeSelf)
        //    {
        //        activeCheck = true;
        //        break;
        //    }
        //    else
        //    {
        //        activeCheck = false;
        //    }
        //}
        //Debug.Log($"もらう{active}設定{activeCheck}");
        //if (activeCheck)
        //{
        //    m_battleCommandPanels[0].SetActive(active);
        //}
        //else if (activeCheck != active)
        //{
            m_battleCommandPanels[0].SetActive(active);
        //}
    }
}
