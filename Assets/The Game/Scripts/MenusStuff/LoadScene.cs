using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Зпгузка сцены.
/// </summary>
public class LoadScene : MonoBehaviour
{
	/// <summary>
	/// Загрузка сцены по заданному индексу.
	/// </summary>
	/// <param name="sceneIndex"></param>
	public void LoadByIndex(int sceneIndex)
	{
		if (sceneIndex == 0)
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
			//MazeLoader.mainPlayer.GetComponent<PlayerInfo>().enabled = false;
			MazeLoader.Dying = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			MazeLoader.Dying = false;
		}
		if (sceneIndex == 11)
			SceneManager.LoadScene(PlayerPrefs.GetInt("max_chapter_num"));
		else if (sceneIndex == 10)
			SceneManager.LoadScene(PlayerPrefs.GetInt("chapter_num"));
		else
			SceneManager.LoadScene(sceneIndex);
	}
}
