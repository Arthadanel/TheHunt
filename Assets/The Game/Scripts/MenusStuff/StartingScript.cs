using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Начальный скрипт игры.
/// </summary>
public class StartingScript : MonoBehaviour
{
	/// <summary>
	/// Создает значения по умолчанию для параметров, которые не были инициализированы ранее.
	/// </summary>
	void Start()
	{
		if (!PlayerPrefs.HasKey("language"))
		{
			PlayerPrefs.SetString("language", "English");
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		if (!PlayerPrefs.HasKey("max_chapter_num"))
			PlayerPrefs.SetInt("max_chapter_num", 1);
		if (!PlayerPrefs.HasKey("chapter_num"))
			PlayerPrefs.SetInt("chapter_num", 1);
		if (!PlayerPrefs.HasKey("deathcount"))
			PlayerPrefs.SetInt("deathcount", 0);
		if (!PlayerPrefs.HasKey("headcount"))
			PlayerPrefs.SetInt("headcount", 0);
		if (!PlayerPrefs.HasKey("bracers"))
			PlayerPrefs.SetString("bracers", "None");
		if (!PlayerPrefs.HasKey("aura"))
			PlayerPrefs.SetString("aura", "None");
		if (!PlayerPrefs.HasKey("sword"))
			PlayerPrefs.SetString("sword", "Broken");
		if (!PlayerPrefs.HasKey("charges"))
			PlayerPrefs.SetInt("charges", 0);
		if (!PlayerPrefs.HasKey("master_volume"))
			PlayerPrefs.SetFloat("master_volume", 1f);
		if (!PlayerPrefs.HasKey("music_volume"))
			PlayerPrefs.SetFloat("music_volume", 0.5f);
		if (!PlayerPrefs.HasKey("sound_volume"))
			PlayerPrefs.SetFloat("sound_volume", 0.3f);
		if (!PlayerPrefs.HasKey("camera_sensetivity"))
			PlayerPrefs.SetFloat("camera_sensetivity", 3f);
	}

	/// <summary>
	/// Обновление состояния игры (проверка ввода).
	/// </summary>
	private void Update()
	{
		ChaptersCheat();
	}

	/// <summary>
	/// Чит для открытия всех уровней.
	/// </summary>
	private void ChaptersCheat()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			PlayerPrefs.SetInt("max_chapter_num", 6);
		}
	}
}
