using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за кнопки перехода к конкретным уровням.
/// </summary>
public class ChapterAvailable : MonoBehaviour
{
	/// <summary>
	/// В зависимости от того, доступен ли уровень, активирует или дизактивирует кнопку.
	/// </summary>
	void Update()
	{
		if (int.Parse(transform.GetChild(0).GetComponent<Text>().text) > PlayerPrefs.GetInt("max_chapter_num"))
			GetComponent<Button>().interactable = false;
		else
			GetComponent<Button>().interactable = true;
	}
}
