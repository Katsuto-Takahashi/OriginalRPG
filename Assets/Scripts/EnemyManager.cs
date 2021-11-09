using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, ITakableDamage
{
    [SerializeField] EnemyParameters m_enemyParameters = null;
    public EnemyParameters EnemyParameters { get => m_enemyParameters;private set => m_enemyParameters = value; }
    //　毒状態かどうか
    [SerializeField]
    private bool isPoisonState = false;
    public bool IsPoisonState { get => isPoisonState; set => isPoisonState = value; }
    //　痺れ状態かどうか
    [SerializeField]
    private bool isNumbnessState = false;
    public bool IsNumbnessState { get => isNumbnessState; set => isNumbnessState = value; }
    //　HPがあるかどうか
    [SerializeField]
    private bool isDeadState = false;
    public bool IsDeadState { get => isDeadState; set => isDeadState = value; }
    private int nowHP = 0;
    public int HP
    {
        get { return nowHP; }
        set { nowHP = value; }
    }
    private int nowAP = 0;
    public int AP
    {
        get { return nowAP; }
        set { nowAP = value; }
    }
    public enum EnemyState
    {
        Idle,
        Move,
        Search,
        Attack,
        Death
    }
    public EnemyState enemyState = EnemyState.Idle;
    void Start()
    {
        HP = EnemyParameters.MaxHP;
        AP = EnemyParameters.MaxAP;
        for (int i = 0; i < EnemyParameters.attackAttributeResistance.Length; i++)
        {
            EnemyParameters.attackAttributeResistance[i] = float.Parse(EnemyParameters.attackAttributeResistance[i].ToString("f1"));
            if (EnemyParameters.attackAttributeResistance[i] <= 0.0f)
            {
                EnemyParameters.attackAttributeResistance[i] = 0.0f;
            }
            else if (EnemyParameters.attackAttributeResistance[i] >= 2.0f)
            {
                EnemyParameters.attackAttributeResistance[i] = 2.0f;
            }
        }
        for (int i = 0; i < EnemyParameters.attackTypeResistance.Length; i++)
        {
            EnemyParameters.attackTypeResistance[i] = float.Parse(EnemyParameters.attackTypeResistance[i].ToString("f1"));
            if (EnemyParameters.attackTypeResistance[i] <= 0.0f)
            {
                EnemyParameters.attackTypeResistance[i] = 0.0f;
            }
            else if (EnemyParameters.attackTypeResistance[i] >= 2.0f)
            {
                EnemyParameters.attackTypeResistance[i] = 2.0f;
            }
        }
    }

    void Update()
    {
        if (HP >= EnemyParameters.MaxHP)
        {
            HP = EnemyParameters.MaxHP;
        }
        else if (HP <= 0)
        {
            HP = 0;
            IsDeadState = true;
        }
        if (AP >= EnemyParameters.MaxAP)
        {
            AP = EnemyParameters.MaxAP;
        }
        else if (AP < 1)
        {
            AP = 0;
        }
    }
    public virtual void TakeDamage(int damage)
    {
        Debug.Log(HP);
        HP -= damage;
        Debug.Log(HP);
    }
}
