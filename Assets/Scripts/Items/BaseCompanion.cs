using UnityEngine;
[CreateAssetMenu(fileName = "Companion", menuName = "Item/Companion")]
public class BaseCompanion : BaseItem
{
	public BaseCompanion() : base(ItemType.Companion) { }
	public int health;
}
