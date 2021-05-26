using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управление звуком игры.
/// </summary>
public class Volume : MonoBehaviour
{
	/// <summary>
	/// Слайдер общей громкости.
	/// </summary>
	public Slider masterSlider;
	/// <summary>
	/// Источник музыки.
	/// </summary>
	public AudioSource musicSource;
	/// <summary>
	/// Слайдер громкости музыки.
	/// </summary>
	public Slider musicSlider;
	/// <summary>
	/// Источник звука.
	/// </summary>
	public AudioSource sfxSource;
	/// <summary>
	/// Слайдер громкости звуков.
	/// </summary>
	public Slider sfxSlider;

	/// <summary>
	/// Запускается автоматически при появлении объекта на сцене. Загружает данные о громкости, если они есть.
	/// </summary>
	private void Start()
	{
		sfxSource = GameObject.Find("Menu Click SFX Source").GetComponent<AudioSource>();
		musicSource = GameObject.Find("Menu Background Music Source").GetComponent<AudioSource>();

		if (PlayerPrefs.HasKey("sound_volume"))
		{
			sfxSlider.value = PlayerPrefs.GetFloat("sound_volume");
		}
		else
			sfxSlider.value = sfxSlider.maxValue;

		if (PlayerPrefs.HasKey("music_volume"))
		{
			musicSlider.value = PlayerPrefs.GetFloat("music_volume");
		}
		else
			musicSlider.value = musicSlider.maxValue;

		if (PlayerPrefs.HasKey("master_volume"))
		{
			masterSlider.value = PlayerPrefs.GetFloat("master_volume");
		}
		else
			masterSlider.value = masterSlider.maxValue;
	}

	/// <summary>
	/// Изменяет общую громкость.
	/// </summary>
	public void ChangeVolume()
	{
		ChangeMusicVolume();
		ChangeSfxVolume();
		PlayerPrefs.SetFloat("master_volume", masterSlider.value);
	}

	/// <summary>
	/// Изменяет громкость музыки.
	/// </summary>
	public void ChangeMusicVolume()
	{
		musicSource.volume = musicSlider.value * masterSlider.value;
		PlayerPrefs.SetFloat("music_volume", musicSlider.value);
	}

	/// <summary>
	/// Изменяет громкость звуков.
	/// </summary>
	public void ChangeSfxVolume()
	{
		sfxSource.volume = sfxSlider.value * masterSlider.value;
		if (MazeLoader.mainPlayer != null)
			MazeLoader.mainPlayer.GetComponent<PlayerController>().walk.volume = sfxSource.volume;
		PlayerPrefs.SetFloat("sound_volume", sfxSlider.value);
	}
}
