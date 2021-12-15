using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterParameter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("名前")]
    StringReactiveProperty m_name = new StringReactiveProperty();
    /// <summary>キャラクターの名前</summary>
    public IReadOnlyReactiveProperty<string> Name => m_name;

    [SerializeField]
    [Tooltip("HP")]
    IntReactiveProperty m_hp = new IntReactiveProperty();
    /// <summary>キャラクターの現在HP</summary>
    public IReactiveProperty<int> HP => m_hp;

    [SerializeField]
    [Tooltip("最大HP")]
    IntReactiveProperty m_maxHp = new IntReactiveProperty();
    /// <summary>キャラクターの最大HP</summary>
    public IReactiveProperty<int> MaxHP => m_maxHp;
    
    [SerializeField]
    [Tooltip("AP")]
    IntReactiveProperty m_ap = new IntReactiveProperty();
    /// <summary>キャラクターの現在AP</summary>
    public IReactiveProperty<int> AP => m_ap;

    [SerializeField]
    [Tooltip("最大AP")]
    IntReactiveProperty m_maxAp = new IntReactiveProperty();
    /// <summary>キャラクターの最大AP</summary>
    public IReactiveProperty<int> MaxAP => m_maxAp;

    [SerializeField]
    [Tooltip("攻撃力")]
    IntReactiveProperty m_strength = new IntReactiveProperty();
    /// <summary>キャラクターの攻撃力</summary>
    public IReactiveProperty<int> Strength => m_strength;

    [SerializeField]
    [Tooltip("防御力")]
    IntReactiveProperty m_defense = new IntReactiveProperty();
    /// <summary>キャラクターの防御力</summary>
    public IReactiveProperty<int> Defense => m_defense;

    [SerializeField]
    [Tooltip("魔法攻撃力")]
    IntReactiveProperty m_magicPower = new IntReactiveProperty();
    /// <summary>キャラクターの魔法攻撃力</summary>
    public IReactiveProperty<int> MagicPower => m_magicPower;

    [SerializeField]
    [Tooltip("魔法抵抗力")]
    IntReactiveProperty m_magicResist = new IntReactiveProperty();
    /// <summary>キャラクターの魔法抵抗力</summary>
    public IReactiveProperty<int> MagicResist => m_magicResist;

    [SerializeField]
    [Tooltip("運")]
    IntReactiveProperty m_luck = new IntReactiveProperty();
    /// <summary>キャラクターの運</summary>
    public IReactiveProperty<int> Luck => m_luck;

    [SerializeField]
    [Tooltip("レベル補正")]
    IntReactiveProperty m_speed = new IntReactiveProperty();
    /// <summary>キャラクターの速さ</summary>
    public IReactiveProperty<int> Speed => m_speed;

    [SerializeField]
    [Tooltip("レベル")]
    IntReactiveProperty m_level = new IntReactiveProperty();
    /// <summary>キャラクターのレベル</summary>
    public IReactiveProperty<int> Level => m_level;

    [SerializeField]
    [Tooltip("スキルポイント")]
    IntReactiveProperty m_skillPoint = new IntReactiveProperty();
    /// <summary>キャラクターのスキルポイント</summary>
    public IReactiveProperty<int> SkillPoint => m_skillPoint;

    IntReactiveProperty m_nowExp = new IntReactiveProperty();
    /// <summary>キャラクターの現在の経験値</summary>
    public IReactiveProperty<int> NowExp => m_nowExp;

    IntReactiveProperty m_totalExp = new IntReactiveProperty();
    /// <summary>キャラクターの総経験値</summary>
    public IReactiveProperty<int> TotalExp => m_totalExp;

    IntReactiveProperty m_nextExp = new IntReactiveProperty(10);
    /// <summary>次のレベルまでの経験値</summary>
    public IReactiveProperty<int> NextExp => m_nextExp;

    BoolReactiveProperty m_availableSpecialSkill = new BoolReactiveProperty(false);
    /// <summary>特別な技が使用可能かどうか</summary>
    public IReactiveProperty<bool> SpecialSkill => m_availableSpecialSkill;

    /// <summary>レベル補正</summary>
    [SerializeField]
    [Tooltip("レベル補正")]
    protected float m_levelCorrection = 4.8f;

    /// <summary>キャラクター補正</summary>
    [SerializeField]
    [Tooltip("キャラクター補正")]
    protected float m_charaCorrection = 0.07f;
}
