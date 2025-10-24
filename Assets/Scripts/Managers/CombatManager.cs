using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
	[SerializeField] private Canvas[] canvases;
	[SerializeField] private CanvasGroup[] canvasGroups;
	[SerializeField] private GameObject[] enemyEntities;
	[SerializeField] private Entity playerEntity;
	[SerializeField] private TextMeshProUGUI playerStatsTMP;
	[SerializeField] private TextMeshProUGUI enemyStatsTMP;

	public float canvasLerpSpeed;

	public const int INVENTORY = 0;
	public const int TAKE_LEAVE = 1;
	public const int SETTINGS = 2;
	public const int ENGAGE = 3;
	public const int STATS = 4;

	private void Start()
	{
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

	private void ShowCanvas(int index, bool hideOthers = true)
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
			defender.Health -= damage;
			StartCoroutine(ShakeObject(defender.gameObject));
			UpdateStats(player, enemy);

			if (player.Health <= 0)
			{
				Debug.Log("Died");
				yield break;
			}
			else if (enemy.Health <= 0)
			{
				Debug.Log("go next enemy");
				EndCombat(true, enemy);
				yield break;
			}
			
			playersTurn = !playersTurn;
			yield return new WaitForSeconds(2f);
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
		if (enemyEntities[0].TryGetComponent<Entity>(out var enemyEntity))
		{
			ShowCanvas(STATS, false);
			UpdateStats(playerEntity, enemyEntity);
			StartCoroutine(Combat(playerEntity, enemyEntity));
			HideCanvas(ENGAGE);
		}
		else Debug.LogError($"ENEMY OBJECT DOESNT HAVE ENTITY");
	}

	private void EndCombat(bool playerLived, Entity deadEntity)
	{
		if (playerLived)
		{
			ShowCanvas(TAKE_LEAVE);
			StartCoroutine(FadeOutSprite(deadEntity.SpriteRenderer, 2f, true));
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
	
}
