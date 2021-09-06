using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterParameterManager : CharacterParameters, ITakableDamage
{
    private int nextExp = 0;
    private float levelCorrection = 4.8f;
    //int baseParameter = 0;
    //　装備している武器
    [SerializeField]
    private ItemData equipWeapon = null;
    //　装備している鎧
    [SerializeField]
    private ItemData equipArmor = null;
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
    }
    private void LevelUp()
    {
        Level++;
        CalculateExp(Level);
    }
    public void GetExp(EnemyParameters enemyParameters)
    {
        CharacterExp += enemyParameters.ExperiencePoint;
        CharacterTotalExp += enemyParameters.ExperiencePoint;
        if (CharacterNextExp - CharacterExp <= 0)
        {
            nextExp = CharacterNextExp - CharacterExp;
            CharacterExp = 0;
            if (Level != 100)
            {
                LevelUp();
                CharacterExp -= nextExp;
            }
        }
    }
    private void CalculateExp(int level)
    {
        if (level % 10 == 0)
        {
            levelCorrection *= 1.002f;
        }
        if (level - 10 <= 1)
        {
            CharacterNextExp = (int)((level * level * 0.07 * 1 + level * 2) * levelCorrection);
        }
        else
        {
            CharacterNextExp = (int)((level * level * 0.07 * (level - 10) + level * 2) * levelCorrection);
        }
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
