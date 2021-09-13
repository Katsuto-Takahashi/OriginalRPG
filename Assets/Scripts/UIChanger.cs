using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIChanger : MonoBehaviour
{
    /// <summary>次のコマンドパネル</summary>
    [SerializeField] GameObject m_nextCommandPanel = null;
    /// <summary>前のコマンドパネル</summary>
    [SerializeField] GameObject m_beforeCommandPanel = null;
    //[SerializeField] SceneLoader m_sceneLoader = null;
    private GameObject m_myCommandPanel;
    [SerializeField] float m_ColorAlpha = 120;
    [SerializeField] Image m_myImage = null;
    void Start()
    {
        m_myImage = GetComponent<Image>();
        m_myCommandPanel = transform.parent.gameObject;
        if (m_nextCommandPanel != null)
        {
            m_nextCommandPanel.SetActive(false);
        }
    }

    void Update()
    {
        
    }
    public void OnUI()
    {
        if (m_nextCommandPanel != null)
        {
            CommandPanelChanged();
        }
        else
        {
            CommandSelectedAction();
        }
    }
    public void CommandPanelChanged()
    {
        m_myCommandPanel.SetActive(false);
        m_nextCommandPanel.SetActive(true);
    }
    public void OnUIPanelReturnChanged()
    {
        if (m_beforeCommandPanel != null)
        {
            m_myCommandPanel.SetActive(false);
            m_beforeCommandPanel.SetActive(true);
        }
    }
    public void CommandSelectedAction()
    {
        int num = this.GetComponentInParent<UIController>().m_selectedCommandNumber;
        GameObject.FindGameObjectWithTag("Player").GetComponent<BattleStateMachine>().m_targetNumber = num;
        GameObject.FindGameObjectWithTag("Player").GetComponent<BattleStateMachine>().m_action = true;
        m_myCommandPanel.SetActive(false);
        Debug.Log("コマンドけし");
    }
    public void SelectedCommandColorChange()
    {
        m_myImage.color = new Color( 1, 1, 1, m_ColorAlpha / 255);
        
    }
    public void NonCommandColorChange()
    {
        m_myImage.color = new Color(1, 1, 1, 0);
    }
}
