using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Враг.
/// </summary>
public class Enemy : MonoBehaviour
{
	/// <summary>
	/// Тип врага.
	/// </summary>
	[SerializeField] private string type;
	/// <summary>
	/// Свойство доступа к типу врага.
	/// </summary>
	internal string Type { get { return type; } }

	/// <summary>
	/// Экран атаки.
	/// </summary>
	[SerializeField] private GameObject AttackScreen;
	/// <summary>
	/// Экран защиты.
	/// </summary>
	[SerializeField] private GameObject DefenseScreen;

	/// <summary>
	/// Активен ли враг.
	/// </summary>
	public bool Active { get; set; }

	/// <summary>
	/// Сила физической атаки.
	/// </summary>
	private int atk;
	/// <summary>
	/// Процент защиты.
	/// </summary>
	private float def;
	/// <summary>
	/// Колличество очков здоровья.
	/// </summary>
	private float hp;
	/// <summary>
	/// Количество очков здоровья.
	/// </summary>
	public float HP { get { return hp; } set { hp = value; MazeLoader.mainPlayer.GetComponent<PlayerInfo>().EnemyHP.GetComponent<Slider>().value = value; } }
	/// <summary>
	/// Максимальное количество очков здоровья.
	/// </summary>
	public float maxHP { get; protected set; }
	/// <summary>
	/// Количество очков выносливости.
	/// </summary>
	private float sp;
	/// <summary>
	/// Количество очков выносливости.
	/// </summary>
	public float SP { get { return sp; } set { sp = value; } }
	/// <summary>
	/// Максимальное количество очков выносливости.
	/// </summary>
	public float maxSP { get; protected set; }

	/// <summary>
	/// Задание начальных параметров врага.
	/// </summary>
	private void Start()
	{
		switch (type)
		{
			case "Knight":
				maxHP = Random.Range(30, 41);
				maxSP = Random.Range(2, 3);
				atk = Random.Range(11, 21);
				def = 0.7f;
				break;
			case "Eagle":
				maxHP = Random.Range(15, 26);
				maxSP = Random.Range(2, 8);
				atk = Random.Range(4, 12);
				def = 0.3f;
				break;
			case "Chimera":
				maxHP = Random.Range(30, 75);
				maxSP = Random.Range(2, 10);
				atk = Random.Range(21, 36);
				def = 0.4f;
				break;
			case "Spiders":
				maxHP = Random.Range(80, 250);
				maxSP = Random.Range(16, 26);
				atk = Random.Range(4, 7);
				def = 0.1f;
				break;
			case "Mage":
				maxHP = Random.Range(15, 26);
				maxSP = Random.Range(20, 40);
				atk = Random.Range(1, 5);
				def = 0.1f;
				break;
			case "Owl":
				maxHP = Random.Range(33, 96);
				maxSP = Random.Range(6, 7);
				atk = Random.Range(50, 100);
				def = 0.3f;
				break;
			case "Sphynx":
				maxHP = Random.Range(90, 200);
				maxSP = Random.Range(4, 7);
				atk = Random.Range(30, 55);
				def = 0.3f;
				break;
			case "Sage":
				maxHP = Random.Range(15, 26);
				maxSP = Random.Range(30, 40);
				atk = Random.Range(9, 11);
				def = 1f;
				break;
			case "Wolf":
				maxHP = Random.Range(90, 160);
				maxSP = Random.Range(3, 5);
				atk = Random.Range(15, 37);
				def = 0.2f;
				break;
			case "Guard":
				maxHP = Random.Range(100, 200);
				maxSP = Random.Range(3, 4);
				atk = Random.Range(30, 40);
				def = 0.8f;
				break;
			case "Boss":
				maxHP = Random.Range(700, 1000);
				maxSP = Random.Range(5, 12);
				atk = Random.Range(30, 51);
				def = 0.7f;
				break;
		}
		HP = maxHP;
		SP = maxSP;
	}

