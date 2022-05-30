using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterParameter : MonoBehaviour
{
    [SerializeField]
    StringReactiveProperty m_name = new StringReactiveProperty();
    /// <summary>キャラクターの名前</summary>
    public StringReactiveProperty Name => m_name;

    [SerializeField]
    IntReactiveProperty m_hp = new IntReactiveProperty(1);
    /// <summary>キャラクターの現在HP</summary>
    public IntReactiveProperty HP => m_hp;

    [SerializeField]
    IntReactiveProperty m_maxHp = new IntReactiveProperty(1);
    /// <summary>キャラクターの最大HP</summary>
    public IntReactiveProperty MaxHP => m_maxHp;

    [SerializeField]
    IntReactiveProperty m_ap = new IntReactiveProperty(1);
    /// <summary>キャラクターの現在AP</summary>
    public IntReactiveProperty AP => m_ap;

    [SerializeField]
    IntReactiveProperty m_maxAp = new IntReactiveProperty(1);
    /// <summary>キャラクターの最大AP</summary>
    public IntReactiveProperty MaxAP => m_maxAp;

    [SerializeField]
    IntReactiveProperty m_strength = new IntReactiveProperty(1);
    /// <summary>キャラクターの攻撃力</summary>
    public IntReactiveProperty Strength => m_strength;

    [SerializeField]
    IntReactiveProperty m_defense = new IntReactiveProperty(1);
    /// <summary>キャラクターの防御力</summary>
    public IntReactiveProperty Defense => m_defense;

    [SerializeField]
    IntReactiveProperty m_magicPower = new IntReactiveProperty(0);
    /// <summary>キャラクターの魔法攻撃力</summary>
    public IntReactiveProperty MagicPower => m_magicPower;

    [SerializeField]
    IntReactiveProperty m_magicResist = new IntReactiveProperty(1);
    /// <summary>キャラクターの魔法抵抗力</summary>
    public IntReactiveProperty MagicResist => m_magicResist;

    [SerializeField]
    IntReactiveProperty m_luck = new IntReactiveProperty(1);
    /// <summary>キャラクターの運</summary>
    public IntReactiveProperty Luck => m_luck;

    [SerializeField]
    IntReactiveProperty m_speed = new IntReactiveProperty(1);
    /// <summary>キャラクターの速さ</summary>
    public IntReactiveProperty Speed => m_speed;

    [SerializeField]
    IntReactiveProperty m_level = new IntReactiveProperty(1);
    /// <summary>キャラクターのレベル</summary>
    public IntReactiveProperty Level => m_level;

    [SerializeField]
    IntReactiveProperty m_skillPoint = new IntReactiveProperty(0);
    /// <summary>キャラクターのスキルポイント</summary>
    public IntReactiveProperty SkillPoint => m_skillPoint;

    IntReactiveProperty m_nowExp = new IntReactiveProperty(0);
    /// <summary>キャラクターの現在の経験値</summary>
    public IntReactiveProperty NowExp => m_nowExp;

    IntReactiveProperty m_totalExp = new IntReactiveProperty(0);
    /// <summary>キャラクターの総経験値</summary>
    public IntReactiveProperty TotalExp => m_totalExp;

    IntReactiveProperty m_nextExp = new IntReactiveProperty(10);
    /// <summary>次のレベルまでの経験値</summary>
    public IntReactiveProperty NextExp => m_nextExp;

    BoolReactiveProperty m_availableSpecialSkill = new BoolReactiveProperty(false);
    /// <summary>特別な技が使用可能かどうか</summary>
    public BoolReactiveProperty SpecialSkill => m_availableSpecialSkill;

    /// <summary>レベル補正</summary>
    [SerializeField]
    protected float m_levelCorrection = 4.8f;

    /// <summary>キャラクター補正</summary>
    [SerializeField]
    protected float m_charaCorrection = 0.07f;

    [SerializeField]
    [Range(1, 5)]
    int m_maxActionCount = 3;
    /// <summary>蓄積可能な行動回数</summary>
    public int MaxActionCount => m_maxActionCount;

    [SerializeField]
    List<int> m_hasSkillIndex = new List<int>();
    /// <summary>持っているスキルのインデックス</summary>
    public List<int> HasSkillIndex => m_hasSkillIndex;
}
