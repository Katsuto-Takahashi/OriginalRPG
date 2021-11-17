using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField, Tooltip("アイテムの名前")]
    string itemName = "";
    public string ItemName => itemName;
    [SerializeField, Tooltip("アイテムID"), Range(0, 10000)]
    int itemID = 0;
    public int ItemID => itemID;
    [SerializeField, Tooltip("アイテムレア度"), Range(0, 10)]
    int itemRarity = 0;
    public int ItemRarity => itemRarity;
    [SerializeField, Tooltip("アイテムの効果値"), Range(0, 1000)]
    int itemEffectValue = 0;
    public int ItemEffectValue => itemEffectValue;
    [SerializeField, Tooltip("アイテムのサブ効果値"), Range(0, 1000)]
    int itemSubEffectValue = 0;
    public int ItemSubEffectValue => itemSubEffectValue;
    [SerializeField, Tooltip("アイテムの売値"), Range(0, 10000)]
    int sellingPrice = 0;
    public int SellingPrice => sellingPrice;
    [SerializeField, Tooltip("アイテムの買値"), Range(0, 10000)]
    int bidPrice = 0;
    public int BidPrice  => bidPrice;
    public enum ItemAttributes
    {
        HitPointRecovery,
        ActionPointRecovery,
        weapon,
        armor,
        magicalWeapon,
        magicalArmor
    }
    [Tooltip("アイテムの属性")]
    public ItemAttributes itemAttributes;
    [SerializeField, Tooltip("アイテムの情報")]
    string itemInformation = "";
    public string ItemInformation  => itemInformation;
}
