using System;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
	[SerializeField] private Canvas[] canvases;
	[SerializeField] private CanvasGroup[] canvasGroups;
	[SerializeField] private GameObject[] Enemies;

	public float lerpSpeed;

	public const int INVENTORY = 0;
	public const int TAKE_LEAVE = 1;
	public const int SETTINGS = 2;
	public const int ENGAGE = 3;
	public const int STATS = 4;
	
	private void Start()
	{
		ResetAllCanvases();
		canvases[ENGAGE].gameObject.SetActive(true);
		StartCoroutine(UIManager.Instance.InterpolateCanvasGroupAlpha(canvasGroups[ENGAGE], 1f, lerpSpeed));
	}

	private void ResetAllCanvases()
	{
		foreach (var canvasGroup in canvasGroups)
		{
			canvasGroup.gameObject.SetActive(false);
			canvasGroup.alpha = 0f;
		}
	}
}
