using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за чувствительность камеры в игре.
/// </summary>
public class CameraSensetivity : MonoBehaviour
{
	/// <summary>
	/// Слайдер параметра "чувствительность камеры".
	/// </summary>
	[SerializeField] Slider camSens;

	/// <summary>
	/// Загружает значение слайдера, если оно было задано ранее.
	/// </summary>
	void Start()
	{
		if (PlayerPrefs.HasKey("camera_sensetivity"))
			camSens.value = PlayerPrefs.GetFloat("camera_sensetivity");
		else
			camSens.value = 6f;
	}

	/// <summary>
	/// Изменяет параметр чувствительности камеры.
	/// </summary>
	public void ChangeSensetivity()
	{
		CameraMove.cameraSensitivity = camSens.value;
		PlayerPrefs.SetFloat("camera_sensetivity", camSens.value);
	}

}
