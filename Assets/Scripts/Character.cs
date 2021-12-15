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
        
    }
    public virtual void TakeDamage(int damage)
    {
        HP.Value -= damage;
        if (HP.Value < 1)
        {
            HP.Value = 0;
        }
        //m_UIDisplay.ChangeUI();
    }
    void LevelUp()
    {
        //LevelUP = true;
        Level.Value++;
        CalculateNextExp(Level.Value);
        ParameterUp(Level.Value);
        //m_UIDisplay.ChangeUI();
    }
    public void GetExp(int getExp)
    {
        CalculateExp(getExp);
    }

    void CalculateExp(int getExp)
    {
        NowExp.Value += getExp;
        TotalExp.Value += getExp;
        if (NextExp.Value - NowExp.Value <= 0)
        {
            int exp = NextExp.Value - NowExp.Value;
            NowExp.Value = 0;
            if (Level.Value != 100)
            {
                LevelUp();
                NowExp.Value -= exp;
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
    void ParameterUp(int level)
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
