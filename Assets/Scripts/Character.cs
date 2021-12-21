using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : CharacterParameter, ITakableDamage
{
    public void SetUp()
    {

    }

    public void OnUpdate()
    {
        if (HP.Value >= MaxHP.Value)
        {
            HP.Value = MaxHP.Value;
        }
        else if (HP.Value < 1)
        {
            HP.Value = 0;
        }
        if (AP.Value >= MaxAP.Value)
        {
            AP.Value = MaxAP.Value;
        }
        else if (AP.Value < 1)
        {
            AP.Value = 0;
        }
    }
    public virtual void TakeDamage(int damage)
    {
        if (HP.Value - damage < 1)
        {
            HP.Value = 0;
        }
        else
        {
            HP.Value -= damage;
        }
    }
    void LevelUp()
    {
        Level.Value++;
        CalculateNextExp(Level.Value);
        ParameterUp(Level.Value);
    }
    public void GetExp(int getExp)
    {
        CalculateExp(getExp);
    }

    void CalculateExp(int getExp)
    {
        NowExp.Value += getExp;
        TotalExp.Value += getExp;
        if (NextExp.Value <= NowExp.Value)
        {
            int exp = NextExp.Value - NowExp.Value;
            if (Level.Value != 100)
            {
                LevelUp();
                NowExp.Value -= exp;
            }
            else
            {
                NowExp.Value = 0;
            }
        }
    }

    void CalculateNextExp(int level)
    {
        if (level % 10 == 0)
        {
            m_levelCorrection *= 1.002f;
        }
        if (level - 10 <= 1)
        {
            NextExp.Value = (int)((1 * m_charaCorrection * level * level + level * 2) * m_levelCorrection);
        }
        else
        {
            NextExp.Value = (int)(((level - 10) * m_charaCorrection * level * level + level * 2) * m_levelCorrection);
        }
    }
    void ParameterUp(int level)//上昇の仕方は検討中
    {
        int baseParameter = level % 5 + 1;
        MaxHP.Value += baseParameter;
        HP.Value += baseParameter;
        MaxAP.Value += baseParameter;
        AP.Value += baseParameter;
        Strength.Value += baseParameter;
        Defense.Value += baseParameter;
        MagicPower.Value += baseParameter;
        MagicResist.Value += baseParameter;
        Speed.Value += baseParameter;
        Luck.Value++;
        SkillPoint.Value += baseParameter;
    }
}
