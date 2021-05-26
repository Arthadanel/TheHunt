using UnityEngine;

/// <summary>
/// Отвечает за сбросс сохранений.
/// </summary>
public class ResetSaves : MonoBehaviour
{
	/// <summary>
	/// Удаляет сохраненные данные об игровом процессе, инициализируя их значениями по умолчанию.
	/// </summary>
	public void EraseSaveData()
	{
		PlayerPrefs.SetInt("max_chapter_num", 1);
		PlayerPrefs.SetInt("chapter_num", 1);
		PlayerPrefs.SetInt("deathcount", 0);
		PlayerPrefs.SetInt("headcount", 0);
		PlayerPrefs.SetString("bracers", "None");
		PlayerPrefs.SetString("aura", "None");
		PlayerPrefs.SetString("sword", "Broken");
		PlayerPrefs.SetInt("charges", 0);
	}
}
