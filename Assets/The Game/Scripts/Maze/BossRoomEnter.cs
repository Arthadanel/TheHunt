using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управление объектами финальной комнаты.
/// </summary>
public class BossRoomEnter : MonoBehaviour
{
	/// <summary>
	/// Тип объекта.
	/// </summary>
	[SerializeField] string type;

	/// <summary>
	/// Босс.
	/// </summary>
	[SerializeField] GameObject boss;

	/// <summary>
	/// Опущен ли лифт.
	/// </summary>
	bool down = false;
	/// <summary>
	/// Анимация спуска лифта.
	/// </summary>
	Animation anim;

	/// <summary>
	/// Вектор, на который сдвигаются некоторые объекты.
	/// </summary>
	Vector3 move;
	/// <summary>
	/// Игрок.
	/// </summary>
	PlayerInfo player;

	/// <summary>
	/// Экран завершения игры.
	/// </summary>
	internal static GameObject EndScreen;
	/// <summary>
	/// Звук, проигрываемый при завершении игры.
	/// </summary>
	internal static AudioSource WinSound;
	/// <summary>
	/// Музыка финальной битвы.
	/// </summary>
	internal static AudioSource FinalBattleMusic;
	/// <summary>
	/// Текущая фоновая музыка.
	/// </summary>
	internal static AudioSource CurrentBGMusic;

	/// <summary>
	/// Звук телепорта.
	/// </summary>
	internal static AudioSource Teleport;

	/// <summary>
	/// Инициализация начальных параметров, конфигурация комнаты.
	/// </summary>
	private void Start()
	{
		player = MazeLoader.mainPlayer.GetComponent<PlayerInfo>();
		move = new Vector3(0, (transform.parent.localScale.y + 1f) * 3f, 0);
		if (player.Lvl == 6 && type == "Portal")
		{
			transform.position -= move;
		}
		else
			boss.transform.position -= move;
	}

	/// <summary>
	/// Побежден ли босс.
	/// </summary>
	internal static bool BossDefeated;

	/// <summary>
	/// Проверка состояния комнаты и выполнение действий в соответсвие с результатом проверки.
	/// </summary>
	private void Update()
	{
		if (player.Lvl == 6 && type == "Portal" && BossDefeated)
		{
			transform.position += move;
			BossDefeated = false;
		}
	}

	/// <summary>
	/// Действия, выполняемые при входе игрока в зону влияния объекта.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if (type == "Elevator" && !down && other.tag == "Player")
		{
			if (player.Lvl == 6)
			{
				CurrentBGMusic.Stop();
				FinalBattleMusic.Play();
				FinalBattleMusic.volume = PlayerPrefs.GetFloat("music_volume") * PlayerPrefs.GetFloat("master_volume");
			}
			MazeLoader.mainPlayer.GetComponent<PlayerInfo>().HintPanel.SetActive(false);
			anim = GetComponentInParent<Animation>();
			anim.Play();
			down = true;
			if (player.Lvl == 6)
			{
				boss.transform.position += move;
			}
		}

		if (type == "Portal" && other.tag == "Player")
		{
			if (player.Lvl < 6)
			{
				Teleport.Play();
				if (PlayerPrefs.GetInt("max_chapter_num") == MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Lvl)
					PlayerPrefs.SetInt("max_chapter_num", PlayerPrefs.GetInt("max_chapter_num") + 1);
				MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Lvl++;

				//Loading next level
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				MazeLoader.Dying = false;
				SceneManager.LoadScene(MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Lvl);
			}
			else
			{
				FinalBattleMusic.Stop();
				EndScreen.SetActive(true);
				WinSound.GetComponent<AudioSource>().Play();
				MazeLoader.mainPlayer.GetComponent<PlayerController>().Engaged = true;
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
				CameraMove.CameraMoveEnabled = false;
			}
		}
	}
}
