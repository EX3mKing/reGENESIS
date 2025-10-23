using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Bonuses
{
    public int Health;
    public int Defense;
    public int Attack;
}

public class Equipment : MonoBehaviour
{
    [SerializeField] private BaseHelmeth helmeth;
    [SerializeField] private BaseArmor armor;
    [SerializeField] private BaseWeapon weapon;
    [SerializeField] private BaseCompanion companion;
    [SerializeField] private BaseBind[] binds;
    private List<BaseItem> _items = new List<BaseItem>();

    private int _health = 0;
    private int _defense = 0;
    private int _attack = 0;
    
    private void ResetItemsList()
    {
        _items.Clear();
        foreach (BaseBind bind in binds)
        {
            if(bind != null) _items.Add(bind);
        }
        if (helmeth != null) _items.Add(helmeth);
        if (armor != null) _items.Add(armor);
        if (weapon != null) _items.Add(weapon);
        if (companion != null) _items.Add(companion);
        
    }

    public Bonuses CalculateTotalBonus()
    {
        ResetItemsList();
        Bonuses bonuses = new Bonuses();
        foreach (BaseItem item in _items)
        {
            bonuses.Attack += item.attack;
            bonuses.Defense += item.defense;
        }
        if (companion != null) bonuses.Health += companion.health;
        return bonuses;
    }
}
