using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Добавляет звук на элементы меню.
/// </summary>
public class AddClickSfx : MonoBehaviour
{
	/// <summary>
	/// Кнопка.
	/// </summary>
	private Button button;
	/// <summary>
	/// Слайдер.
	/// </summary>
	private Slider slider;
	/// <summary>
	/// Переключатель.
	/// </summary>
	private Toggle toggle;
	/// <summary>
	/// Звук, добавляемый на элемент.
	/// </summary>
	private AudioSource audioSource;

	/// <summary>
	/// Добавление звука.
	/// </summary>
	void Start()
	{
		audioSource = GameObject.Find("Menu Click SFX Source").GetComponent<AudioSource>();
		try
		{
			button = GetComponentInParent<Button>();
			button.onClick.AddListener(audioSource.Play);
		}
		catch (System.NullReferenceException)
		{
			try
			{
				slider = GetComponentInParent<Slider>();
				slider.onValueChanged.AddListener(delegate { audioSource.Play(); });
			}
			catch (System.NullReferenceException)
			{
				toggle = GetComponentInParent<Toggle>();
				toggle.onValueChanged.AddListener(delegate { audioSource.Play(); });
			}
		}
	}
}