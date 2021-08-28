using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, ITakableDamage
{
    [SerializeField] EnemyParameters enemyParameters = null;
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
        
    }

    public virtual void TakeDamage(int damage)
    {
        Debug.Log(HP);
        HP -= damage;
        Debug.Log(HP);
    }
}
