using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Информация об игровом персонаже.
/// </summary>
public class PlayerInfo : MonoBehaviour
{
	/// <summary>
	/// Табличка с именем НИПа, с которым объеденился персонаж.
	/// </summary>
	[SerializeField] internal GameObject NameplateNPC;
	/// <summary>
	/// Индикатор количества убитых врагов.
	/// </summary>
	[SerializeField] Text HeadcountIndicator;
	/// <summary>
	/// 
	/// </summary>
	[SerializeField] internal GameObject alert;
	/// <summary>
	/// Меч.
	/// </summary>
	[SerializeField] GameObject swordProp;
	/// <summary>
	/// Индикатор очков здоровья врага.
	/// </summary>
	[SerializeField] internal GameObject EnemyHP;
	/// <summary>
	/// Некоторая информация о состоянии игрока (очки здоровья, выносливость и т.д.)
	/// </summary>
	[SerializeField] internal GameObject HUD;
	/// <summary>
	/// Уведомления, отображаемые на экране перед игроком.
	/// </summary>
	[SerializeField] internal GameObject Notices;
	/// <summary>
	/// Подсказки (какое действие можно выполнить).
	/// </summary>
	[SerializeField] internal GameObject Tip;
	/// <summary>
	/// Панель отображения записки.
	/// </summary>
	[SerializeField] internal GameObject HintPanel;

	/// <summary>
	/// Анимация атаки игрока.
	/// </summary>
	internal Animation AttackAnim { get { return swordProp.GetComponent<Animation>(); } }

	/// <summary>
	/// Индикатор здоровья игрока.
	/// </summary>
	[SerializeField] Slider HPSlider;
	/// <summary>
	/// Индикатор выносливости игрока.
	/// </summary>
	[SerializeField] Slider SPSlider;
	/// <summary>
	/// Индикатор дополнительной выносливости.
	/// </summary>
	[SerializeField] Slider ESPSlider;

	/// <summary>
	/// Объдинился ли игрок с каким-либо НИПом.
	/// </summary>
	internal bool Cooperating;
	/// <summary>
	/// НИП, с которым объединился персонаж.
	/// </summary>
	internal NPC CoopNPC;

	/// <summary>
	/// Уровень, на котором в данный момент находится персонаж.
	/// </summary>
	private int lvl;
	/// <summary>
	/// Уровень, на котором в данный момент находится персонаж.
	/// </summary>
	public int Lvl { get { return lvl; } set { lvl = value; PlayerPrefs.SetInt("chapter_num", Lvl); } }

	/// <summary>
	/// Количество убитых врагов.
	/// </summary>
	private int headcount;
	/// <summary>
	/// Количество убитых врагов.
	/// </summary>
	public int Headcount { get { return headcount; } set { headcount = value; HeadcountIndicator.text = value.ToString(); PlayerPrefs.SetInt("headcount", value); } }

	/// <summary>
	/// Количество смертей персонажа.
	/// </summary>
	public int Deathcount { get { return PlayerPrefs.GetInt("deathcount"); } internal set { PlayerPrefs.SetInt("deathcount", value); } }

	/// <summary>
	/// Нацден ли на уровне НИП.
	/// </summary>
	internal bool NPCFound { get; set; }

	/// <summary>
	/// Имя персонажа.
	/// </summary>
	private static string alias;
	/// <summary>
	/// Свойство доступа к именю персонажа.
	/// </summary>
	public static string Alias { get { return alias; } set { alias = value; } }
	/// <summary>
	/// Текущие очки здоровья.
	/// </summary>
	private float hp;
	/// <summary>
	/// Текущие очки здоровья.
	/// </summary>
	public float HP { get { return hp; } set { hp = value; HPSlider.value = value; } }
	/// <summary>
	/// Максимальное возможное количество очков здоровья.
	/// </summary>
	public float maxHP { get; private set; }
	/// <summary>
	/// Текущие очки выносливости.
	/// </summary>
	private float sp;
	/// <summary>
	/// Текущие очки выносливости.
	/// </summary>
	public float SP { get { return sp; } set { sp = value; SPSlider.value = value; } }
	/// <summary>
	/// Максимальное возможное количество очков выносливости.
	/// </summary>
	public float maxSP { get; private set; }
	/// <summary>
	/// Текущие дополнительные очки выносливости.
	/// </summary>
	private float extraSP;
	/// <summary>
	/// Текущие дополнительные очки выносливости.
	/// </summary>
	public float ExtraSP { get { return extraSP; } set { if (value <= maxExtraSP) { extraSP = value; ESPSlider.value = value; } } }
	/// <summary>
	/// Максимальное возможное количество дополнительных очков выносливости.
	/// </summary>
	public float maxExtraSP { get; private set; }

