using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class BaseWeapon : BaseItem
{
	public BaseWeapon() : base(ItemType.Weapon) { }
}
