using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    /// <summary>アイテムの名前</summary>
    [SerializeField]
    private string itemName;
    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }
    /// <summary>アイテムID</summary>
    [SerializeField]
    [Range(0, 10000)] private int itemID;
    public int ItemID
    {
        get { return itemID; }
        private set { itemID = value; }
    }
    /// <summary>アイテムレア度</summary>
    [SerializeField]
    [Range(0, 10)] private int itemRarity;
    public int ItemRarity
    {
        get { return itemRarity; }
        private set { itemRarity = value; }
    }
    /// <summary>アイテムの効果値</summary>
    [SerializeField] 
    [Range(0, 1000)] private int itemEffectValue;
    public int ItemEffectValue
    {
        get { return itemEffectValue; }
        private set { itemEffectValue = value; }
    }
    /// <summary>アイテムのサブ効果値</summary>
    [SerializeField]
    [Range(0, 1000)] private int itemSubEffectValue;
    public int ItemSubEffectValue
    {
        get { return itemSubEffectValue; }
        private set { itemSubEffectValue = value; }
    }
    /// <summary>アイテムの売値</summary>
    [SerializeField] 
    [Range(0, 10000)] private int sellingPrice;
    public int SellingPrice
    {
        get { return sellingPrice; }
        private set { sellingPrice = value; }
    }
    /// <summary>アイテムの買値</summary>
    [SerializeField] 
    [Range(0, 10000)] private int bidPrice;
    public int BidPrice
    {
        get { return bidPrice; }
        private set { bidPrice = value; }
    }
    public enum ItemAttributes
    {
        HitPointRecovery,
        ActionPointRecovery,
        weapon,
        armor,
        masicalWeapon,
        masicalArmor
    }
    /// <summary>アイテムの属性</summary>
    public ItemAttributes itemAttributes;
    /// <summary>アイテムの情報</summary>
    [SerializeField]
    private string itemInformation;
    public string ItemInformation
    {
        get { return itemInformation; }
        set { itemInformation = value; }
    }
}