	/// <summary>
	/// Базовый физический урон.
	/// </summary>
	private float atk;
	/// <summary>
	/// Показатель физического урона.
	/// </summary>
	public float Atk { get { return atk + atkBonus; } }
	/// <summary>
	/// Бонусный физический урон.
	/// </summary>
	private float atkBonus
	{
		get
		{
			float dmg = 0;
			switch (sword)
			{
				case "Wood":
					dmg += atk * 0.15f;
					break;
				case "Steel":
					dmg += atk * 0.27f;
					break;
				default:
					break;
			}
			switch (Aura)
			{
				case "Rage":
					dmg += atk * 0.3f;
					break;
				default:
					break;
			}
			return dmg;
		}
	}

	/// <summary>
	/// Базовая защита.
	/// </summary>
	private float def;
	/// <summary>
	/// Бонусная защита.
	/// </summary>
	private float defBonus
	{
		get
		{
			float rsst = 0;
			switch (bracers)
			{
				case "Cloth":
					rsst += def * 0.5f;
					break;
				case "Leather":
					rsst += def * 0.9f;
					break;
				default:
					break;
			}
			switch (Aura)
			{
				case "Tranquility":
					rsst += def * 1.3f;
					break;
				default:
					break;
			}
			return rsst;
		}
	}
	/// <summary>
	/// Показатель защиты персонажа.
	/// </summary>
	public float Def { get { return def + defBonus; } }

	/// <summary>
	/// Показатель урона атаки магическими зарядами.
	/// </summary>
	public float MgcAtc { get { float dmg = atk * 1.5f; if (aura == "Wisdom") dmg *= 2f; return dmg; } }

	/// <summary>
	/// Текущее количество магических зарядов.
	/// </summary>
	private int magicCharges;
	/// <summary>
	/// Текущее количество магических зарядов.
	/// </summary>
	public int MgcCharges { get { return magicCharges; } set { magicCharges = value; GameObject.Find("MagicCounter").GetComponent<Text>().text = value.ToString(); PlayerPrefs.SetInt("charges", value); } }

	/// <summary>
	/// Тип ауры персонажа.
	/// </summary>
	private string aura = "None";
	/// <summary>
	/// Прочность ауры.
	/// </summary>
	private int auraDurability;
	/// <summary>
	/// Прочность ауры.
	/// </summary>
	public int AuraDurability
	{
		get { return auraDurability; }
		set
		{
			auraDurability = value;
			GameObject.Find("AuraDurability").GetComponent<Slider>().value = auraDurability;
		}
	}
	/// <summary>
	/// Тип ауры персонажа.
	/// </summary>
	public string Aura
	{
		get { return aura; }
		set
		{
			aura = value;
			GameObject.Find("AuraType").GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? value : MazeLoader.EngToRus[value];
			GameObject.Find("AuraType").GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
			switch (value)
			{
				case "Rage":
					auraDurability = 10;
					GameObject.Find("AuraDurability").GetComponent<Slider>().maxValue = auraDurability;
					GameObject.Find("AuraDurability").GetComponent<Slider>().value = auraDurability;
					GameObject.Find("AuraSlot").GetComponent<Image>().color = new Color32(209, 17, 17, 255);
					break;
				case "Tranquility":
					auraDurability = 20;
					GameObject.Find("AuraDurability").GetComponent<Slider>().maxValue = auraDurability;
					GameObject.Find("AuraDurability").GetComponent<Slider>().value = auraDurability;
					GameObject.Find("AuraSlot").GetComponent<Image>().color = new Color32(0, 255, 17, 255);
					break;
				case "Wisdom":
					auraDurability = 15;
					GameObject.Find("AuraDurability").GetComponent<Slider>().maxValue = auraDurability;
					GameObject.Find("AuraDurability").GetComponent<Slider>().value = auraDurability;
					GameObject.Find("AuraSlot").GetComponent<Image>().color = new Color32(0, 166, 255, 255);
					break;
				default:
					auraDurability = 0;
					GameObject.Find("AuraSlot").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
					break;
			}
			PlayerPrefs.SetString("aura", value);
		}
	}

