using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Основной класс. Отвечает за генерацию уровня и расстановку предметов. Управляет игровым процессом.
/// </summary>
public class MazeLoader : MonoBehaviour
{
	/// <summary>
	/// Основной шрифт для английского текста.
	/// </summary>
	[SerializeField] private Font mainFont;
	/// <summary>
	/// Шрифт для английского текста (заголовки).
	/// </summary>
	[SerializeField] private Font titleFont;
	/// <summary>
	/// Шрифт для английского текста (для длинных сообщений).
	/// </summary>
	[SerializeField] private Font textFont;
	/// <summary>
	/// Основной шрифт для русского текста.
	/// </summary>
	[SerializeField] private Font mainFontRus;
	/// <summary>
	/// Шрифт для русского текста (заголовки).
	/// </summary>
	[SerializeField] private Font titleFontRus;
	/// <summary>
	/// Шрифт для русского текста (для длинных сообщений).
	/// </summary>
	[SerializeField] private Font textFontRus;

	/// <summary>
	/// Экран, выводимый при завершении игры.
	/// </summary>
	[SerializeField] private GameObject endScreen;
	/// <summary>
	/// Звук, проигрываемый при завершении игры.
	/// </summary>
	[SerializeField] private AudioSource winSound;
	/// <summary>
	/// Музыка финального босса.
	/// </summary>
	[SerializeField] private AudioSource finalBattle;
	/// <summary>
	/// Звук телепортации.
	/// </summary>
	[SerializeField] private AudioSource teleport;

	/// <summary>
	/// Номер генерируемого уровня.
	/// </summary>
	[SerializeField] private int lvl;
	/// <summary>
	/// Размеры лабиринта.
	/// </summary>
	[SerializeField] private int mazeRows = 7, mazeColumns = 7;
	/// <summary>
	/// Доступ к количеству рядов лабиринта.
	/// </summary>
	public int MazeRows { get { return mazeRows; } }
	/// <summary>
	/// Доступ к количеству столбцов лабиринта.
	/// </summary>
	public int MazeColumns { get { return mazeColumns; } }

	/// <summary>
	/// Шанс генерации сундука.
	/// </summary>
	[SerializeField] private int chestChance = 100;
	/// <summary>
	/// Шанс генерации одержимого сундука.
	/// </summary>
	[SerializeField] private int posessedChestChance = 1000;
	/// <summary>
	/// Шанс генерации зелья.
	/// </summary>
	[SerializeField] private int buffChance = 775;
	/// <summary>
	/// Шанс генерации врага.
	/// </summary>
	[SerializeField] private int enemyChance = 500;
	/// <summary>
	/// Шанс генерации записки.
	/// </summary>
	[SerializeField] private int hintChance = 250;

	/// <summary>
	/// Стена.
	/// </summary>
	[SerializeField] private GameObject wall;
	/// <summary>
	/// Стена с дверью.
	/// </summary>
	[SerializeField] private GameObject wallDoor;
	/// <summary>
	/// Стена с проходом.
	/// </summary>
	[SerializeField] private GameObject wallDoorFrame;
	/// <summary>
	/// Стена с решетками.
	/// </summary>
	[SerializeField] private GameObject wallBars;
	/// <summary>
	/// Пол.
	/// </summary>
	[SerializeField] private GameObject floor;
	/// <summary>
	/// Потолок.
	/// </summary>
	[SerializeField] private GameObject ceiling;
	/// <summary>
	/// Колонна.
	/// </summary>
	[SerializeField] private GameObject pillar;

