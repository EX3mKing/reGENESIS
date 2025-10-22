using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private void Start()
    {
        CalculateTotalBonus();
    }

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

    private void CalculateTotalBonus()
    {
        ResetItemsList();
        foreach (BaseItem item in _items)
        {
            _attack += item.attack;
            _defense += item.defense;
        }
        if (companion != null) _health += companion.health;
    }

    public BaseItem GetRandomItem()
    {
        ResetItemsList();
        int i = Random.Range(0, _items.Count);
        return _items[i];
    }
}
