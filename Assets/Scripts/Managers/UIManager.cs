using System;
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	/// <summary>
	/// Interpolates current canvas group alpha to target alpha <para />
	/// targetAlpha = 1 -> 0% transparency <para />
	/// targetAlpha = 0 -> 100% transparency
	/// </summary>
	/// <param name="canvasGroup">Canvas group whose alpha is being interpolated</param>
	/// <param name="targetAlpha">Desired alpha value</param>
	/// <param name="speed">speed at which alpha is interpolated</param>
	public IEnumerator InterpolateCanvasGroupAlpha(CanvasGroup canvasGroup, float targetAlpha, float speed)
	{
		try
		{
			if (speed < 0f)
			{
				throw new ArgumentException("speed must be positive");
			}
		}
		catch (ArgumentException ex)
		{
			Debug.LogError(ex.Message);
			Debug.LogError(ex.StackTrace);
			Debug.LogWarning("speed will be set to 0");
			
			speed = 0;
			//throw;
		}

		try
		{
			if (targetAlpha > 1f || targetAlpha < 0f)
			{
				throw new ArgumentException("targetAlpha must be between 0 and 1");
			}
		}
		catch (ArgumentException ex)
		{
			Debug.LogError(ex.Message);
			Debug.LogError(ex.StackTrace);
			Debug.LogWarning("targetAlpha will be clamped");
			
			targetAlpha = Mathf.Clamp(targetAlpha, 0f, 1f);
			
			//throw;
		}
		
		while (true)
		{
			//canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, speed * Time.deltaTime);

			if (canvasGroup.alpha < targetAlpha)
			{
				canvasGroup.alpha += speed * Time.deltaTime;
				canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha, 0, targetAlpha);
			}
			else
			{
				canvasGroup.alpha -= speed * Time.deltaTime;
				canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha, targetAlpha, 1);
			}
			
			yield return new WaitForSeconds(Time.deltaTime * speed);
			if (Extras.NearlyEqual(canvasGroup.alpha, targetAlpha, 0.3f)) break;
		}
		canvasGroup.alpha = targetAlpha;
	}
	
}
