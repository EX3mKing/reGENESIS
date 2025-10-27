using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public BaseHelmeth[] helmets;
    public BaseArmor[] armors;
    public BaseWeapon[] weapons;
    public BaseCompanion[] companions;
    public BaseBind[] binds;
    public TextMeshProUGUI[] texts;
    public Image[] itemIcons;
    public Entity player;
    
    private List<BaseItem> _items = new List<BaseItem>();
    private BaseItem[] shopItems =  new BaseItem[4];


    private void Awake()
    {
        foreach (var item in helmets)
        {
            _items.Add(item);
        }

        foreach (var item in armors)
        {
            _items.Add(item);
        }

        foreach (var item in weapons)
        {
            _items.Add(item);
        }

        foreach (var item in companions)
        {
            _items.Add(item);
        }

        foreach (var item in binds)
        {
            _items.Add(item);
        }
    }

    public void PickShopItems()
    {
        for (int i = 0; i < 4; i++)
        {
            int randomItem = Random.Range(0, _items.Count);
            Debug.Log(_items[randomItem].itemName);
            shopItems[i] = _items[randomItem];
            itemIcons[i].sprite = shopItems[i].itemSprite;
            texts[i].text = $"{Random.Range(1, 6)}";
        }
    }

    public void BuyItem(int index)
    {
        int cost = Int32.Parse(texts[index].text);
        
        if (cost > player.Coins) return;
        
        texts[index].gameObject.transform.parent.gameObject.SetActive(false);
        itemIcons[index].gameObject.transform.parent.gameObject.SetActive(false);
        
        player.Coins -= cost;
        switch (shopItems[index].itemType)
        {
            case ItemType.Helmeth:
                foreach (var helmeth in helmets)
                {
                    if (helmeth.itemName == shopItems[index].itemName)
                    {
                        player.equipment.helmeth =  helmeth;
                    }
                }
                break;
            case ItemType.Armor:
                foreach (var armor in armors)
                {
                    if (armor.itemName == shopItems[index].itemName)
                    {
                        player.equipment.armor = armor;
                    }
                }
                break;
            case ItemType.Weapon:
                foreach (var weapon in weapons)
                {
                    if (weapon.itemName == shopItems[index].itemName)
                    {
                        player.equipment.weapon = weapon;
                    }
                }
                break;
            case ItemType.Companion:
                foreach (var companion in companions)
                {
                    if (companion.itemName == shopItems[index].itemName)
                    {
                        player.equipment.companion = companion;
                    }
                }
                break;
            case ItemType.Bind:
                foreach (var bind in binds)
                {
                    if (bind.itemName == shopItems[index].itemName)
                    {
                        player.equipment.binds.Add(bind);
                    }
                }
                break;
        }
    }
}
