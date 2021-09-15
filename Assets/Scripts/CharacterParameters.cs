using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameters : MonoBehaviour
{
    /// <summary>キャラクターの名前</summary>
    [SerializeField]
    private string characterName;
    public string CharacterName 
    {
        get { return characterName; }
        set { characterName = value;} 
    }
    /// <summary>キャラクターの最大HP</summary>
    [SerializeField]
    [Range(1, 10000)]
    protected int maxHP;
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    /// <summary>キャラクターの現在HP</summary>
    [SerializeField]
    [Range(0, 10000)]
    protected int nowHP;
    public int NowHP
    {
        get { return nowHP; }
        set { nowHP = value; }
    }
    /// <summary>キャラクターの最大AP</summary>
    [SerializeField]
    [Range(0, 10000)]
    protected int maxAP;
    public int MaxAP
    {
        get { return maxAP; }
        set { maxAP = value; }
    }
    /// <summary>キャラクターの現在AP</summary>
    [SerializeField]
    [Range(0, 10000)]
    protected int nowAP;
    public int NowAP
    {
        get { return nowAP; }
        set { nowAP = value; }
    }
    /// <summary>キャラクターの物理攻撃力</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int strength;
    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }
    /// <summary>キャラクターの物理防御力</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int defense;
    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }
    /// <summary>キャラクターの魔法攻撃力</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int magicPower;
    public int MagicPower
    {
        get { return magicPower; }
        set { magicPower = value; }
    }
    /// <summary>キャラクターの魔法防御力</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int magicResist;
    public int MagicResist
    {
        get { return magicResist; }
        set { magicResist = value; }
    }
    /// <summary>キャラクターの運</summary>
    [SerializeField]
    [Range(0, 10000)]
    private int luck;
    public int Luck
    {
        get { return luck; }
        set { luck = value; }
    }
    /// <summary>キャラクターの速さ</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int speed;
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    /// <summary>キャラクターのレベル</summary>
    [SerializeField]
    [Range(1, 100)]
    private int level;
    protected int Level
    {
        get { return level; }
        set { level = value; }
    }
    /// <summary>キャラクターの経験値</summary>
    [SerializeField]
    [Range(0, 10000000)]
    private int exp;
    protected int CharacterExp
    {
        get { return exp; }
        set { exp = value; }
    }
    /// <summary>キャラクターの総経験値</summary>
    [SerializeField]
    [Range(0, 10000000)]
    private int characterTotalExp;
    protected int CharacterTotalExp
    {
        get { return characterTotalExp; }
        set { characterTotalExp = value; }
    }
    /// <summary>キャラクターの次のレベルまでの経験値</summary>
    [SerializeField]
    [Range(0, 10000000)]
    private int characterNextExp;
    protected int CharacterNextExp
    {
        get { return characterNextExp; }
        set { characterNextExp = value; }
    }
    /// <summary>キャラクターのスキルポイント</summary>
    [SerializeField]
    [Range(0, 1000)]
    private int characterSP;
    public int SP
    {
        get { return characterSP; }
        set { characterSP = value; }
    }
    /// <summary>キャラクターのスペシャルゲージ</summary>
    [SerializeField]
    [Range(0, 100)]
    private int characterSG;
    public int SG
    {
        get { return characterSG; }
        set { characterSG = value; }
    }
    
    public enum NomalAttackType
    {
        physicalAttack,
        magicAttack
    }
    [EnumIndex(typeof(NomalAttackType))]
    /// <summary>通常攻撃のSkillData</summary>
    public SkillData[] normalskill = new SkillData[2];
    /// <summary>所持スキル</summary>
    public SkillData[] hasSkills = new SkillData[] { };
}