	/// <summary>
	/// Тип наручей персонажа.
	/// </summary>
	private string bracers = "None";
	/// <summary>
	/// Прочность наручей.
	/// </summary>
	private int bracersDurability;
	/// <summary>
	/// Прочность наручей.
	/// </summary>
	public int BracersDurability
	{
		get { return bracersDurability; }
		set
		{
			bracersDurability = value;
			GameObject.Find("BracersDurability").GetComponent<Slider>().value = bracersDurability;
		}
	}
	/// <summary>
	/// Тип наручей
	/// </summary>
	public string Bracers
	{
		get { return bracers; }
		set
		{
			bracers = value;
			GameObject.Find("BracersType").GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? value : MazeLoader.EngToRus[value];
			GameObject.Find("BracersType").GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
			switch (value)
			{
				case "Leather":
					bracersDurability = 70;
					GameObject.Find("BracersDurability").GetComponent<Slider>().maxValue = bracersDurability;
					GameObject.Find("BracersDurability").GetComponent<Slider>().value = bracersDurability;
					GameObject.Find("BracersSlot").GetComponent<Image>().color = new Color32(99, 55, 45, 255);
					break;
				case "Cloth":
					bracersDurability = 55;
					GameObject.Find("BracersDurability").GetComponent<Slider>().maxValue = bracersDurability;
					GameObject.Find("BracersDurability").GetComponent<Slider>().value = bracersDurability;
					GameObject.Find("BracersSlot").GetComponent<Image>().color = new Color32(120, 134, 166, 255);
					break;
				default:
					bracersDurability = 0;
					GameObject.Find("BracersSlot").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
					break;
			}
			PlayerPrefs.SetString("bracers", value);
		}
	}

	/// <summary>
	/// Тип меча персонажа.
	/// </summary>
	private string sword = "Broken";
	/// <summary>
	/// Прочность меча.
	/// </summary>
	private int swordDurability;
	/// <summary>
	/// Прочность меча.
	/// </summary>
	public int SwordDurability
	{
		get { return swordDurability; }
		set
		{
			swordDurability = value;
			GameObject.Find("SwordDurability").GetComponent<Slider>().value = swordDurability;
		}
	}
	/// <summary>
	/// Тип меча.
	/// </summary>
	public string Sword
	{
		get { return sword; }
		set
		{
			sword = value;
			GameObject.Find("SwordType").GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? value : MazeLoader.EngToRus[value];
			GameObject.Find("SwordType").GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
			switch (value)
			{
				case "Wood":
					swordDurability = 35;
					GameObject.Find("SwordDurability").GetComponent<Slider>().maxValue = swordDurability;
					GameObject.Find("SwordDurability").GetComponent<Slider>().value = swordDurability;
					GameObject.Find("SwordSlot").GetComponent<Image>().color = new Color32(99, 73, 59, 255);
					swordProp.GetComponent<Renderer>().material.color= new Color32(55, 35, 25, 255);
					break;
				case "Steel":
					swordDurability = 50;
					GameObject.Find("SwordDurability").GetComponent<Slider>().maxValue = swordDurability;
					GameObject.Find("SwordDurability").GetComponent<Slider>().value = swordDurability;
					GameObject.Find("SwordSlot").GetComponent<Image>().color = new Color32(85, 85, 90, 255);
					swordProp.GetComponent<Renderer>().material.color = new Color32(75, 75, 80, 255);
					break;
				default:
					swordDurability = 0;
					GameObject.Find("SwordSlot").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
					swordProp.GetComponent<Renderer>().material.color = new Color32(156, 156, 156, 255);
					break;
			}
			PlayerPrefs.SetString("sword", value);
		}
	}

