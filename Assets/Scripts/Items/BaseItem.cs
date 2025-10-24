using System;
using UnityEngine;

public enum ItemType
{
    Helmeth,
    Armor,
    Weapon,
    Companion,
    Bind
}

public abstract class BaseItem : ScriptableObject
{
    public string itemName;
    [TextArea] 
    public string itemDescription;
    
    [Space(15f)]
    public Sprite itemSprite;
    public ItemType itemType;
    
    [Space(15f)]
    public int defense;
    public int attack;
    public int maxHealth;

    public BaseItem(ItemType itemType)
    {
        this.itemType = itemType;
    }
}
