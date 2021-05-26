using UnityEngine;

/// <summary>
/// Локализация элементов игры.
/// </summary>
public class Localization : MonoBehaviour
{
	/// <summary>
	/// На русском ли языке текст элемента.
	/// </summary>
	[SerializeField] private bool Russian;

	/// <summary>
	/// Проверка, необходимо ли активировать или дизактивировать элемент в зависимости от выбранного в меню языка.
	/// </summary>
	void Start()
	{
		if (PlayerPrefs.GetString("language") == "Русский")
		{
			if (Russian) gameObject.SetActive(true);
			else gameObject.SetActive(false);
		}
		else if (PlayerPrefs.GetString("language") == "English")
		{
			if (!Russian) gameObject.SetActive(true);
			else gameObject.SetActive(false);
		}
	}
}
