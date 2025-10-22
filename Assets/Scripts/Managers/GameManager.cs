using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
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

	public static void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public static void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}
	
	public static void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public static void Quit()
	{
		Debug.Log("Quit");
		Application.Quit();
	}
}