	/// <summary>
	/// Сундук.
	/// </summary>
	[SerializeField] private GameObject chest;
	/// <summary>
	/// Массив врагов.
	/// </summary>
	[SerializeField] private GameObject[] enemies;
	/// <summary>
	/// Массив зелий.
	/// </summary>
	[SerializeField] private GameObject[] buffs;
	/// <summary>
	/// Записка.
	/// </summary>
	[SerializeField] private GameObject hint;
	/// <summary>
	/// Игровой персонаж.
	/// </summary>
	[SerializeField] private GameObject playerPrefab;
	/// <summary>
	/// Дверь к конечной комнате.
	/// </summary>
	[SerializeField] private GameObject bossDoor;
	/// <summary>
	/// Не играбельный персонаж.
	/// </summary>
	[SerializeField] private GameObject npc;
	/// <summary>
	/// Куда скинуть всю архитиктуру лабиринта в иерархии Unity.
	/// </summary>
	[SerializeField] private GameObject MazeParent;
	/// <summary>
	/// Начальная комната.
	/// </summary>
	[SerializeField] private GameObject startRoom;
	/// <summary>
	/// Имя игрока.
	/// </summary>
	[SerializeField] private string playerName;
	/// <summary>
	/// Игрок.
	/// </summary>
	public static GameObject mainPlayer;

	/// <summary>
	/// Размер пола каждой ячейки лабиринта.
	/// </summary>
	static private float size = 6.75f;
	/// <summary>
	/// Свойство доступа к размеру пола каждой ячейки лабиринта.
	/// </summary>
	static public float Size { get { return size; } }

	/// <summary>
	/// Лабиринт (представлен массивом ячеек).
	/// </summary>
	static private MazeCell[,] mazeCells;
	/// <summary>
	/// Свойство доступа к массиву ячеек лабиринта.
	/// </summary>
	static public MazeCell[,] MazeCells { get { return mazeCells; } }

	/// <summary>
	/// Экран, отображаемый при смерти.
	/// </summary>
	[SerializeField] private GameObject DeathScreen;
	/// <summary>
	/// Звук, проигрываемый при смерти.
	/// </summary>
	[SerializeField] private GameObject DeathSound;
	/// <summary>
	/// Звук элементов меню.
	/// </summary>
	[SerializeField] private GameObject SfxSound;
	/// <summary>
	/// Фоновая музыка.
	/// </summary>
	[SerializeField] private GameObject BGMusic;
	/// <summary>
	/// Звук смеха.
	/// </summary>
	[SerializeField] private AudioSource laugh;
	/// <summary>
	/// Умирает ли персонаж.
	/// </summary>
	public static bool Dying { get; set; }

	/// <summary>
	/// Показан ли заголовок уровня.
	/// </summary>
	bool chapterTitleShown = true;
	/// <summary>
	/// Время начала уровня.
	/// </summary>
	float startTime;

	/// <summary>
	/// Информация об игроке.
	/// </summary>
	PlayerInfo player;

	/// <summary>
	/// Словарь для перевода текстов с английского на русский.
	/// </summary>
	public static Dictionary<string, string> EngToRus;
	/// <summary>
	/// Словарь для выбора нужного шрифта.
	/// </summary>
	public static Dictionary<string, Font> Fonts;

	/// <summary>
	/// Заполняет словарь нужными значениями.
	/// </summary>
	void FillDictionaries()
	{
		EngToRus = new Dictionary<string, string>();
		EngToRus.Add("Descend", "Спуститься");
		EngToRus.Add("Loot", "Взять");
		EngToRus.Add("Open", "Открыть");
		EngToRus.Add("Read", "Прочитать");
		EngToRus.Add("Close", "Закрыть");
		EngToRus.Add("Talk", "Говорить");
		EngToRus.Add("Who are you?", "Кто ты?");
		EngToRus.Add("Cooperate", "Объединиться");

		EngToRus.Add("Aura", "Аура");
		EngToRus.Add("Bracers", "Наручи");
		EngToRus.Add("Sword", "Меч");

		EngToRus.Add("None", "Нет");
		EngToRus.Add("Broken", "Сломан");
		EngToRus.Add("Rage", "Ярость");
		EngToRus.Add("Wisdom", "Мудрость");
		EngToRus.Add("Tranquility", "Спокойствие");
		EngToRus.Add("Leather", "Кожа");
		EngToRus.Add("Cloth", "Ткань");
		EngToRus.Add("Wood", "Дерево");
		EngToRus.Add("Steel", "Сталь");

		EngToRus.Add("Locked", "Закрыто");
		EngToRus.Add("Chapter", "Глава");

		Fonts = new Dictionary<string, Font>();
		Fonts.Add("main", mainFont);
		Fonts.Add("title", titleFont);
		Fonts.Add("text", textFont);
		Fonts.Add("main_rus", mainFontRus);
		Fonts.Add("title_rus", titleFontRus);
		Fonts.Add("text_rus", textFontRus);
	}

