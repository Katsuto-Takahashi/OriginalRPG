using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField]
    GameObject m_MenuPanel = null;

    [SerializeField]
    GameObject m_battleCommandPanel = null;

    void Start()
    {
        Character player = GameManager.Instance.Player;
        player.BCSM.CanSelect.DistinctUntilChanged().Subscribe(s => DisplayBattleCommandPanel(s)).AddTo(player.gameObject);
    }

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
        m_battleCommandPanel.SetActive(active);
    }
}
