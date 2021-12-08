using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterParameterManager : CharacterParameters, ITakableDamage
{
    float m_levelCorrection = 4.8f;
    float m_charaCorrection = 0.07f;
    //int baseParameter = 0;
    ////　装備している武器
    //[SerializeField]
    //ItemData equipWeapon = null;
    ////　装備している鎧
    //[SerializeField]
    //ItemData equipArmor = null;
    //　毒状態かどうか
    [SerializeField]
    bool isPoisonState = false;
    public bool IsPoisonState { get => isPoisonState; set => isPoisonState = value; }
    //　痺れ状態かどうか
    [SerializeField]
    bool isNumbnessState = false;
    public bool IsNumbnessState { get => isNumbnessState; set => isNumbnessState = value; }
    //　HPがあるかどうか
    [SerializeField]
    bool isDeadState = false;
    public bool IsDeadState { get => isDeadState; set => isDeadState = value; }
    bool levelUP = false;
    public bool LevelUP { get => levelUP; set => levelUP = value; }
    [SerializeField] HPAndAPDisplay m_UIDisplay = null;

    void Start()
    {
    }

    void Update()
    {
        if (NowHP >= MaxHP)
        {
            NowHP = MaxHP;
        }
        else if (NowHP < 1)
        {
            NowHP = 0;
            IsDeadState = true;
        }
        else
        {
            IsDeadState = false;
        }
        if (NowAP >= MaxAP)
        {
            NowAP = MaxAP;
        }
        else if (NowAP < 1)
        {
            NowAP = 0;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        NowHP -= damage;
        if (NowHP < 1)
        {
            NowHP = 0;
        }
        m_UIDisplay.ChangeUI();
    }
    void LevelUp()
    {
        LevelUP = true;
        Level++;
        CalculateNextExp(Level);
        ParameterUp(Level);
        m_UIDisplay.ChangeUI();
    }
    public void GetExp(int getExp)
    {
        CalculateExp(getExp);
    }

    void CalculateExp(int getExp)
    {
        NowExp += getExp;
        TotalExp += getExp;
        if (NextExp - NowExp <= 0)
        {
            int exp = NextExp - NowExp;
            NowExp = 0;
            if (Level != 100)
            {
                LevelUp();
                NowExp -= exp;
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
            NextExp = (int)((1 * m_charaCorrection * level * level + level * 2) * m_levelCorrection);
        }
        else
        {
            NextExp = (int)(((level - 10) * m_charaCorrection * level * level + level * 2) * m_levelCorrection);
        }
    }
    void ParameterUp(int level)
    {
        int baseParameter = level % 5 + 1;
        MaxHP += baseParameter;
        NowHP += baseParameter;
        MaxAP += baseParameter;
        NowAP += baseParameter;
        Strength += baseParameter;
        Defense += baseParameter;
        MagicPower += baseParameter;
        MagicResist += baseParameter;
        Speed += baseParameter;
        Luck++;
        SkillPoint += baseParameter;
    }
    //private int BuffChange(int parameter)
    //{
    //    int correction = parameter / 4;
    //    switch (buffState)
    //    {
    //        case BuffState.twoQuarters:
    //            baseParameter = parameter - correction * 2;
    //            break;
    //        case BuffState.threeQuarters:
    //            baseParameter = parameter - correction;
    //            break;
    //        case BuffState.fourQuarters:
    //            baseParameter = parameter;
    //            break;
    //        case BuffState.fiveQuarters:
    //            baseParameter = parameter + correction;
    //            break;
    //        case BuffState.sixQuarters:
    //            baseParameter = parameter + correction * 2;
    //            break;
    //    }
    //    return baseParameter;
    //}

    //public enum BuffState
    //{
    //    twoQuarters,
    //    threeQuarters,
    //    fourQuarters,
    //    fiveQuarters,
    //    sixQuarters
    //}

    //readonly BuffState buffState = BuffState.fourQuarters;

    public void UseItem(ItemData itemData)
    {
        switch (itemData.itemAttributes)
        {
            case ItemData.ItemAttributes.HitPointRecovery:
                NowHP += itemData.ItemEffectValue;
                break;
            case ItemData.ItemAttributes.ActionPointRecovery:
                NowAP += itemData.ItemEffectValue;
                break;
        }
    }
    
    public void EquipEquipment(ItemData equipment)
    {
        switch (equipment.itemAttributes)
        {
            case ItemData.ItemAttributes.weapon:
                Strength += equipment.ItemEffectValue;
                MagicPower += equipment.ItemSubEffectValue;
                break;
            case ItemData.ItemAttributes.armor:
                Defense += equipment.ItemEffectValue;
                MagicResist += equipment.ItemSubEffectValue;
                break;
            case ItemData.ItemAttributes.magicalWeapon:
                MagicPower += equipment.ItemEffectValue;
                Strength += equipment.ItemSubEffectValue;
                break;
            case ItemData.ItemAttributes.magicalArmor:
                MagicResist += equipment.ItemEffectValue;
                Defense += equipment.ItemSubEffectValue;
                break;
        }
    }

    public void RemoveEquipment(ItemData equipment)
    {
        switch (equipment.itemAttributes)
        {
            case ItemData.ItemAttributes.weapon:
                Strength -= equipment.ItemEffectValue;
                MagicPower -= equipment.ItemSubEffectValue;
                break;
            case ItemData.ItemAttributes.armor:
                Defense -= equipment.ItemEffectValue;
                MagicResist -= equipment.ItemSubEffectValue;
                break;
            case ItemData.ItemAttributes.magicalWeapon:
                MagicPower -= equipment.ItemEffectValue;
                Strength -= equipment.ItemSubEffectValue;
                break;
            case ItemData.ItemAttributes.magicalArmor:
                MagicResist -= equipment.ItemEffectValue;
                Defense -= equipment.ItemSubEffectValue;
                break;
        }
    }
}
