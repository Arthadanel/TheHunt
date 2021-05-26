using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управление поворотом камеры.
/// </summary>
public class CameraMove : MonoBehaviour
{
	/// <summary>
	/// Чувствительность камеры.
	/// </summary>
	internal static float cameraSensitivity;

	/// <summary>
	/// Включено ли движение камеры.
	/// </summary>
	private static bool cameraMoveEnabled;
	/// <summary>
	/// Включено ли движение камеры.
	/// </summary>
	public static bool CameraMoveEnabled { get { return cameraMoveEnabled; } set { cameraMoveEnabled = value; } }

	/// <summary>
	/// Начальная загрузка некоторых параметров.
	/// </summary>
	void Start()
	{
		if (PlayerPrefs.HasKey("camera_sensetivity")) cameraSensitivity = PlayerPrefs.GetFloat("camera_sensetivity");
		else cameraSensitivity = 1f;
		cameraMoveEnabled = true;
	}

	/// <summary>
	/// Обновление состояния камеры.
	/// </summary>
	void Update()
	{
		if (CameraMoveEnabled)
		{

			if ((int)transform.localRotation.eulerAngles.y == 0 || (transform.localRotation.eulerAngles.x > 100 && Input.GetAxis("Mouse Y") < 0 || transform.localRotation.eulerAngles.x < 100 && Input.GetAxis("Mouse Y") > 0))
			{
				transform.parent.Rotate(0, Input.GetAxis("Mouse X") * cameraSensitivity, 0);
				transform.Rotate(-Input.GetAxis("Mouse Y") * cameraSensitivity, 0, 0);
			}
		}
	}
}