	/// <summary>
	/// Создание уровня, его заполнение, добавление игрока на уровень, инициализация полей, обновление некоторых сохранений.
	/// </summary>
	void Start()
	{
		FillDictionaries();

		Chest.PossessionChannce = posessedChestChance;
		PlayerPrefs.SetInt("chapter_num", lvl);

		startTime = Time.time;
		InitializeMaze();

		HuntAndKillAlgorithm mazeAlg = new HuntAndKillAlgorithm(mazeCells);
		mazeAlg.CreateMaze();

		PlaceUniqueObjects();
		PlaceObjects();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		Dying = false;
		Time.timeScale = 1f;

		mainPlayer = GameObject.FindGameObjectWithTag("Player");
		player = mainPlayer.GetComponent<PlayerInfo>();
		player.Lvl = lvl;

		GameObject.Find("ChapterTitle").GetComponent<Text>().text = (PlayerPrefs.GetString("language") == "English" ? "Chapter" : EngToRus["Chapter"]) + " " + (PlayerPrefs.GetInt("chapter_num").ToString()); //"Chapter 1: Honor";
		GameObject.Find("ChapterTitle").GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? Fonts["title"] : Fonts["title_rus"];

		GameObject tip = mainPlayer.GetComponent<PlayerInfo>().Tip;
		tip.SetActive(false);

		Pause.HUD = mainPlayer.GetComponent<PlayerInfo>().HUD;
		Pause.Notices = mainPlayer.GetComponent<PlayerInfo>().Notices;

		BossRoomEnter.WinSound = winSound;
		BossRoomEnter.EndScreen = endScreen;
		BossRoomEnter.FinalBattleMusic = finalBattle;
		BossRoomEnter.CurrentBGMusic = BGMusic.GetComponent<AudioSource>();
		BossRoomEnter.Teleport = teleport;

		SfxSound.GetComponent<AudioSource>().volume = PlayerPrefs.HasKey("master_volume") && PlayerPrefs.HasKey("sound_volume") ? PlayerPrefs.GetFloat("master_volume") * PlayerPrefs.GetFloat("sound_volume") : PlayerPrefs.HasKey("sound_volume") ? PlayerPrefs.GetFloat("sound_volume") : 1;
		DeathSound.GetComponent<AudioSource>().volume = PlayerPrefs.HasKey("master_volume") && PlayerPrefs.HasKey("sound_volume") ? PlayerPrefs.GetFloat("master_volume") * PlayerPrefs.GetFloat("sound_volume") : PlayerPrefs.HasKey("sound_volume") ? PlayerPrefs.GetFloat("sound_volume") : 1;
		BGMusic.GetComponent<AudioSource>().volume = PlayerPrefs.HasKey("master_volume") && PlayerPrefs.HasKey("music_volume") ? PlayerPrefs.GetFloat("master_volume") * PlayerPrefs.GetFloat("music_volume") : PlayerPrefs.HasKey("music_volume") ? PlayerPrefs.GetFloat("music_volume") : 1;

		if (PlayerPrefs.HasKey("camera_sensetivity"))
			CameraMove.cameraSensitivity = PlayerPrefs.GetFloat("camera_sensetivity");
	}

	/// <summary>
	/// Обновление состояния игры. Проверяет, нужно ли скрыть название уровня/запустить метод смерти персонажа/запустить метод предательства НИПа.
	/// </summary>
	void Update()
	{
		if (chapterTitleShown && Time.time - startTime > 1.5f)
		{
			GameObject.Find("ChapterTitle").SetActive(false);
			chapterTitleShown = false;
		}

		if (player.Lvl == 3 && player.Cooperating && player.HP <= player.maxHP * 0.5)
			NPC.Betray(player, laugh);

		if (!chapterTitleShown && !Dying && playerSet && mainPlayer != null && ((int)mainPlayer.GetComponent<PlayerInfo>().HP <= 0 && Time.timeScale == 1f) && mainPlayer.GetComponent<PlayerController>().Engaged)
		{
			GameOver();
		}
	}
	private void GameOver()
	{
		player = mainPlayer.GetComponent<PlayerInfo>();
		player.Deathcount++;
		Dying = true;
		player.Notices.SetActive(false);
		player.HUD.SetActive(false);
		DeathScreen.SetActive(true);
		BGMusic.GetComponent<AudioSource>().Stop();
		DeathSound.GetComponent<AudioSource>().Play();
		(DeathScreen.GetComponent<Animation>()).Play();
		mainPlayer.GetComponent<PlayerController>().Engaged = true;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		CameraMove.CameraMoveEnabled = false;
	}

