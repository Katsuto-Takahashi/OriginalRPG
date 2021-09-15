using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, ITakableDamage
{
    public EnemyParameters enemyParameters = null;
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
        HP = enemyParameters.MaxHP;
        AP = enemyParameters.MaxAP;
        for (int i = 0; i < enemyParameters.attackAttributeResistance.Length; i++)
        {
            enemyParameters.attackAttributeResistance[i] = float.Parse(enemyParameters.attackAttributeResistance[i].ToString("f1"));
            if (enemyParameters.attackAttributeResistance[i] <= 0.0f)
            {
                enemyParameters.attackAttributeResistance[i] = 0.0f;
            }
            else if (enemyParameters.attackAttributeResistance[i] >= 2.0f)
            {
                enemyParameters.attackAttributeResistance[i] = 2.0f;
            }
        }
        for (int i = 0; i < enemyParameters.attackTypeResistance.Length; i++)
        {
            enemyParameters.attackTypeResistance[i] = float.Parse(enemyParameters.attackTypeResistance[i].ToString("f1"));
            if (enemyParameters.attackTypeResistance[i] <= 0.0f)
            {
                enemyParameters.attackTypeResistance[i] = 0.0f;
            }
            else if (enemyParameters.attackTypeResistance[i] >= 2.0f)
            {
                enemyParameters.attackTypeResistance[i] = 2.0f;
            }
        }
    }

    void Update()
    {
        if (HP >= enemyParameters.MaxHP)
        {
            HP = enemyParameters.MaxHP;
        }
        else if (HP <= 0)
        {
            HP = 0;
            IsDeadState = true;
        }
        if (AP >= enemyParameters.MaxAP)
        {
            AP = enemyParameters.MaxAP;
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
