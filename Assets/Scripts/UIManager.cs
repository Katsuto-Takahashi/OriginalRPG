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

    /// <summary>���j���[UI�̕\����؂�ւ���</summary>
    /// <param name="active">�\�����邩�ǂ���</param>
    public void DisplayMenu(bool active)
    {
        m_MenuPanel.SetActive(active);
    }

    /// <summary>�o�g���R�}���h�̕\����؂�ւ���</summary>
    /// <param name="active">�\�����邩�ǂ���</param>
    public void DisplayBattleCommandPanel(bool active)
    {
        m_battleCommandPanel.SetActive(active);
    }
}
