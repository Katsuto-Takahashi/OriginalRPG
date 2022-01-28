using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    [SerializeField] Text m_messageText = null;
    //[SerializeField] Text m_nameText = null;
    [SerializeField] float m_waitTime = 0f;
    [SerializeField] List<string> m_message = new List<string>();
    int m_count;
    bool m_skipMessage;
    bool m_nextMessage;

    bool m_stop;
    bool m_click;
    bool m_skipMode;
    bool m_autoMode;
    void Start()
    {
        m_count = 0;
        MessageWindows(m_message);
    }

    void Update()
    {
        if (!Stop())
        {
            if (Click() && m_count < m_message.Count)
            {
                m_skipMessage = true;
            }
            if (m_skipMessage || Auto())
            {
                if (Click() || Auto())
                {
                    m_nextMessage = true;
                }
            }
            if (m_click)
            {
                m_click = false;
            }
        }
    }

    bool Click()
    {
        return m_click || Skip();
    }

    bool Skip()
    {
        return m_skipMode;
    }

    bool Auto()
    {
        return m_autoMode;
    }

    bool Stop()
    {
        return m_stop = Input.GetKey(KeyCode.Space);
    }

    public void MessageWindows(List<string> messageList)
    {
        StartCoroutine(Message(messageList));
    }

    IEnumerator Message(List<string> messageList)
    {
        m_nextMessage = false;
        m_skipMessage = false;
        int wordCount = 0;
        m_messageText.text = "";
        while (messageList[m_count].Length > wordCount)
        {
            if (m_skipMessage)
            {
                m_messageText.text = "";
                m_messageText.text = messageList[m_count];
                break;
            }
            m_messageText.text += messageList[m_count][wordCount];
            wordCount++;
            yield return new WaitForSeconds(m_waitTime);
        }
        
        m_count++;
        StartCoroutine(WaitMessage(messageList));
    }

    IEnumerator WaitMessage(List<string> messageList)
    {
        m_nextMessage = false;
        while (!m_nextMessage)
        {
            yield return new WaitForEndOfFrame();
        }

        if (m_count < m_message.Count)
        {
            MessageWindows(messageList);
        }
        else
        {
            //話が終わった後の処理
            yield return new WaitForSeconds(2f);
            m_count = 0;
            MessageWindows(messageList);
        }
    }

    public void OnClick()
    {
        if (m_click) m_click = false;
        else m_click = true;
    }

    public void OnSkip()
    {
        if (m_skipMode) m_skipMode = false;
        else m_skipMode = true;
    }

    public void OnAuto()
    {
        if (m_autoMode) m_autoMode = false;
        else m_autoMode = true;
    }

    public IEnumerator FadeIn()
    {
        while (!m_skipMessage)
        {
            yield return null;
        }
    }
    public IEnumerator FadeOut()
    {
        while (!m_skipMessage)
        {
            yield return null;
        }
    }

    //１キャラを出しながらテキスト
    //キャラを順に出しながらテキスト
    //キャラを同時に出しながらテキスト
    //１キャラを出してからテキスト
    //キャラを順に出してからテキスト
    //キャラを同時に出してからテキスト
    //１キャラを消しながらテキスト
    //キャラを順に消しながらテキスト
    //キャラを同時に消しながらテキスト
    //１キャラを消してからテキスト
    //キャラを順に消してからテキスト
    //キャラを同時に消してからテキスト
}
