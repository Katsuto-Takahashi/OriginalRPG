using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField, Tooltip("コマンドとして使うImageのList")] 
    protected List<Image> m_commandList = new List<Image>();
    [Tooltip("長押し判定")]
    bool m_isLongDown = false;
    [Tooltip("長押しと判定する時間")]
    float m_downTime = 0.2f;
    [Tooltip("長押し中に処理をするときのインターバル")]
    float m_interval = 0.1f;
    [Tooltip("インターバルごとに保持しておく時間")]
    float m_saveTime = 0f;
    [Tooltip("padが押されている時間")]
    float m_getTime = 0f;
    [Tooltip("select状態にあるコマンドのインデックス")]
    protected int m_selectedCommandNumber = 0;
    [SerializeField, Tooltip("選択中のコマンドのα値")]
    float m_ColorAlpha = 120f;
    [Tooltip("GridLayoutGroup")]
    GridLayoutGroup m_gg;
    [Tooltip("Dpadの横方向の入力の回数")]
    int m_horiCount = 0;
    [SerializeField, Tooltip("現在のコマンドパネルから変化するパネルList")]
    List<Command> m_changeCommandList = new List<Command>();
    enum ChangeCommand
    {
        Before,
        Next,
        SpecialBefore,
        SpecialNext
    }
    public bool m_special = false;
    void OnEnable()
    {
        GoToZero();
    }

    void Start()
    {
        StartSet();
    }

    protected virtual void StartSet()
    {
        m_gg = GetComponent<GridLayoutGroup>();
    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        float v = InputController.Instance.CommandMove().y;
        float h = InputController.Instance.CommandMove().x;
        CommandMove(v, h);
        if (InputController.Instance.Decide())
        {
            OnUI();
        }
        //else if (Input.GetButtonDown("×button"))
        //{
        //    OnUIPanelReturnChanged();
        //}
    }

    /// <summary>Dpadでのコマンド操作</summary>
    /// <param name="v">Dpadの縦方向の入力</param>
    /// <param name="h">Dpadの横方向の入力</param>
    void CommandMove(float v, float h)
    {
        if (v < 0)
        {
            if (!m_isLongDown)
            {
                if (m_gg.constraintCount == 1)
                {
                    CommandChenge(v);
                }
                else
                {
                    VerticalAndHorizontalCommandChenge(v, h);
                }
            }

            m_isLongDown = true;
            m_getTime += Time.deltaTime;

            if (m_getTime > m_downTime)
            {
                if (m_getTime - m_saveTime > m_interval)
                {
                    m_saveTime = m_getTime;
                    if (m_gg.constraintCount == 1)
                    {
                        ContinuousCommandChenge(v);
                    }
                    else
                    {
                        ContinuousVerticalAndHorizontalCommandChenge(v, h);
                    }
                }
            }
        }
        else if (v > 0)
        {
            if (!m_isLongDown)
            {
                if (m_gg.constraintCount == 1)
                {
                    CommandChenge(v);
                }
                else
                {
                    VerticalAndHorizontalCommandChenge(v, h);
                }
            }

            m_isLongDown = true;
            m_getTime += Time.deltaTime;

            if (m_getTime > m_downTime)
            {
                if (m_getTime - m_saveTime > m_interval)
                {
                    m_saveTime = m_getTime;
                    if (m_gg.constraintCount == 1)
                    {
                        ContinuousCommandChenge(v);
                    }
                    else
                    {
                        ContinuousVerticalAndHorizontalCommandChenge(v, h);
                    }
                }
            }
        }
        else if (h < 0)
        {
            if (!m_isLongDown)
            {
                if (m_gg.constraintCount == 1)
                {
                    CommandChenge(h);
                }
                else
                {
                    VerticalAndHorizontalCommandChenge(v, h);
                }
            }

            m_isLongDown = true;
            m_getTime += Time.deltaTime;

            if (m_getTime > m_downTime)
            {
                if (m_getTime - m_saveTime > m_interval)
                {
                    m_saveTime = m_getTime;
                    if (m_gg.constraintCount == 1)
                    {
                        ContinuousCommandChenge(h);
                    }
                    else
                    {
                        ContinuousVerticalAndHorizontalCommandChenge(v, h);
                    }
                }
            }
        }
        else if (h > 0)
        {
            if (!m_isLongDown)
            {
                if (m_gg.constraintCount == 1)
                {
                    CommandChenge(h);
                }
                else
                {
                    VerticalAndHorizontalCommandChenge(v, h);
                }
            }

            m_isLongDown = true;
            m_getTime += Time.deltaTime;

            if (m_getTime > m_downTime)
            {
                if (m_getTime - m_saveTime > m_interval)
                {
                    m_saveTime = m_getTime;
                    if (m_gg.constraintCount == 1)
                    {
                        ContinuousCommandChenge(h);
                    }
                    else
                    {
                        ContinuousVerticalAndHorizontalCommandChenge(v, h);
                    }
                }
            }
        }
        else
        {
            m_isLongDown = false;
            m_getTime = 0f;
            m_saveTime = 0f;
        }
    }

    /// <summary>選択しているコマンドの変更</summary>
    /// <param name="input">Dpadの方向の入力</param>
    void CommandChenge(float input)
    {
        if (input < 0)
        {
            NonCommandColorChange();
            if (m_selectedCommandNumber == m_commandList.Count - 1)
            {
                m_selectedCommandNumber = 0;
            }
            else
            {
                m_selectedCommandNumber++;
            }
        }
        else if (input > 0)
        {
            NonCommandColorChange();
            if (m_selectedCommandNumber == 0)
            {
                m_selectedCommandNumber = m_commandList.Count - 1;
            }
            else
            {
                m_selectedCommandNumber--;
            }
        }
        SelectedCommandColorChange();
    }

    /// <summary>選択しているコマンドの継続的な変更</summary>
    /// <param name="input">Dpadの方向の入力</param>
    void ContinuousCommandChenge(float input)
    {
        if (input < 0)
        {
            if (m_selectedCommandNumber != m_commandList.Count - 1)
            {
                NonCommandColorChange();
                m_selectedCommandNumber++;
            }
        }
        else if (input > 0)
        {
            if (m_selectedCommandNumber != 0)
            {
                NonCommandColorChange();
                m_selectedCommandNumber--;
            }
        }
        SelectedCommandColorChange();
    }

    /// <summary>選択しているコマンドの縦横方向の変更</summary>
    /// <param name="v">Dpadの縦方向の入力</param>
    /// <param name="h">Dpadの横方向の入力</param>
    void VerticalAndHorizontalCommandChenge(float v, float h)
    {
        int amari = m_commandList.Count % m_gg.constraintCount;
        if (v < 0)
        {
            NonCommandColorChange();
            if (m_selectedCommandNumber + m_gg.constraintCount > m_commandList.Count - 1)
            {
                if (amari != 0)
                {
                    m_selectedCommandNumber = m_selectedCommandNumber + m_gg.constraintCount - (m_commandList.Count + m_gg.constraintCount - amari);
                    if (m_selectedCommandNumber < 0)
                    {
                        m_selectedCommandNumber = m_commandList.Count - 1;
                        m_horiCount = amari - 1;
                    }
                }
                else
                {
                    m_selectedCommandNumber = m_selectedCommandNumber + m_gg.constraintCount - m_commandList.Count;
                }
            }
            else
            {
                m_selectedCommandNumber += m_gg.constraintCount;
            }
        }
        else if (v > 0)
        {
            NonCommandColorChange();
            if (m_selectedCommandNumber - m_gg.constraintCount < 0)
            {
                if (amari != 0)
                {
                    m_selectedCommandNumber = m_selectedCommandNumber - m_gg.constraintCount + (m_commandList.Count + m_gg.constraintCount - amari);
                    if (m_selectedCommandNumber > m_commandList.Count - 1)
                    {
                        m_selectedCommandNumber -= m_gg.constraintCount;
                    }
                }
                else
                {
                    m_selectedCommandNumber = m_selectedCommandNumber - m_gg.constraintCount + m_commandList.Count;
                }
            }
            else
            {
                m_selectedCommandNumber -= m_gg.constraintCount;
            }
        }
        else if (h > 0)
        {
            NonCommandColorChange();
            if (m_horiCount == m_gg.constraintCount - 1)
            {
                m_selectedCommandNumber -= (m_gg.constraintCount - 1);
                m_horiCount = 0;
            }
            else
            {
                m_selectedCommandNumber++;
                m_horiCount++;
            }
        }
        else if (h < 0)
        {
            NonCommandColorChange();
            if (m_horiCount == 0)
            {
                m_selectedCommandNumber += (m_gg.constraintCount - 1);
                m_horiCount = m_gg.constraintCount - 1;
            }
            else
            {
                m_selectedCommandNumber--;
                m_horiCount--;
            }
        }
        SelectedCommandColorChange();
    }

    /// <summary>選択しているコマンドの継続的な縦横方向の変更</summary>
    /// <param name="v">Dpadの縦方向の入力</param>
    /// <param name="h">Dpadの横方向の入力</param>
    void ContinuousVerticalAndHorizontalCommandChenge(float v, float h)
    {
        if (v < 0)
        {
            if (m_selectedCommandNumber + m_gg.constraintCount <= m_commandList.Count - 1)
            {
                NonCommandColorChange();
                m_selectedCommandNumber += m_gg.constraintCount;
            }
        }
        else if (v > 0)
        {
            if (m_selectedCommandNumber - m_gg.constraintCount >= 0)
            {
                NonCommandColorChange();
                m_selectedCommandNumber -= m_gg.constraintCount;
            }
        }
        else if (h > 0)
        {
            if (m_horiCount < m_gg.constraintCount - 1)
            {
                if (m_selectedCommandNumber < m_commandList.Count - 1)
                {
                    NonCommandColorChange();
                    m_selectedCommandNumber++;
                    m_horiCount++;
                }
            }
        }
        else if (h < 0)
        {
            if (m_horiCount > 0)
            {
                NonCommandColorChange();
                m_selectedCommandNumber--;
                m_horiCount--;
            }
        }
        SelectedCommandColorChange();
    }

    protected virtual void GoToZero()
    {
        NonCommandColorChange();
        m_selectedCommandNumber = 0;
        SelectedCommandColorChange();
        m_horiCount = 0;
    }

    void OnUI()
    {
        if (m_changeCommandList[m_selectedCommandNumber].m_commandPanelList[(int)ChangeCommand.Next] != null)
        {
            CommandPanelChanged();
        }
        else
        {
            CommandSelectedAction();
        }
    }

    void CommandPanelChanged()
    {
        if (m_special)
        {
            m_changeCommandList[m_selectedCommandNumber].m_commandPanelList[(int)ChangeCommand.SpecialNext].SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            m_changeCommandList[m_selectedCommandNumber].m_commandPanelList[(int)ChangeCommand.Next].SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void OnUIPanelReturnChanged()
    {
        if (m_changeCommandList[m_selectedCommandNumber].m_commandPanelList[(int)ChangeCommand.Before] != null)
        {
            if (m_special)
            {
                m_changeCommandList[m_selectedCommandNumber].m_commandPanelList[(int)ChangeCommand.SpecialBefore].SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                m_changeCommandList[m_selectedCommandNumber].m_commandPanelList[(int)ChangeCommand.Before].SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    protected virtual void CommandSelectedAction() { }

    void SelectedCommandColorChange()
    {
        m_commandList[m_selectedCommandNumber].color = new Color(1, 1, 1, m_ColorAlpha / 255);
    }

    void NonCommandColorChange()
    {
        m_commandList[m_selectedCommandNumber].color = new Color(1, 1, 1, 0);
    }
}
[Serializable]
public class Command
{
    enum ChangeCommand
    {
        Before,
        Next,
        SpecialBefore,
        SpecialNext
    }
    [EnumIndex(typeof(ChangeCommand))]
    public GameObject[] m_commandPanelList = { };
}
