using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int coins;

    public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();

    public int Health
    {
        get => health;
        set => health = value;
    }

    public int MaxHealth
    {
        get => maxHealth + equipment.maxHealth;
        set => maxHealth = value;
    }

    public int Defense
    {
        get => defense +  equipment.defense;
        set => defense = value;
    }
    public int Attack 
    { 
        get => attack + equipment.attack; 
        set => this.attack = value;
    }

    public int Coins
    {
        get => coins; set => this.coins = value;
    }
    
    public Equipment equipment;
}