	/// <summary>
	/// Создает пустой лабиринт со всеми четырьмя стенами в каждой ячейке.
	/// </summary>
	private void InitializeMaze()
	{
		mazeCells = new MazeCell[mazeRows, mazeColumns];
		GameObject[] walls = { wall, wallBars, wall, wall };
		int wallType;

		//
		//baseFloor.transform.localScale.Scale(new Vector3(10, 1, 10));
		//baseFloor = Instantiate(baseFloor, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		//

		for (int row = 0; row < mazeRows; row++)
		{
			for (int column = 0; column < mazeColumns; column++)
			{
				mazeCells[row, column] = new MazeCell();

				mazeCells[row, column].Floor = Instantiate(floor, new Vector3(row * size, 0, column * size), Quaternion.identity) as GameObject;
				mazeCells[row, column].Floor.transform.SetParent(MazeParent.transform, true);

				mazeCells[row, column].Ceiling = Instantiate(floor, new Vector3(row * size, 6.2f, column * size), Quaternion.identity) as GameObject;
				//mazeCells[row, column].Ceiling.transform.Rotate(Vector3.left * 180);
				mazeCells[row, column].Ceiling.transform.SetParent(MazeParent.transform, true);
				//mazeCells[row, column].Ceiling.SetActive(false);

				if (column == 0)
				{
					mazeCells[row, column].WallWest = Instantiate(wall, new Vector3(row * size, 0, (column * size) - (size / 2f)), Quaternion.identity) as GameObject;
					mazeCells[row, column].WallWest.transform.Rotate(Vector3.up * 90);
					mazeCells[row, column].WallWest.transform.SetParent(MazeParent.transform, true);
				}

				wallType = Random.Range(0, (mazeRows - 1 - row) * (mazeColumns - 1 - column) % 4 + 1);
				if (wallType != 0)
					mazeCells[row, column].WallEastEmpty = wallType == 0;
				mazeCells[row, column].WallEast = Instantiate(walls[wallType], new Vector3(row * size, 0, (column * size) + (size / 2f)), Quaternion.identity) as GameObject;
				mazeCells[row, column].WallEast.transform.Rotate(Vector3.up * 90);
				mazeCells[row, column].WallEast.transform.SetParent(MazeParent.transform, true);

				if (row == 0)
				{
					mazeCells[row, column].WallNorth = Instantiate(wall, new Vector3(row * size - (size / 2f), 0, (column * size)), Quaternion.identity) as GameObject;
					mazeCells[row, column].WallNorth.transform.SetParent(MazeParent.transform, true);
				}

				wallType = Random.Range(0, (mazeRows - 1 - row) * (mazeColumns - 1 - column) % 4 + 1);
				if (wallType != 0)
					mazeCells[row, column].WallSouthEmpty = false;
				mazeCells[row, column].WallSouth = Instantiate(walls[wallType], new Vector3(row * size + (size / 2f), 0, (column * size)), Quaternion.identity) as GameObject;
				mazeCells[row, column].WallSouth.transform.SetParent(MazeParent.transform, true);


				if (row == 0 & column == mazeColumns - 1)
				{
					mazeCells[row, column].Pillar = Instantiate(pillar, new Vector3(row * size - (size / 2f), 0, (column * size) + (size / 2f)), Quaternion.identity) as GameObject;
					mazeCells[row, column].Pillar.transform.SetParent(MazeParent.transform, true);
				}
				if (row == 0)
				{
					mazeCells[row, column].Pillar = Instantiate(pillar, new Vector3(row * size - (size / 2f), 0, (column * size) - (size / 2f)), Quaternion.identity) as GameObject;
					mazeCells[row, column].Pillar.transform.SetParent(MazeParent.transform, true);
				}
				if (column == mazeColumns - 1)
				{
					mazeCells[row, column].Pillar = Instantiate(pillar, new Vector3(row * size + (size / 2f), 0, (column * size) + (size / 2f)), Quaternion.identity) as GameObject;
					mazeCells[row, column].Pillar.transform.SetParent(MazeParent.transform, true);
				}
				mazeCells[row, column].Pillar = Instantiate(pillar, new Vector3(row * size + (size / 2f), 0, (column * size) - (size / 2f)), Quaternion.identity) as GameObject;
				mazeCells[row, column].Pillar.transform.SetParent(MazeParent.transform, true);

				floor.name = "Floor";
				pillar.name = "Pillar";
				wall.name = "Wall";
				wallBars.name = "Wall with bars";
			}
		}
	}
	bool playerSet;

