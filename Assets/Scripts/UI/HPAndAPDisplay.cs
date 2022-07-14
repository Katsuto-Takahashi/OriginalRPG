using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HPAndAPDisplay : SingletonMonoBehaviour<HPAndAPDisplay>, IManagable
{
    [SerializeField] 
    bool m_isDisplay = false;

    [SerializeField] 
    GameObject m_prefabObject = null;

    GameObject m_ui;

    List<CharacterParameterUI> m_gameObjects = new List<CharacterParameterUI>();

    int m_memberNumber = 1;

    int m_beforeMemberNumber;

    List<Character> m_cm = new List<Character>();

    void Start()
    {
        GameManager.Instance.Party.ObserveCountChanged().Subscribe(num => CheckMember(num)).AddTo(this);
    }

    void CheckMember(int num)
    {
        Debug.Log("人数変化");
        Debug.Log($"人数{num}");
        m_memberNumber = num;
        Change();
        ReflectionParam();
        m_beforeMemberNumber = m_memberNumber;
    }

    void ChangeText(int index)
    {
        Debug.Log("チェンジ");

        var ui = m_gameObjects[index];
        ui.CreateName(m_cm[index].Name.Value);
        ui.CreateParameter(
            m_cm[index].HP.Value,
            m_cm[index].MaxHP.Value,
            m_cm[index].AP.Value,
            m_cm[index].MaxAP.Value
            );
    }

    void ReflectionParam()
    {
        for (int i = 0; i < m_cm.Count; i++)
        {
            m_cm[i].HP.DistinctUntilChanged().Subscribe(_ => ChangeText(i)).AddTo(m_gameObjects[i]);
            m_cm[i].MaxHP.DistinctUntilChanged().Subscribe(_ => ChangeText(i)).AddTo(m_gameObjects[i]);
            m_cm[i].AP.DistinctUntilChanged().Subscribe(_ => ChangeText(i)).AddTo(m_gameObjects[i]);
            m_cm[i].MaxAP.DistinctUntilChanged().Subscribe(_ => ChangeText(i)).AddTo(m_gameObjects[i]);
        }
    }

    void Create(int partyIndex)
    {
        m_ui = Instantiate(m_prefabObject);
        m_ui.transform.SetParent(transform, false);
        m_gameObjects.Add(m_ui.GetComponent<CharacterParameterUI>());
        m_cm.Add(GameManager.Instance.Party[partyIndex]);
    }

    void Delete(int index)
    {
        Destroy(transform.GetChild(index - 1).gameObject);
        m_gameObjects.Remove(m_gameObjects[index - 1]);
        m_cm.Remove(m_cm[index - 1]);
    }

    void Change()
    {
        for (int i = m_beforeMemberNumber; i > 0; i--)
        {
            Delete(i);
        }
        
        for (int i = 0; i < m_memberNumber; i++)
        {
            Create(i);
        }
    }

    public void Initialize()
    {
        m_memberNumber = GameManager.Instance.Party.Count;
        m_beforeMemberNumber = m_memberNumber;
        for (int i = 0; i < m_beforeMemberNumber; i++)
        {
            Create(i);
        }
        ReflectionParam();
    }
}
