using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Отвечает за локализацию игры (английская или русская).
/// </summary>
public class UpdateLocalization : MonoBehaviour
{
	/// <summary>
	/// Элемент выбора языка.
	/// </summary>
	Dropdown dropdown;
	/// <summary>
	/// Заданно ли значение по умолчанию.
	/// </summary>
	bool defaultValueSet;

	/// <summary>
	/// Задание значения по умолчанию.
	/// </summary>
	private void Start()
	{
		dropdown = gameObject.GetComponent<Dropdown>();
		if (PlayerPrefs.GetString("language") == "Русский") dropdown.value = 1;
		else dropdown.value = 0;
		defaultValueSet = true;
	}

	/// <summary>
	/// Изменение языка игры.
	/// </summary>
	public void ChangePreferences()
	{
		PlayerPrefs.SetString("language", dropdown.options[dropdown.value].text);
		if (defaultValueSet)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
