using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyParameter : MonoBehaviour
{
    [SerializeField]
    StringReactiveProperty m_name = new StringReactiveProperty();
    /// <summary>キャラクターの名前</summary>
    public IReadOnlyReactiveProperty<string> Name => m_name;

    [SerializeField]
    IntReactiveProperty m_hp = new IntReactiveProperty();
    /// <summary>キャラクターの現在HP</summary>
    public IReactiveProperty<int> HP => m_hp;

    [SerializeField]
    IntReactiveProperty m_maxHp = new IntReactiveProperty();
    /// <summary>キャラクターの最大HP</summary>
    public IReadOnlyReactiveProperty<int> MaxHP => m_maxHp;

    [SerializeField]
    IntReactiveProperty m_ap = new IntReactiveProperty();
    /// <summary>キャラクターの現在AP</summary>
    public IReactiveProperty<int> AP => m_ap;

    [SerializeField]
    IntReactiveProperty m_maxAp = new IntReactiveProperty();
    /// <summary>キャラクターの最大AP</summary>
    public IReadOnlyReactiveProperty<int> MaxAP => m_maxAp;

    [SerializeField]
    IntReactiveProperty m_strength = new IntReactiveProperty();
    /// <summary>キャラクターの攻撃力</summary>
    public IReadOnlyReactiveProperty<int> Strength => m_strength;

    [SerializeField]
    IntReactiveProperty m_defense = new IntReactiveProperty();
    /// <summary>キャラクターの防御力</summary>
    public IReadOnlyReactiveProperty<int> Defense => m_defense;

    [SerializeField]
    IntReactiveProperty m_magicPower = new IntReactiveProperty();
    /// <summary>キャラクターの魔法攻撃力</summary>
    public IReadOnlyReactiveProperty<int> MagicPower => m_magicPower;

    [SerializeField]
    IntReactiveProperty m_magicResist = new IntReactiveProperty();
    /// <summary>キャラクターの魔法抵抗力</summary>
    public IReadOnlyReactiveProperty<int> MagicResist => m_magicResist;

    [SerializeField]
    IntReactiveProperty m_luck = new IntReactiveProperty();
    /// <summary>キャラクターの運</summary>
    public IReadOnlyReactiveProperty<int> Luck => m_luck;

    [SerializeField]
    IntReactiveProperty m_speed = new IntReactiveProperty();
    /// <summary>キャラクターの速さ</summary>
    public IReadOnlyReactiveProperty<int> Speed => m_speed;

    [SerializeField]
    [Range(1, 5)]
    int m_maxActionCount = 3;
    /// <summary>蓄積可能な行動回数</summary>
    public int MaxActionCount { get => m_maxActionCount; }
}
