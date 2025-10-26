using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
	[SerializeField] private Canvas[] canvases;
	[SerializeField] private CanvasGroup[] canvasGroups;
	[SerializeField] private GameObject[] enemyEntities;
	[SerializeField] private Entity playerEntity;
	[SerializeField] private TextMeshProUGUI playerStatsTMP;
	[SerializeField] private TextMeshProUGUI enemyStatsTMP;
	[SerializeField] private Image itemDropImage;
	[SerializeField] private TextMeshProUGUI coinDropTMP;
	[SerializeField] private InventoryUIManager inventoryUIManager;

	private ItemDrop _itemDrop = null;
	private int _coinsDrop = 0;
	private int _enemyIndex = 0;
	private Entity _enemyEntity;

	public float canvasLerpSpeed;
	public float timeBetweenAttacks;

	public const int INVENTORY = 0;
	public const int TAKE_LEAVE = 1;
	public const int SETTINGS = 2;
	public const int ENGAGE = 3;
	public const int STATS = 4;

	private void Start()
	{
		GameObject go = Instantiate(enemyEntities[_enemyIndex]);
		_enemyEntity = go.GetComponent<Entity>();
		ResetAllCanvases();
		ShowCanvas(ENGAGE, false);
	}

	private void ResetAllCanvases()
	{
		foreach (var canvasGroup in canvasGroups)
		{
			canvasGroup.gameObject.SetActive(false);
			canvasGroup.alpha = 0f;
		}
	}

	private void ShowCanvas(int index, bool hideOthers)
	{
		canvases[index].gameObject.SetActive(true);
		StartCoroutine(UIManager.Instance.InterpolateCanvasGroupAlpha(
			canvasGroups[index], 1f, canvasLerpSpeed));


		if (!hideOthers) return;
		for (int i = 0; i < canvases.Length; i++)
		{
			if (i != index)
			{
				canvases[i].gameObject.SetActive(false);
				StartCoroutine(UIManager.Instance.InterpolateCanvasGroupAlpha(
					canvasGroups[i], 0f, canvasLerpSpeed));
			}
		}
	}

	public void ShowArmory()
	{
		ShowCanvas(INVENTORY, true);
		inventoryUIManager.UpdateInventory();
	}

	public void ShowEngage()
	{
		ShowCanvas(ENGAGE, true);
	}

	public void HideCanvas(int index, bool disableOnEnd = true)
	{
		//canvases[index].gameObject.SetActive(true);
		StartCoroutine(UIManager.Instance.InterpolateCanvasGroupAlpha(
			canvasGroups[index], 0f, canvasLerpSpeed, disableOnEnd));
	}

	private IEnumerator Combat(Entity player, Entity enemy)
	{
		bool playersTurn = true;
		Entity attacker;
		Entity defender;

		while (true)
		{
			if (playersTurn)
			{
				attacker = player;
				defender = enemy;
			}
			else
			{
				attacker = enemy;
				defender = player;
			}
			
			int damage = attacker.Attack - defender.Defense + (attacker.Attack - defender.Defense == 0 ? 1 : 0);
			defender.Health = Mathf.Clamp(defender.Health - damage, 0, defender.Health);
			StartCoroutine(ShakeObject(defender.gameObject));
			UpdateStats(player, enemy);

			if (player.Health <= 0)
			{
				Debug.Log("Died");
				EndCombat(false, player);
				yield break;
			}
			else if (enemy.Health <= 0)
			{
				EndCombat(true, enemy);
				yield break;
			}
			
			playersTurn = !playersTurn;
			yield return new WaitForSeconds(timeBetweenAttacks);
		}

		//yield return null;
	}


	private IEnumerator ShakeObject(GameObject go, float duration = 0.2f, float strength = 30f, float distance = 0.2f)
	{
		float timer = 0f;
		Vector3 pos = go.transform.position;

		while (timer < duration || !Extras.NearlyEqual(go.transform.position.x, pos.x, 0.01f))
		{
			go.transform.position = pos + new Vector3(
				distance * Mathf.Sin(strength * timer), 0f, 0f);
			timer += Time.deltaTime;
			yield return null;
		}
	}

	public void OnEngage()
	{
		ShowCanvas(STATS, false);
		UpdateStats(playerEntity, _enemyEntity);
		StartCoroutine(Combat(playerEntity, _enemyEntity));
		HideCanvas(ENGAGE);
	}

	private void EndCombat(bool playerLived, Entity deadEntity)
	{
		if (playerLived)
		{
			_itemDrop = deadEntity.GetComponent<ItemDrop>();
			_coinsDrop = deadEntity.Coins;
			ShowCanvas(TAKE_LEAVE, true);
			itemDropImage.sprite = _itemDrop.item.itemSprite;
			coinDropTMP.text = $"+{_coinsDrop}";
			//StartCoroutine(FadeOutSprite(deadEntity.SpriteRenderer, 2f));
			StartCoroutine(FadeOutSprite(deadEntity.SpriteRenderer, 2f, true));
		}
		else
		{
			Debug.Log("Player died");
		}
	}

	private void UpdateStats(Entity pe, Entity ee)
	{
		pe.equipment.CalculateTotalBonus();
		ee.equipment.CalculateTotalBonus();
		
		playerStatsTMP.text =
			$"HLT: {pe.Health} / {pe.MaxHealth}\n" +
			$"ATK: {pe.Attack} \n" +
			$"DEF: {pe.Defense} \n" +
			$"MNY: {pe.Coins}";

		enemyStatsTMP.text =
			$"{ee.Health} / {ee.MaxHealth}: HLT\n" +
			$"{ee.Attack} :ATK\n" +
			$"{ee.Defense} :DEF\n";
		//+ $"{ee.coins} :MNY";
	}

	private IEnumerator FadeOutSprite(SpriteRenderer sr, float speed, bool destroyGameObjectOnEnd = false)
	{
		float alpha = sr.color.a;
		while (alpha > 0f)
		{
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
			alpha -= Time.deltaTime * speed;
			yield return null;
		}
		if (destroyGameObjectOnEnd) Destroy(sr.gameObject);
	}

	private void EquipItem<T>(T item) where T : BaseItem
	{
		if (item == null)
		{
			Debug.LogError($"ITEM IS NULL");
			return;
		}
		
		if (!typeof(BaseItem).IsAssignableFrom(typeof(T)))
		{
			Debug.LogError($"WRONG TYPE PASSED AS EQUIPMENT: {typeof(T).Name}");
			return;
		}

		switch (item)
		{
			case BaseHelmeth helmeth:
				playerEntity.equipment.helmeth = helmeth;
				break;
			case BaseArmor armor:
				playerEntity.equipment.armor = armor;
				break;
			case BaseWeapon weapon:
				playerEntity.equipment.weapon = weapon;
				break;
			case BaseCompanion companion:
				playerEntity.equipment.companion = companion;
				break;
			case BaseBind bind:
				playerEntity.equipment.binds.Add(bind);
				break;
			default:
				Debug.LogError("Item is not one of main 5 categories");
				break;
		}
		
		playerEntity.equipment.CalculateTotalBonus();
	}

	public void TakeOrLeave(bool take)
	{
		if (take)
		{
			EquipItem(_itemDrop.item);
		}
		else
		{
			playerEntity.Coins += _coinsDrop;
		}

		_coinsDrop = 0;
		_itemDrop = null;
		
		NextEncounter();
	}

	private void NextEncounter()
	{
		Debug.Log("NEXT ENCOUNTER");
		SpawnNextEnemy();
		ShowCanvas(ENGAGE, true);
	}

	private void SpawnNextEnemy()
	{
		_enemyIndex++;
		GameObject go = Instantiate(enemyEntities[_enemyIndex]);
		_enemyEntity = go.GetComponent<Entity>();
	}
	
}
