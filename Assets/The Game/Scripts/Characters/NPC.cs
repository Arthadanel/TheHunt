using UnityEngine;

/// <summary>
/// Класс НИП (не играбельного персонажа).
/// </summary>
public class NPC : MonoBehaviour
{
	/// <summary>
	/// Является ли НИП предателем.
	/// </summary>
	[SerializeField] private bool traitor;
	/// <summary>
	/// Поприветсвовал ли НИП персонажа.
	/// </summary>
	internal bool Greeted;
	/// <summary>
	/// Представился ли НИП.
	/// </summary>
	internal bool Introduced;

	/// <summary>
	/// Имя НИПа.
	/// </summary>
	public string Name
	{
		get
		{
			if (PlayerPrefs.GetString("language") == "English")
			{
				if (traitor)
					return "Prod";
				else
					return "Candor";
			}
			else
			{
				if (traitor)
					return "Прод";
				else
					return "Кандор";
			}
		}
	}

	/// <summary>
	/// Преветственная речь НИПа.
	/// </summary>
	public string Greeting
	{
		get
		{
			if (PlayerPrefs.GetString("language") == "English")
			{
				if (traitor)
					return "Hey, watcha doin' in this place? 'Nother one of those hunter recruits? This dangerous place, its no good to wander alone. I can lend ya a hand, if you want.";
				else
					return "Hello there, hunter! Oh, you have yet to become one... Yes, yes, of course. You're still in this place, after all. But, no worries! You will finish this Trial in no time. Why? Well, surely because I, Candor Vacuitas, am going to help you!";
			}
			else
			{
				if (traitor)
					return "Эй, ты что здесь делаешь? Очередной рекрут в охотники? Тут опасно, можешь даже не пытаться пройти дальше в одиночку. Я могу помочь, если хочешь.";
				else
					return "Приветствую, охотник! Ох, ты еще не охотник... Да, да, конечно. Ты же до сих пор здесь, в конце-концов. Но нет места переживаниям! Ты не успеешь и глазом моргнуть, как это испытание закончиться. Почему? Ну конечно же потому, что я, Кандор Вакуитас, помогу тебе!";
			}
		}
	}

	/// <summary>
	/// Представление НИПа.
	/// </summary>
	public string Introduction
	{
		get
		{
			if (PlayerPrefs.GetString("language") == "English")
			{
				if (traitor)
					return "Who am I? Name's Prod, if thats whatch'ya askin'. I'm nothin' special, know my way with a knife, though.";
				else
					return "What?! You have never heard of me?! Preposterous! I am the legendary Lancer, second only to the Warrior, one of the Four Chosen! Surely you've heard of them. Each of the Chosen is considered the most skilled in their respective fraction: the Warrior, the Adventurer, the Healer and, of course, the Hunter.";
			}
			else
			{
				if (traitor)
					return "Кто я такой? Зовут Прод, если ты об этом. Нет никаких особых навыков, хотя обращаться с ножом умею.";
				else
					return "Как?! Ты впервые обо мне слышишь?! Абсурд! Я легендарный Копьеносец, уступаю в навыках только Воину, одному из Четверки Избранных! О них-то ты наверняка слышал. Каждый Избранный считается лучшим в своей фракции: Воин, Исследователь, Целитель и, конечно, Охотник.";
			}
		}
	}

	/// <summary>
	/// Прощание НИПа.
	/// </summary>
	public string Farewell
	{
		get
		{
			if (PlayerPrefs.GetString("language") == "English")
			{
				if (traitor)
					return "Ha, this looks like the door to this elevator thingy... Well, I'm gonna leave you to your own devices then. Was nice working with ya.";
				else
					return "Oh, seems like we've found a passage to the next layer! Wonderful! Now, you can continue your thrilling journey! Unfortunatelly, I must stay on this floor. There still may be some other poor soul in need of my help may on this layer. And you seem quiet cappable with this sword of yours! Mayby you will even be able to become The Hunter! Anyhow, I sincerely wish you good luck!";
			}
			else
			{
				if (traitor)
					return "Ха, это похоже на дверь, ведущую к этому странному лифту... Ну, дальше ты уже сам разбирайся. Неплохо было поработать с тобой.";
				else
					return "О, похоже что мы нашли проход на следующий уровень! Восхитительно! Теперь ты сможешь продолжить свое захватывающее путешествие! К сожалению, я вынужден буду остаться здесь. Здесь могут быть еще бедолаги, которым пригодиться моя помощь. Да и ты очень неплохо умеешь обращаться с мечом, ты наверняка справишься и сам! Кто знает, может ты станешь следующим Избранным Охотником. В любом случае, я от всего сердца желаю тебе удачи!";
			}
		}
	}

	/// <summary>
	/// Действия, выполняемые при предательстве НИПа.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="laugh"></param>
	public static void Betray(PlayerInfo player, AudioSource laugh)
	{
		player.HP -= player.maxHP * 0.15f;
		player.AuraDurability = 0;
		player.SwordDurability = 0;
		player.BracersDurability = 0;
		player.NameplateNPC.SetActive(false);
		player.Cooperating = false;
		laugh.Play();
	}

	/// <summary>
	/// Помещение НИПа в лабиринт в заданную ячейку.
	/// </summary>
	/// <param name="row"></param>
	/// <param name="column"></param>
	/// <param name="npcPrefab"></param>
	public static void Place(int row, int column, GameObject npcPrefab)
	{
		npcPrefab = Instantiate(npcPrefab, new Vector3(row * MazeLoader.Size, 1.5f, column * MazeLoader.Size), Quaternion.identity);
		if (!MazeLoader.MazeCells[row, column].N)
			npcPrefab.transform.Rotate(Vector3.up * 90);
		else if (!MazeLoader.MazeCells[row, column].S)
			npcPrefab.transform.Rotate(Vector3.up * 270);
		else if (!MazeLoader.MazeCells[row, column].E)
			npcPrefab.transform.Rotate(Vector3.up * 180);
		npcPrefab.name = "NPC";
	}
}