	/// <summary>
	/// Заполняет лабиринт уникальными объектами.
	/// </summary>
	private void PlaceUniqueObjects()
	{
		int row;
		int column;
		bool bossDoorSet = false;
		bool npcSet = false;

		do
		{
			row = Random.Range(0, mazeRows);
			column = Random.Range(0, mazeColumns);

			if (!mazeCells[row, column].HasObject())
			{
				if (mazeCells[row, column].Walls() == 3 && !bossDoorSet)
				{
					mazeCells[row, column].BossDoor = true;
					Portal.Place(row, column, bossDoor);
					bossDoorSet = true;
				}
				else if ((PlayerPrefs.GetInt("chapter_num") == 2 || PlayerPrefs.GetInt("chapter_num") == 3) && mazeCells[row, column].Walls() == 3 && !npcSet)
				{
					mazeCells[row, column].CellNPC = true;
					NPC.Place(row, column, npc);
					npcSet = true;
				}
				else if (!playerSet)
				{
					mazeCells[row, column].CellPlayer = true;
					PlayerInfo.Place(row, column, playerPrefab, playerName);
					playerSet = true;
					Instantiate(startRoom, new Vector3(row * Size, 6f, column * Size), Quaternion.identity);
				}
			}
		} while (!((npcSet || PlayerPrefs.GetInt("chapter_num") < 2 || PlayerPrefs.GetInt("chapter_num") > 3) && bossDoorSet && playerSet));
	}

	/// <summary>
	/// Заполняет лабиринт повторяющимися объектами.
	/// </summary>
	private void PlaceObjects()
	{
		for (int row = 0; row < mazeRows; row++)
		{
			for (int column = 0; column < mazeColumns; column++)
			{
				switch (mazeCells[row, column].Walls())
				{
					case 1:
						if (!mazeCells[row, column].HasObject() && Random.Range(1, 1000) > hintChance)
						{
							mazeCells[row, column].CellHint = true;
							Hint.Place(row, column, hint);
						}
						break;
					case 2:
						if (!mazeCells[row, column].HasObject() && !((MazeCells[row, column].N | MazeCells[row, column].S) && (MazeCells[row, column].W | MazeCells[row, column].E)) && Random.Range(1, 1000) > enemyChance)
						{
							mazeCells[row, column].CellEnemy = true;
							Enemy.Place(row, column, enemies[Random.Range(0, enemies.Length)]);
						}
						if (!mazeCells[row, column].HasObject() && ((MazeCells[row, column].N | MazeCells[row, column].S) && (MazeCells[row, column].W | MazeCells[row, column].E)) && Random.Range(1, 1000) > buffChance)
						{
							mazeCells[row, column].CellBuffs = true;
							Buffs.Place(row, column, buffs[Random.Range(0, buffs.Length)]);
						}
						break;
					case 3:
						if (!mazeCells[row, column].HasObject() && Random.Range(1, 1000) > chestChance)
						{
							mazeCells[row, column].CellChest = true;
							Chest.Place(row, column, chest);
						}
						break;
					default:
						break;
				}
			}
		}
	}
}