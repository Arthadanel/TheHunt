using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Класс, отвечающий за выбор элемента меню при вводе с клавиатуры.
/// </summary>
public class SelectOnInput : MonoBehaviour
{
	/// <summary>
	/// Источник фоновой музыки.
	/// </summary>
	[SerializeField] AudioSource BGMusic;
	/// <summary>
	/// Источник звука.
	/// </summary>
	[SerializeField] AudioSource SfxSound;

	/// <summary>
	/// Система событий.
	/// </summary>
	public EventSystem eventSystem;
	/// <summary>
	/// Выбранный объект.
	/// </summary>
	public GameObject selectedObject;

	/// <summary>
	/// Выбрана ли кнопка.
	/// </summary>
	private bool buttonSelected;

	/// <summary>
	/// Начальный метод. Инициализация громкости.
	/// </summary>
	void Start()
	{
		SfxSound.volume = PlayerPrefs.HasKey("master_volume") && PlayerPrefs.HasKey("sound_volume") ? PlayerPrefs.GetFloat("master_volume") * PlayerPrefs.GetFloat("sound_volume") : PlayerPrefs.HasKey("sound_volume") ? PlayerPrefs.GetFloat("sound_volume") : 1;
		BGMusic.volume = PlayerPrefs.HasKey("master_volume") && PlayerPrefs.HasKey("music_volume") ? PlayerPrefs.GetFloat("master_volume") * PlayerPrefs.GetFloat("music_volume") : PlayerPrefs.HasKey("music_volume") ? PlayerPrefs.GetFloat("music_volume") : 1;
	}

	/// <summary>
	/// Проверяет ввод.
	/// </summary>
	void Update()
	{
		if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
		{
			eventSystem.SetSelectedGameObject(selectedObject);
			buttonSelected = true;
		}
	}

	/// <summary>
	/// Действия, производимые при дизактивации элемента.
	/// </summary>
	private void OnDisable()
	{
		buttonSelected = false;
	}
}