	/// <summary>
	/// Помещение врага в лабиринт.
	/// </summary>
	/// <param name="row"></param>
	/// <param name="column"></param>
	/// <param name="enemyPrefab"></param>
	public static void Place(int row, int column, GameObject enemyPrefab)
	{
		if (MazeLoader.MazeCells[row, column].N && MazeLoader.MazeCells[row, column].S)
		{
			enemyPrefab = Instantiate(enemyPrefab, new Vector3(row * MazeLoader.Size, -1.5f, column * MazeLoader.Size), Quaternion.identity);
		}
		if (MazeLoader.MazeCells[row, column].E && MazeLoader.MazeCells[row, column].W)
		{
			enemyPrefab = Instantiate(enemyPrefab, new Vector3(row * MazeLoader.Size, -1.5f, column * MazeLoader.Size), Quaternion.identity);
			enemyPrefab.transform.Rotate(Vector3.up * 90);
		}
		enemyPrefab.name = "Enemy";
	}

	/// <summary>
	/// Защищается ли враг.
	/// </summary>
	internal bool OnGuard;
	/// <summary>
	/// Уровень защиты.
	/// </summary>
	internal float Def { get { return def; } }

	/// <summary>
	/// Достаточно ли выносливости для атаки.
	/// </summary>
	/// <returns></returns>
	private bool EnoghSP()
	{
		switch (type)
		{
			case "Knight": return sp >= 2;
			case "Eagle": return sp >= 1;
			case "Chimera": return sp >= 2.5f;
			case "Spiders": return sp >= 1.5f;
			case "Mage": return sp >= 1f;
			case "Owl": return sp >= 6f;
			case "Sphynx": return sp >= 4f;
			case "Sage": return sp >= 1.5f;
			case "Wolf": return sp >= 2f;
			case "Guard": return sp >= 2.5f;
			case "Boss": return sp >= 3f;
			default: return sp >= 2;
		}
	}

	/// <summary>
	/// Процесс сражения с игроком.
	/// </summary>
	/// <param name="target"></param>
	/// <param name="targetDefending"></param>
	internal void Engage(PlayerInfo target, bool targetDefending = false)
	{
		Animation attack = AttackScreen.GetComponent<Animation>();
		Animation defense = DefenseScreen.GetComponent<Animation>();
		if (EnoghSP() && !defense.isPlaying && !attack.isPlaying && Random.Range(0, 100) > 70)
		{
			DefenseScreen.SetActive(false);
			AttackScreen.SetActive(true);
			attack.Play();
			if (target.Notices.activeSelf && target.Bracers != "None")
				target.BracersDurability--;
			if (targetDefending && target.SP + target.ExtraSP >= 1)
			{
				Animation pDefAnim = target.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
				pDefAnim.Play("PlayerDefense");
				if (target.Cooperating && (type == "Chimera" || type == "Spiders"))
					target.HP -= atk / 2 * (1 - target.Def);
				else
					target.HP -= atk * (1 - target.Def);
				for (int i = 0; i < 1; i++)
				{
					if (target.ExtraSP > 0)
						target.ExtraSP--;
					else
						target.SP--;
				}
			}
			else
				target.HP -= atk;
			SP -= 2;
			OnGuard = false;
		}
		else if (type != "Mimic" && type != "Eagle" && type != "Spiders" && type != "Owl" && EnoghSP() && !attack.isPlaying && !defense.isPlaying && Random.Range(0, 100) < 15)
		{
			AttackScreen.SetActive(false);
			DefenseScreen.SetActive(true);
			defense.Play();
			if (target.Notices.activeSelf && target.Bracers != "None")
				target.BracersDurability--;
			SP -= 1;
			OnGuard = true;
		}
		else if (!attack.isPlaying && !defense.isPlaying)
		{
			if (Random.Range(1, 100) % 16 == 0 && SP < maxSP)
				SP += Random.Range(0.2f, 1f);
			if (SP > maxSP) SP = maxSP;
			AttackScreen.SetActive(false);
			DefenseScreen.SetActive(false);
			OnGuard = false;
		}
		if (defense.isPlaying)
			OnGuard = true;
	}
}
