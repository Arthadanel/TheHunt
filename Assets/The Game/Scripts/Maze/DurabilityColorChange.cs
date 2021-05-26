using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Регулирует изменеие цвета индикатора прочности снаряжения.
/// </summary>
public class DurabilityColorChange : MonoBehaviour
{
	/// <summary>
	/// Слайдер прочности.
	/// </summary>
    [SerializeField] private Slider slider;
	/// <summary>
	/// Заполнение слайдера (здесь меняем цвет).
	/// </summary>
	[SerializeField] private GameObject fill;
	/// <summary>
	/// Цвет при минимальной прочности.
	/// </summary>
	[SerializeField] private Color32 lowColor;
	/// <summary>
	/// Цвет при максимальной прочности.
	/// </summary>
	[SerializeField] private Color32 fullColor;

	/// <summary>
	/// Расчитывает и изменяет цвет слайдера.
	/// </summary>
    void Update()
    {
        fill.GetComponent<Image>().color = Color32.Lerp(lowColor, fullColor, slider.value/slider.maxValue);
    }
}
