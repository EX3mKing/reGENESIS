using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    
    
    [SerializeField] private Image helmethIcon;
    [SerializeField] private Image armorIcon;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image companionIcon;
    [SerializeField] private TextMeshProUGUI mnyAmountTMP;
    [SerializeField] private GameObject bindsLayoutGroup;
    [SerializeField] private GameObject bindHolderPrefab;
    
    [SerializeField] private Entity player;

    public void UpdateInventory()
    {
        if (player.equipment.helmeth != null) helmethIcon.sprite = player.equipment.helmeth.itemSprite;
        if (player.equipment.armor != null) armorIcon.sprite = player.equipment.armor.itemSprite;
        if (player.equipment.weapon != null) weaponIcon.sprite = player.equipment.weapon.itemSprite;
        if (player.equipment.companion != null) companionIcon.sprite = player.equipment.companion.itemSprite;

        mnyAmountTMP.text = player.Coins.ToString();

        foreach (Transform child in bindsLayoutGroup.gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (BaseBind bind in player.equipment.binds)
        {
            GameObject bindHolder = Instantiate(bindHolderPrefab, bindsLayoutGroup.transform);
            bindHolder.transform.GetChild(0).GetComponent<Image>().sprite = bind.itemSprite;
        }
    }
}
