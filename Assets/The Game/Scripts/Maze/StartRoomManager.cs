using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управление стартовой комнатой.
/// </summary>
public class StartRoomManager : MonoBehaviour
{
	/// <summary>
	/// Спустился ли лифтю
	/// </summary>
	private bool down;
	/// <summary>
	/// Элемент интерфейса для отображения подсказок.
	/// </summary>
	GameObject tip;

	/// <summary>
	/// Инициализация полей.
	/// </summary>
	void Start()
	{
		down = false;
		tip = MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip;
		tip.SetActive(true);
		tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Descend" : MazeLoader.EngToRus["Descend"];
		tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
		tip.transform.GetChild(0).GetComponent<Text>().text = "E";
	}

	/// <summary>
	/// Проверка ввода (нужно ли опустить лифт).
	/// </summary>
	void Update()
	{
		if (!down && !tip.activeSelf)
			tip.SetActive(true);
		if (!down && Input.GetKeyDown(KeyCode.E))
		{
			down = true;
			tip.SetActive(false);
			GetComponent<Animation>().Play();
		}
	}
}
