using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Root : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    Node m_myChildNode;

    void Start()
    {
        if (m_myChildNode != null)
        {
            Debug.Log($"子は{m_myChildNode}");
            m_myChildNode.GetNode();
            Debug.Log(m_myChildNode.Result());
        }
        else
        {
            Debug.Log("子がない");
        }
    }

    void Update()
    {
        
    }


    public enum 装備中
    {
        大剣,
        太刀
    }

    public enum 攻撃の場所
    {
        地上,
        空中
    }

    [SerializeField]
    Dictionary<装備中, 武器> データ = new Dictionary<装備中, 武器>();
    
    public 技データ 攻撃(装備中 wepon, 攻撃の場所 place, int index)
    {
        switch (place)
        {
            case 攻撃の場所.地上:
                return データ[wepon].技[index];
            case 攻撃の場所.空中:
                return データ[wepon].空中技[index];
            default:
                return default;
        }
    }
}

[System.Serializable]
public class 武器
{
    public int combo = 2;
    [SerializeField]
    public List<技データ> 技 = new List<技データ>();
    public List<技データ> 空中技 = new List<技データ>();
}

[System.Serializable]
public class 技データ
{
    [SerializeField]
    public int attack = 1;
}