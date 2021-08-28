using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>ロードするシーン名</summary>
    [SerializeField] string m_sceneNameToBeLoaded = "SceneNameToBeLoaded";
    /// <summary>フェードするためのマスク</summary>
    [SerializeField] Image m_fadePanel = null;
    /// <summary>フェードするスピード</summary>
    [SerializeField] float m_fadeSpeed = 1f;
    /// <summary>ロード開始フラグ</summary>
    bool m_isLoadStarted = false;

    private void Update()
    {
        if (m_isLoadStarted)
        {
            if (m_fadePanel != null)
            {
                Color panelColor = m_fadePanel.color;
                panelColor.a += m_fadeSpeed * Time.deltaTime;
                m_fadePanel.color = panelColor;

                if (panelColor.a > 0.99f)
                {
                    SceneManager.LoadScene(m_sceneNameToBeLoaded);
                    m_isLoadStarted = false;
                }
            }
            else
            {
                SceneManager.LoadScene(m_sceneNameToBeLoaded);
                m_isLoadStarted = false;
            }
        }
    }
    public void LoadScene(string loadSceneName)
    {
        m_isLoadStarted = true;
        m_sceneNameToBeLoaded = loadSceneName;
    }

    public void LoadedBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
