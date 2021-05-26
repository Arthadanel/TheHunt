using UnityEngine;

/// <summary>
/// Отвечает зы выход из игры.
/// </summary>
public class Quit : MonoBehaviour
{
	/// <summary>
	/// Выходит из приложения.
	/// </summary>
	public void QuitGame()
	{
		Application.Quit();
	}
}