	/// <summary>
	/// Инициализация начальных параметров персонажа.
	/// </summary>
	private void Start()
	{
		if (PlayerPrefs.HasKey("headcount")) Headcount = PlayerPrefs.GetInt("headcount");
		else Headcount = 0;
		if (PlayerPrefs.HasKey("bracers")) Bracers = PlayerPrefs.GetString("bracers");
		else Bracers = "None";
		if (PlayerPrefs.HasKey("aura")) Aura = PlayerPrefs.GetString("aura");
		else Aura = "None";
		if (PlayerPrefs.HasKey("sword")) Sword = PlayerPrefs.GetString("sword");
		else Sword = "Broken";
		if (PlayerPrefs.HasKey("charges")) MgcCharges = PlayerPrefs.GetInt("charges");
		else MgcCharges = 0;

		HeadcountIndicator.text = Headcount.ToString();

		if (!PlayerPrefs.HasKey("chapter_num"))
			Lvl = 1;
		else
			Lvl = PlayerPrefs.GetInt("chapter_num");
		switch (lvl)
		{
			case 1:
				maxHP = 75;
				maxSP = 3;
				maxExtraSP = 2;
				ExtraSP = 0;
				atk = 14;
				def = 0.45f;
				break;
			case 2:
				maxHP = 150;
				maxSP = 5;
				maxExtraSP = 2;
				ExtraSP = 0;
				atk = 28;
				def = 0.5f;
				break;
			case 3:
				maxHP = 175;
				maxSP = 6;
				maxExtraSP = 3;
				ExtraSP = 0;
				atk = 32;
				def = 0.5f;
				break;
			case 4:
				maxHP = 200;
				maxSP = 7;
				maxExtraSP = 3;
				ExtraSP = 0;
				atk = 35;
				def = 0.6f;
				break;
			case 5:
				maxHP = 225;
				maxSP = 8;
				maxExtraSP = 4;
				ExtraSP = 0;
				atk = 40;
				def = 0.6f;
				break;
			case 6:
				maxHP = 500;
				maxSP = 10;
				maxExtraSP = 4;
				ExtraSP = 0;
				atk = 40;
				def = 0.6f;
				break;
		}
		HP = maxHP;
		SP = maxSP;
		GameObject.Find("HP").GetComponent<Slider>().maxValue = maxHP;
		GameObject.Find("SP").GetComponent<Slider>().maxValue = maxSP;
		GameObject.Find("ExtraSP").GetComponent<Slider>().maxValue = maxExtraSP;
		GameObject.Find("HP").GetComponent<Slider>().value = hp;
		GameObject.Find("SP").GetComponent<Slider>().value = sp;
		GameObject.Find("ExtraSP").GetComponent<Slider>().value = extraSP;
	}

	/// <summary>
	/// Проверка, выполнил ли игрок требование, необходимое для прохождения уровня.
	/// </summary>
	/// <returns></returns>
	public bool ConditionMet()
	{
		switch (lvl)
		{
			case 1: return headcount >= 5;
			case 2: return NPCFound;
			case 3: return NPCFound;
			case 4: return Deathcount > 10;
			case 5: return headcount >= 50;
			case 6: return true;
			default: return true;
		}
	}

	/// <summary>
	/// Отдыхает ли персонаж (не сражается и не защищается).
	/// </summary>
	private bool resting;
	/// <summary>
	/// Отдыхает ли персонаж (не сражается и не защищается).
	/// </summary>
	public bool Resting { get { return resting; } }
	/// <summary>
	/// Выполнил ли персонаж первый удар по врагу.
	/// </summary>
	private bool firstMovePerformed;

	/// <summary>
	/// Дизактивация(уничтожение) поверженного врага.
	/// </summary>
	/// <param name="enemy"></param>
	private void DisactivateEnemy(Enemy enemy)
	{
		EnemyHP.SetActive(false);
		enemy.Active = false;
		Tip.SetActive(false);
		MazeLoader.mainPlayer.GetComponent<PlayerController>().Engaged = false;
		if (Notices.activeSelf && Aura != "None")
			AuraDurability--;
		firstMovePerformed = false;
		Headcount++;
	}

