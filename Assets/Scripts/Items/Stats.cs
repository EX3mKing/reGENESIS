using UnityEngine;

[CreateAssetMenu(fileName = "Stats",  menuName = "Stats")]
public class Stats : ScriptableObject
{
    public int Health;
    public int MaxHealth;
    public int Defense;
    public int Attack;
}
