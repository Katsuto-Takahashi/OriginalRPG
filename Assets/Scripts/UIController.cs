using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    /// <summary>コマンドとして使うImageのList</summary>
    [SerializeField] private List<Image> m_commandList = new List<Image>();
    /// <summary>短押しされているかどうか</summary>
    private bool m_isDad;
    /// <summary>長押しされていると判定する時間</summary>
    private float m_downTime = 0.2f;
    /// <summary>長押し中に処理をするときのインターバル</summary>
    private float m_interval = 0.1f;
    /// <summary>インターバルごとに保持しておく時間</summary>
    float m_saveTime = 0f;
    /// <summary>D-padが押されている時間</summary>
    float m_getTime = 0f;
    /// <summary>select状態にあるコマンドのインデックス</summary>
    public int m_selectedCommandNumber = 0;

    GridLayoutGroup m_gg;
    /// <summary>Dpadの横方向の入力の値</summary>
    int m_horiCount = 0;

    private void OnEnable()
    {
        GoToZero();
    }
    void Start()
    {
        m_gg = this.GetComponent<GridLayoutGroup>();
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Dpad_v");
        float h = Input.GetAxisRaw("Dpad_h");
        CommandMove(v, h);
        if (Input.GetButtonDown("Circlebutton"))
        {
            m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().OnUI();
        }
        else if (Input.GetButtonDown("×button"))
        {
            m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().OnUIPanelReturnChanged();
        }
    }
    /// <summary>Dpadでのコマンド操作</summary>
    /// <param name="v">Dpadの縦方向の入力</param>
    /// <param name="h">Dpadの横方向の入力</param>
    private void CommandMove(float v, float h)
    {
        if (v < 0)
        {
            //短押し
            if (!m_isDad)
            {
                Debug.Log("Dpad_v　↓");
                if (m_gg.constraintCount == 1)
                {
                    VerticalCommandChenge(v);
                }
                else
                {
                    VerticalAndHorizontalCommandChenge(v, h);
                }
            }

            m_isDad = true;
            m_getTime += Time.deltaTime;

            //長押し
            if (m_getTime > m_downTime)
            {
                //Debug.Log(getTime_v);
                if (m_getTime - m_saveTime > m_interval)
                {
                    m_saveTime = m_getTime;
                    Debug.Log("Dpad_v　↓ long");
                    if (m_gg.constraintCount == 1)
                    {
                        ContinuousVerticalCommandChenge(v);
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
            //短押し
            if (!m_isDad)
            {
                Debug.Log("Dpad_v　↑");
                if (m_gg.constraintCount == 1)
                {
                    VerticalCommandChenge(v);
                }
                else
                {
                    VerticalAndHorizontalCommandChenge(v, h);
                }
            }

            m_isDad = true;
            m_getTime += Time.deltaTime;

            //長押し
            if (m_getTime > m_downTime)
            {
                //Debug.Log(getTime_v);
                if (m_getTime - m_saveTime > m_interval)
                {
                    m_saveTime = m_getTime;
                    Debug.Log("Dpad_v　↑ long");
                    if (m_gg.constraintCount == 1)
                    {
                        ContinuousVerticalCommandChenge(v);
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
            //短押し
            if (!m_isDad)
            {
                Debug.Log("Dpad_h　←");
                if (m_gg.constraintCount == 1)
                {

                }
                else
                {
                    VerticalAndHorizontalCommandChenge(v, h);
                }
            }

            m_isDad = true;
            m_getTime += Time.deltaTime;

            //長押し
            if (m_getTime > m_downTime)
            {
                //Debug.Log(getTime_h);
                if (m_getTime - m_saveTime > m_interval)
                {
                    m_saveTime = m_getTime;
                    Debug.Log("Dpad_h　← long");
                    if (m_gg.constraintCount == 1)
                    {
                        
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
            //短押し
            if (!m_isDad)
            {
                Debug.Log("Dpad_h　→");
                if (m_gg.constraintCount == 1)
                {

                }
                else
                {
                    VerticalAndHorizontalCommandChenge(v, h);
                }
            }

            m_isDad = true;
            m_getTime += Time.deltaTime;

            //長押し
            if (m_getTime > m_downTime)
            {
                //Debug.Log(getTime_h);
                if (m_getTime - m_saveTime > m_interval)
                {
                    m_saveTime = m_getTime;
                    Debug.Log("Dpad_h　→ long");
                    if (m_gg.constraintCount == 1)
                    {

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
            m_isDad = false;
            m_getTime = 0f;
            m_saveTime = 0f;
        }
    }
    /// <summary>選択しているコマンドの縦方向の変更</summary>
    /// <param name="v">Dpadの縦方向の入力</param>
    private void VerticalCommandChenge(float v)
    {
        if (v < 0)
        {
            m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
            if (m_selectedCommandNumber == m_commandList.Count - 1)
            {
                m_selectedCommandNumber = 0;
            }
            else
            {
                m_selectedCommandNumber++;
            }
        }
        else if (v > 0)
        {
            m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
            if (m_selectedCommandNumber == 0)
            {
                m_selectedCommandNumber = m_commandList.Count - 1;
            }
            else
            {
                m_selectedCommandNumber--;
            }
        }
        m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().SelectedCommandColorChange();
    }
    /// <summary>選択しているコマンドの継続的な縦方向の変更</summary>
    /// <param name="v">Dpadの縦方向の入力</param>
    private void ContinuousVerticalCommandChenge(float v)
    {
        if (v < 0)
        {
            if (m_selectedCommandNumber != m_commandList.Count - 1)
            {
                m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
                m_selectedCommandNumber++;
            }
        }
        else if (v > 0)
        {
            if (m_selectedCommandNumber != 0)
            {
                m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
                m_selectedCommandNumber--;
            }
        }
        m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().SelectedCommandColorChange();
    }
    /// <summary>選択しているコマンドの縦横方向の変更</summary>
    /// <param name="v">Dpadの縦方向の入力</param>
    /// <param name="h">Dpadの横方向の入力</param>
    private void VerticalAndHorizontalCommandChenge(float v, float h)
    {
        int amari = m_commandList.Count % m_gg.constraintCount;
        if (v < 0)
        {
            m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
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
            m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
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
            m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
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
            m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
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
        m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().SelectedCommandColorChange();
    }
    /// <summary>選択しているコマンドの継続的な縦横方向の変更</summary>
    /// <param name="v">Dpadの縦方向の入力</param>
    /// <param name="h">Dpadの横方向の入力</param>
    private void ContinuousVerticalAndHorizontalCommandChenge(float v, float h)
    {
        if (v < 0)
        {
            if (m_selectedCommandNumber + m_gg.constraintCount <= m_commandList.Count - 1)
            {
                m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
                m_selectedCommandNumber += m_gg.constraintCount;
            }
        }
        else if (v > 0)
        {
            if (m_selectedCommandNumber - m_gg.constraintCount >= 0)
            {
                m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
                m_selectedCommandNumber -= m_gg.constraintCount;
            }
        }
        else if (h > 0)
        {
            if (m_horiCount < m_gg.constraintCount - 1)
            {
                if (m_selectedCommandNumber < m_commandList.Count - 1)
                {
                    m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
                    m_selectedCommandNumber++;
                    m_horiCount++;
                }
            }
        }
        else if (h < 0)
        {
            if (m_horiCount > 0)
            {
                m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
                m_selectedCommandNumber--;
                m_horiCount--;
            }
        }
        m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().SelectedCommandColorChange();
    }

    private void GoToZero()
    {
        m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().NonCommandColorChange();
        m_selectedCommandNumber = 0;
        m_commandList[m_selectedCommandNumber].GetComponent<UIChanger>().SelectedCommandColorChange();
        m_horiCount = 0;
    }
}
