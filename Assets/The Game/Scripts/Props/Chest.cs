using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс сундука.
/// </summary>
public class Chest : MonoBehaviour
{
	/// <summary>
	/// Звук атаки сундука.
	/// </summary>
	[SerializeField] internal AudioSource Attack;
	/// <summary>
	/// Звук издаваемый при смерти духа, которым одержим сундук.
	/// </summary>
	[SerializeField] internal AudioSource Scream;
	/// <summary>
	/// Шанс того, что сундук будет одержим духом.
	/// </summary>
	internal static int PossessionChannce;

	/// <summary>
	/// Открыт ли сундук.
	/// </summary>
	internal bool Opened { get; private set; }
	/// <summary>
	/// Забран ли предмет из сундука.
	/// </summary>
	internal bool Looted { get; set; }
	/// <summary>
	/// Одержим ли сундук.
	/// </summary>
	internal bool Possessed { get; set; }

	/// <summary>
	/// Индекс предмета, лежащего в сундуке.
	/// </summary>
	int item;
	/// <summary>
	/// Тип предмета, лежащего в сундуке.
	/// </summary>
	string itemType;
	/// <summary>
	/// Объект интерфейса персонажа, на который выводятся сообщения о возможных действиях.
	/// </summary>
	GameObject tip;

	/// <summary>
	/// Запускается при появлении объекта на сцене. Расчитывает, одержим ли сундук и находит ссылки на некоторые объекты.
	/// </summary>
	private void Start()
	{
		Possessed = Random.Range(0, 1000) > PossessionChannce;
		tip = MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip;
	}

	/// <summary>
	/// Ставит сундук в заданной ячейке лабиринта.
	/// </summary>
	/// <param name="row">Ряд ячейки.</param>
	/// <param name="column">Колонка ячейки.</param>
	/// <param name="chestPrefab">Префаб сундука.</param>
	public static void Place(int row, int column, GameObject chestPrefab)
	{
		chestPrefab = Instantiate(chestPrefab, new Vector3(row * MazeLoader.Size, 0, column * MazeLoader.Size), Quaternion.identity);
		if (!MazeLoader.MazeCells[row, column].E)
			chestPrefab.transform.Rotate(Vector3.up * 90);
		else if (!MazeLoader.MazeCells[row, column].S)
			chestPrefab.transform.Rotate(Vector3.up * 180);
		else if (!MazeLoader.MazeCells[row, column].W)
			chestPrefab.transform.Rotate(Vector3.up * 270);
		chestPrefab.name = "Chest";
	}

	/// <summary>
	/// Открытие сундука.
	/// </summary>
	public void Open()
	{
		Opened = true;
		(gameObject.transform.GetChild(0).GetComponent<Animation>()).Play();
		tip.transform.GetChild(0).GetComponent<Text>().text = "E";
		tip.transform.GetChild(1).GetComponent<Text>().text = (PlayerPrefs.GetString("language") == "English" ? "Loot" : MazeLoader.EngToRus["Loot"]) + " " + GetItem();
		tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
	}

	/// <summary>
	/// Получение предмета из сундука.
	/// </summary>
	public void Loot()
	{
		Looted = true;
		var selectedItem = MazeLoader.mainPlayer.transform.GetChild(1).GetChild(2).GetChild(item + 1);
		selectedItem.GetChild(3).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? itemType : MazeLoader.EngToRus[itemType];
		selectedItem.GetChild(3).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];

		switch (item)
		{
			case 0:
				MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Aura = itemType;
				break;
			case 1:
				MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Bracers = itemType;
				break;
			case 2:
				MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Sword = itemType;
				break;
			default:
				break;
		}
		tip.SetActive(false);
	}

	/// <summary>
	/// Генерация предмета сундука.
	/// </summary>
	/// <returns></returns>
	private string GetItem()
	{
		string[][] items = new string[][] { new string[] { "Wisdom", "Rage", "Tranquility" }, new string[] { "Cloth", "Leather" }, new string[] { "Wood", "Steel" }, new string[] { "Aura", "Bracers", "Sword" } };
		item = Random.Range(0, 3);
		itemType = items[item][item == 0 ? Random.Range(0, 3) : Random.Range(0, 2)];
		return '『' + (PlayerPrefs.GetString("language") == "English" ? items[3][item] : MazeLoader.EngToRus[items[3][item]]) + '(' + (PlayerPrefs.GetString("language") == "English" ? itemType : MazeLoader.EngToRus[itemType]) + ')' + '』';

	}
}
