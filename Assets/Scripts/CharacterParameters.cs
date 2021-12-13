using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameters : MonoBehaviour
{
    [SerializeField, Tooltip("キャラクターの名前")]
    string characterName = "";
    /// <summary>キャラクターの名前</summary>
    public string CharacterName => characterName;

    [SerializeField]
    [Tooltip("キャラクターの最大HP")]
    [Range(1, 10000)]
    int maxHP = 1;
    /// <summary>キャラクターの最大HP</summary>
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの現在HP")]
    [Range(0, 10000)]
    int nowHP = 0;
    /// <summary>キャラクターの現在HP</summary>
    public int NowHP
    {
        get { return nowHP; }
        set { nowHP = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの最大AP")]
    [Range(1, 10000)]
    int maxAP = 1;
    /// <summary>キャラクターの最大AP</summary>
    public int MaxAP
    {
        get { return maxAP; }
        set { maxAP = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの現在AP")]
    [Range(0, 10000)]
    int nowAP = 0;
    /// <summary>キャラクターの現在AP</summary>
    public int NowAP
    {
        get { return nowAP; }
        set { nowAP = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの物理攻撃力")]
    [Range(1, 10000)]
    int strength = 1;
    /// <summary>キャラクターの物理攻撃力</summary>
    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの物理防御力")]
    [Range(1, 10000)]
    int defense = 1;
    /// <summary>キャラクターの物理防御力</summary>
    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの魔法攻撃力")]
    [Range(1, 10000)]
    int magicPower = 1;
    /// <summary>キャラクターの魔法攻撃力</summary>
    public int MagicPower
    {
        get { return magicPower; }
        set { magicPower = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの魔法防御力")]
    [Range(1, 10000)]
    int magicResist = 1;
    /// <summary>キャラクターの魔法防御力</summary>
    public int MagicResist
    {
        get { return magicResist; }
        set { magicResist = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの運")]
    [Range(1, 10000)]
    int luck = 1;
    /// <summary>キャラクターの運</summary>
    public int Luck
    {
        get { return luck; }
        set { luck = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの速さ")]
    [Range(1, 10000)]
    int speed = 1;
    /// <summary>キャラクターの速さ</summary>
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターのレベル")]
    [Range(1, 100)]
    int level = 1;
    /// <summary>キャラクターのレベル</summary>
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの現在の経験値")]
    [Range(0, 10000000)]
    int exp = 0;
    /// <summary>キャラクターの現在の経験値</summary>
    protected int NowExp
    {
        get { return exp; }
        set { exp = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの総経験値")]
    [Range(0, 10000000)]
    int characterTotalExp = 0;
    /// <summary>キャラクターの総経験値</summary>
    protected int TotalExp
    {
        get { return characterTotalExp; }
        set { characterTotalExp = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターの次のレベルまでの経験値")]
    [Range(1, 10000000)]
    int characterNextExp = 1;
    /// <summary>キャラクターの次のレベルまでの経験値</summary>
    protected int NextExp
    {
        get { return characterNextExp; }
        set { characterNextExp = value; }
    }

    [SerializeField]
    [Tooltip("キャラクターのスキルポイント")]
    [Range(0, 1000)]
    int characterSP = 0;
    /// <summary>キャラクターのスキルポイント</summary>
    public int SkillPoint
    {
        get { return characterSP; }
        set { characterSP = value; }
    }

    [Tooltip("キャラクターの特別な技が使用可能かどうか")]
    bool availableSpecialSkill = false;
    /// <summary>キャラクターの特別な技が使用可能かどうか</summary>
    public bool SS
    {
        get { return availableSpecialSkill; }
        set { availableSpecialSkill = value; }
    }
}