	/// <summary>
	/// Метод сражения с врагом.
	/// </summary>
	/// <param name="enemy">Враг, с которым сражается персонаж.</param>
	public void Engage(Enemy enemy)
	{
		Animation anim = swordProp.GetComponent<Animation>();
		resting = false;
		if (enemy.Type != "Eagle" && enemy.Type != "Knight")
			firstMovePerformed = true;
		if (Input.GetMouseButtonDown(0))
		{
			if (firstMovePerformed)
				enemy.Engage(this);
			if (enemy.Active)
			{
				if ((sword != "Steel" && SP + ExtraSP >= 2) || SP + ExtraSP >= 3)
				{
					anim.Play();
					if (Cooperating && (enemy.Type == "Chimera" || enemy.Type == "Spiders"))
						enemy.HP -= Atk * 2 * (1 - (enemy.OnGuard ? enemy.Def : 0));
					else
						enemy.HP -= Atk * (1 - (enemy.OnGuard ? enemy.Def : 0));
					firstMovePerformed = true;
					for (int i = 0; i < 2; i++)
					{
						if (ExtraSP > 0)
							ExtraSP--;
						else
							SP--;
					}
					if (sword == "Steel")
						if (ExtraSP > 0)
							ExtraSP--;
						else
							SP--;
					if (Notices.activeSelf && Sword != "Broken")
						SwordDurability--;
				}

				if (enemy.HP <= 0)
				{
					DisactivateEnemy(enemy);
				}
			}
		}
		else if (Input.GetMouseButton(1))
		{
			if (firstMovePerformed)
				enemy.Engage(this, true);
			firstMovePerformed = true;
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			if (firstMovePerformed)
				enemy.Engage(this);
			if (enemy.Active)
			{
				if (Notices.activeSelf && MgcCharges > 0)
				{
					enemy.HP -= MgcAtc * (1 - (enemy.OnGuard ? enemy.Def : 0));
					MgcCharges--;
				}

				firstMovePerformed = true;

				if (enemy.HP <= 0)
				{
					DisactivateEnemy(enemy);
				}
			}
		}
		else if (firstMovePerformed)
		{
			enemy.Engage(this);
			if (!anim.isPlaying)
				resting = true;
		}
	}

	/// <summary>
	/// Метод помещения игрока в заданную ячейку лабиринта.
	/// </summary>
	/// <param name="row"></param>
	/// <param name="column"></param>
	/// <param name="body"></param>
	/// <param name="name"></param>
	public static void Place(int row, int column, GameObject body, string name = "")
	{
		Alias = name;
		body = Instantiate(body, new Vector3(row * MazeLoader.Size, 6.5f, column * MazeLoader.Size), Quaternion.identity);
		Destroy(MazeLoader.MazeCells[row, column].Floor);
		if (!MazeLoader.MazeCells[row, column].S)
			body.transform.Rotate(Vector3.up * 90);
		else if (!MazeLoader.MazeCells[row, column].W)
			body.transform.Rotate(Vector3.up * 180);
		else if (!MazeLoader.MazeCells[row, column].N)
			body.transform.Rotate(Vector3.up * 270);
		MazeLoader.MazeCells[row, column].Ceiling.SetActive(false);
		body.name = Alias;
	}

	/// <summary>
	/// Проверяет, не сломались ли предметы персонажа.
	/// </summary>
	private void Update()
	{
		if (Notices.activeSelf)
		{
			if (auraDurability == 0)
				Aura = "None";
			if (bracersDurability == 0)
				Bracers = "None";
			if (swordDurability == 0)
				Sword = "Broken";
		}
		HealCheat();
	}

	/// <summary>
	/// Обновление некоторых параметров персонажа.
	/// </summary>
	private void FixedUpdate()
	{
		if (HUD.activeSelf)
		{
			HP += 0.001f;
			SP += 0.02f;
			if (SP > maxSP)
				SP = maxSP;
		}
	}

	/// <summary>
	/// Чит для повешения здоровья персонажа
	/// </summary>
	private void HealCheat()
	{
		if (HUD.activeSelf && Input.GetKeyDown(KeyCode.L))
		{
			HP = maxHP;
		}
	}
}