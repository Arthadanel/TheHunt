using UnityEngine;

/// <summary>
/// Отвечает за паузу в игре.
/// </summary>
public class Pause : MonoBehaviour
{
	/// <summary>
	/// Объект интерфейса с параметрами персонажа.
	/// </summary>
	internal static GameObject HUD;
	/// <summary>
	/// Объект интерфейса с различной информацией и сообщениями.
	/// </summary>
	internal static GameObject Notices;
	/// <summary>
	/// Объект меню паузы.
	/// </summary>
	[SerializeField] private GameObject pauseMenu;
	/// <summary>
	/// На паузе ли игра.
	/// </summary>
	private bool paused = false;

	/// <summary>
	/// Проверка ввода (нужно ли активировать/дизактивировать меню паузы.
	/// </summary>
	private void Update()
	{
		paused = pauseMenu.activeSelf;
		if (!MazeLoader.Dying && MazeLoader.mainPlayer != null && !MazeLoader.mainPlayer.GetComponent<PlayerController>().Engaged && Input.GetKeyDown(KeyCode.Escape))
		{
			if (paused)
			{
				DisactivatePauseMenu();
			}
			else
			{
				ActivatePauseMenu();
			}
		}
	}

	/// <summary>
	/// Активация меню паузы.
	/// </summary>
	public void ActivatePauseMenu()
	{
		HUD.SetActive(false);
		Notices.SetActive(false);
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		CameraMove.CameraMoveEnabled = false;
	}

	/// <summary>
	/// Дизактивация меню паузы.
	/// </summary>
	public void DisactivatePauseMenu()
	{
		pauseMenu.SetActive(false);
		HUD.SetActive(true);
		Notices.SetActive(true);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1f;
		CameraMove.CameraMoveEnabled = true;
	}
}
