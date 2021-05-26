using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за взаимодействие игрока с объектами.
/// </summary>
public class CheckEnter : MonoBehaviour
{
	/// <summary>
	/// Объект, с которым взаимодействует игрок.
	/// </summary>
	[SerializeField] private GameObject target;

	/// <summary>
	/// Игрок.
	/// </summary>
	private PlayerInfo player;

	/// <summary>
	/// Инициализация полей.
	/// </summary>
	void Start()
	{
		player = MazeLoader.mainPlayer.GetComponent<PlayerInfo>();
	}

	/// <summary>
	/// Находится ли игрок в пределах зоны взаимодействия.
	/// </summary>
	bool entered = false;

	/// <summary>
	/// Процесс взаимодействия.
	/// </summary>
	void Update()
	{
		if (entered)
		{
			Interacting();
		}
	}

	/// <summary>
	/// Продолжительное изменение некоторых характеристик персонажа.
	/// </summary>
	private void FixedUpdate()
	{
		if (entered)
		{
			if (player.Resting)
				if (player.SP < player.maxSP)
					player.SP += 0.025f;
			if (player.SP > player.maxSP) player.SP = player.maxSP;
		}
	}

	/// <summary>
	/// Действия, которые нужно совершить при входе игрока в зону влияния объекта.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		GameObject tip = MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip;
		if (other.gameObject.tag == "Player")
		{
			entered = true;
			switch (target.tag)
			{
				case "Enemy":
					if (!target.GetComponent<Enemy>().Active)
					{
						player.EnemyHP.SetActive(true);
						MazeLoader.mainPlayer.GetComponent<PlayerInfo>().EnemyHP.GetComponent<Slider>().maxValue = target.GetComponent<Enemy>().maxHP;
						player.EnemyHP.GetComponent<Slider>().value = player.EnemyHP.GetComponent<Slider>().maxValue;
						target.GetComponent<Enemy>().Active = true;
						other.GetComponent<PlayerController>().Engaged = true;
						if (target.GetComponent<Enemy>().Type != "Boss")
						{
							Vector3 move = new Vector3(0, transform.parent.localScale.y * 3, 0);
							transform.parent.position += move;
							transform.position -= move;
							Vector3 distanceVec = (target.transform.position - other.transform.position) / (target.transform.position - other.transform.position).magnitude;
							if (Mathf.Abs(distanceVec.x) > 0.7)
							{
								if (distanceVec.x < 0)
									target.transform.Rotate(Vector3.up * 180);
							}
							else
							{
								if (distanceVec.z < 0)
									target.transform.Rotate(Vector3.up * 180);
							}
						}
					}
					break;
				case "Chest":
					if (!target.GetComponent<Chest>().Opened)
					{
						tip.SetActive(true);
						tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Open" : MazeLoader.EngToRus["Open"];
						tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
						tip.transform.GetChild(0).GetComponent<Text>().text = "E";
					}
					break;
				case "Hint":
					tip.SetActive(true);
					tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Read" : MazeLoader.EngToRus["Read"];
					tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
					tip.transform.GetChild(0).GetComponent<Text>().text = "E";
					break;
				case "Door":
					if (!target.GetComponent<Portal>().Open)
					{
						tip.SetActive(true);
						tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Open" : MazeLoader.EngToRus["Open"];
						tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
						tip.transform.GetChild(0).GetComponent<Text>().text = "E";
					}
					break;
				case "NPC":
					NPC npc = target.GetComponent<NPC>();
					if (!player.Cooperating)
					{
						tip.SetActive(true);
						if (!npc.Greeted)
							tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Talk" : MazeLoader.EngToRus["Talk"];
						else if (!npc.Introduced)
							tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Who are you?" : MazeLoader.EngToRus["Who are you?"];
						else if (!player.Cooperating)
							tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Cooperate" : MazeLoader.EngToRus["Cooperate"]; ;
						tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
						tip.transform.GetChild(0).GetComponent<Text>().text = "E";
					}
					break;
				default:
					break;
			}
		}
	}

	/// <summary>
	/// Действия, которые необходимо выполнять, пока персонаж находиться в зоне влияния объекта.
	/// </summary>
	private void Interacting()
	{
		switch (target.tag)
		{
			case "Enemy":
				player.Engage(target.GetComponent<Enemy>());
				Enemy enemy = target.GetComponent<Enemy>();
				if (enemy.HP <= 0)
				{
					if (target.GetComponent<Enemy>().Type == "Boss") BossRoomEnter.BossDefeated = true;
					Destroy(target);
				}
				break;
			case "Chest":
				Chest chest = target.GetComponent<Chest>();
				if (Input.GetKeyDown(KeyCode.E))
				{
					if (chest.Possessed)
					{
						chest.Attack.volume = PlayerPrefs.GetFloat("sound_volume") * PlayerPrefs.GetFloat("master_volume") * 0.9f;
						chest.Attack.Play();
						player.HP *= 0.3f;
						chest.Possessed = false;
					}
					else if (!chest.Opened)
					{
						chest.Open();
					}
					else if (chest.Opened && !chest.Looted)
					{
						chest.Loot();
					}
				}
				else if (chest.Possessed && Input.GetMouseButtonDown(0))
				{
					chest.Scream.volume = PlayerPrefs.GetFloat("sound_volume") * PlayerPrefs.GetFloat("master_volume") * 0.9f;
					chest.Scream.Play();
					player.AttackAnim.Play();
					player.HP *= Random.Range(0.7f, 1f);
					chest.Possessed = false;
				}
				break;
			case "Hint":
				if (Input.GetKeyDown(KeyCode.E))
				{
					if (!player.HintPanel.activeSelf)
					{
						player.HintPanel.SetActive(true);
						player.HintPanel.transform.GetChild(0).GetComponent<Text>().text = target.GetComponent<Hint>().Text;
						player.HintPanel.transform.GetChild(0).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["text"] : MazeLoader.Fonts["text_rus"];
						player.Tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Close" : MazeLoader.EngToRus["Close"];
						player.Tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
					}
					else
					{
						player.HintPanel.SetActive(false);
						player.Tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Read" : MazeLoader.EngToRus["Read"];
						player.Tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
					}
				}
				break;
			case "Door":
				if (Input.GetKeyDown(KeyCode.E))
				{
					if (!target.GetComponent<Portal>().Open)
					{
						if (player.ConditionMet())
						{
							if (player.Cooperating)
							{
								if (!player.HintPanel.activeSelf)
								{
									player.Cooperating = false;
									player.HintPanel.SetActive(true);
									player.HintPanel.transform.GetChild(0).GetComponent<Text>().text = player.CoopNPC.Farewell;
									player.HintPanel.transform.GetChild(0).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["text"] : MazeLoader.Fonts["text_rus"];
									player.NameplateNPC.SetActive(false);
									player.Tip.SetActive(true);
									player.Tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Close" : MazeLoader.EngToRus["Close"];
									player.Tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
								}
							}
							target.GetComponent<Portal>().Open = true;
							(target.transform.GetChild(0).GetComponent<Animation>()).Play();
						}
						else
						{
							player.alert.SetActive(true);
							player.alert.GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Locked" : MazeLoader.EngToRus["Locked"];
							player.alert.GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["title"] : MazeLoader.Fonts["title_rus"];
						}
					}
					else if (player.HintPanel.activeSelf)
					{
						player.HintPanel.SetActive(false);
						player.Tip.SetActive(false);
					}
				}
				break;
			case "NPC":
				NPC npc = target.GetComponent<NPC>();
				if (!npc.Greeted && Input.GetKeyDown(KeyCode.E))
				{
					npc.Greeted = true;
					player.NPCFound = true;
					MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Who are you?" : MazeLoader.EngToRus["Who are you?"];
					MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
					player.HintPanel.SetActive(true);
					player.HintPanel.transform.GetChild(0).GetComponent<Text>().text = npc.Greeting;
					player.HintPanel.transform.GetChild(0).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["text"] : MazeLoader.Fonts["text_rus"];
					npc.Greeted = true;
				}
				else if (!npc.Introduced && Input.GetKeyDown(KeyCode.E))
				{
					MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip.transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetString("language") == "English" ? "Cooperate" : MazeLoader.EngToRus["Cooperate"];
					MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip.transform.GetChild(1).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
					player.HintPanel.SetActive(true);
					player.HintPanel.transform.GetChild(0).GetComponent<Text>().text = npc.Introduction;
					player.HintPanel.transform.GetChild(0).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["text"] : MazeLoader.Fonts["text_rus"];
					npc.Introduced = true;
				}
				else if (Input.GetKeyDown(KeyCode.E))
				{
					player.Cooperating = true;
					player.CoopNPC = npc;
					player.NPCFound = true;
					player.HintPanel.SetActive(false);
					player.NameplateNPC.SetActive(true);
					player.NameplateNPC.transform.GetChild(0).GetComponent<Text>().text = npc.Name;
					player.NameplateNPC.transform.GetChild(0).GetComponent<Text>().font = PlayerPrefs.GetString("language") == "English" ? MazeLoader.Fonts["main"] : MazeLoader.Fonts["main_rus"];
					Vector3 move = new Vector3(0, (transform.parent.localScale.y + 0.1f) * 3f, 0);
					transform.parent.position -= move;
					MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip.SetActive(false);
				}
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Действия, выполняемые при выходе из зоны влияния объекта.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other)
	{
		if (target.tag == "Chest")
			target.GetComponent<Chest>().Looted = true;
		GameObject tip = MazeLoader.mainPlayer.GetComponent<PlayerInfo>().Tip;
		tip.SetActive(false);
		player.alert.SetActive(false);
		player.HintPanel.SetActive(false);
		entered = false;
	}
}
