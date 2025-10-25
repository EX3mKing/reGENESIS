using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Equipment : MonoBehaviour
{
    public BaseHelmeth helmeth;
    public BaseArmor armor;
    public BaseWeapon weapon;
    public BaseCompanion companion;
    public List<BaseBind> binds;
    private List<BaseItem> _items = new List<BaseItem>();

    public int healing;
    public int maxHealth;
    public int attack;
    public int defense;

    public void CalculateTotalBonus()
    {
        healing = 0;
        maxHealth = 0;
        attack = 0;
        defense = 0;
        
        _items.Clear();
        foreach (BaseBind bind in binds)
        {
            if(bind != null) _items.Add(bind);
        }
        if (helmeth != null) _items.Add(helmeth);
        if (armor != null) _items.Add(armor);
        if (weapon != null) _items.Add(weapon);
        if (companion != null) _items.Add(companion);
        
        foreach (BaseItem item in _items)
        {
            attack += item.attack;
            defense += item.defense;
            maxHealth += item.maxHealth;
        }
        if (companion != null) healing += companion.healing;
    }
}
