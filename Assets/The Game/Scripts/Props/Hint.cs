using UnityEngine;

/// <summary>
/// Класс табличек подсказками (записками). Отвечает за содержимое записки и ее корректное расположение на уровне.
/// </summary>
public class Hint : MonoBehaviour
{
	/// <summary>
	/// Текст записки.
	/// </summary>
	private string text;
	/// <summary>
	/// Свойство доступа к тексту.
	/// </summary>
	public string Text { get { return text; } }

	/// <summary>
	/// Запускается при появлении объекта на сцене. Инициализирует поле с текстом записки.
	/// </summary>
	private void Start()
	{
		text = PlayerPrefs.GetString("language") == "English" ? GenerateText() : GenerateTextRus();
	}

	/// <summary>
	/// Генерирует текст записки на английском языке.
	/// </summary>
	/// <returns></returns>
	private string GenerateText()
	{
		string[][] hints =
		{
			new string[] //Honor
			{
				"Knights are... honorable warriors. They like to give a chance even to the weakest, but they also tend to underestimate their opponents. It is what often brings their demise.",
				"This is the highest layer of the labirinth. Enemies are not aware of your strength and potential yet, since you haven't proved yourself battle, so your opponents are quite easily defeated. But the deeper you go, the harder it is to triumph.",
				"Each floor presents you with a goal. You won't know it at the beginning, and may still be unaware of it by the end. You should be able to figure it out by gathering knowledge from those plates.",
				"Eagles, proud birds that help the Knights in  battles. They will keep merclessly attacking you untill either one of you perishes. But after years of workin with the Knights, they were bound to catch some of their fighting habbits.",
				"In order to leave this layer, you must first prove your strength. As to how...",
				"...just keep killing enemies that stand in your way. Sooner or later, you will be able to leave this place. Probably.",
				"If you are lost and unsure of what to do, try to start anew. Maybe you will find something different."
			},
			new string[] //Unity
			{
				"Hunters help each other. They sometimes even cooperate with members of other fractions. You can't really do much all allone, can you?",
				"It is always harder to defeat a united force.",
				"Spiders. Annoying little things. They won't deal you much damage, but when ther's lots of them, they can become a real problem, so you better kill them as fast as possible.",
				"Chimeras... Very strength creatures. Hard to kill, and overall very dangerous. Especially if you fight alone.",
				"In those dangerous times of chaos, you should take whatever help you can get."
			},
			new string[] //Naivety
			{
				"What drives people to help you? Why shouldn't they just betray you on the spot?",
				"Allways ally with people when you get the chance, but never fully trust them.",
				"Is everything really what it seems to be? You have gone through two layers of this labyrinth, surely you know how everything works here already.",
				"Confidence is not the best quality for a hunter. It makes you lose your focus. You just keep doing the same meaningless actions without giving them a second thought.",
				"Hunters are notorious deceivers. After all, they often must gather intell on their targets, and sincerity is not always the best way to get it."
			},
			new string[] //Truth
			{
				"Do you truly understand what does it mean to become a Hunter, join the Hunt?",
				"Many attempt this trial because they believe in Hunters' cause. Are you one of them?",
				"Maybe you wish to become stronger? What for, though? Is it worth it?",
				"You can't turn back now. Couldn't since you first entered this place.",
				"You have very few options now:",
				"Become a full-fledged hunter, devoted to the cause...",
				"...or keep roaming these corridors forever.",
				"The Hunt... is not for everyone. There is no place for righteous heroes, but selfishness isn't welcome, either. You must be willing to follow orders without questions. It can be anything: from hunting an animal or capturing a criminal to slaughtering a village full of seemingly innocent people.",
				"No matter what you think of Hunters, you must always remember one thing:\nThere are no true innocents in this chaotic world. And Hunters are those in charge of keeping things in at least some semblance of order.",
				"The moment you leave the Labyrynth, you will have only one life, one chance. Get acquainted with the filling of Death while you still can. Remember it, and, in the future, avoid it at all costs. It easier to prevent something you are familiar with, don't you think?",
				"Owls are majestic, yet terrifying creatures.",
				//"Sphynxes are terrifying.",
				"Sages are... interesting. They don't have much power, but they are desperate in their attempts to guard knowledge and prevent intruders from progressing farther"
			},
			new string[] //Exasperation
			{
				"Wolfs are very devoted creatures. Kind of like hunters, don't you think?",
				"Guards won't let anyone unworthy pass to the deepest layer. They honor their duty and are fully devoted to their cause.",
				"Doesn't this place remind you of the first layer? Same thing, but your opponents are a bit more merciless."
			},
			new string[] //Resolve
			{
				"It's quite empty in here, don't you think?",
				"No enemies?",
				"Oh, remember the layer with spiders? Wasn't it fun?"
			},
		};
		return hints[PlayerPrefs.GetInt("chapter_num") - 1][Random.Range(0, hints[PlayerPrefs.GetInt("chapter_num") - 1].Length)];
	}

	/// <summary>
	/// Генерирует текст записки на русском языке.
	/// </summary>
	/// <returns></returns>
	private string GenerateTextRus()
	{
		string[][] hints =
		{
			new string[] //Honor
			{
				"Рыцари крайне... благородные воины. Они предпочитают давать шанс даже слабейшим, но также довольно часто недооценивают своих врагов. Это нередко приводит к их скорой смерти.",
				"Это самый верхний уровень Лабиринта. Враги еще не осознали твоей силы и потенциала, так как ты еще не проявил себя в битвах, так что твоих противников довольно несложно победить. Однако чем глубже ты спукаешься, тем сложнее продвигаться вперед.",
				"На каждом уровне у тебя есть цель, которую будет необходимо достигнуть, прежде чем ты сможешь продвинуться дальше. Ты не будешь знать ее в начале, и можешь до сих пор не представлять, что от тебя требуется по достижении выхода с уровня. Однако должно быть несложно ее понять прочтя всю информацию с вот таких табличек.",
				"Орлы - гордые птицы, верные спутники Рыцарей, помогающие им в битвах. Они продолжат безжалостно атаковать тебя пока один из вас не погибнет. Но спустя годы работы с Рыцарями Орлы не могли не позаимствовать у них пару боевых привычек...",
				"Чтобы покинуть этот уровень, ты должен доказать свою силу. А вот как это сделать...",
				"...просто продолжай убивать врагов, стоящих на твоем пути. Рано или поздно, ты сможешь покинуть это место. Или нет.",
				"Если ты растерян и не знаешь, что делать дальше, попробуй начать сначала. Возможно ты откроешь для себя что-то новое."
			},
			new string[] //Unity
			{
				"Охотники помогают друг другу. Порой они даже сотрудничают с членами других фракций. Довольно сложно перевернуть мир в одиночку, не так ли?",
				"Одержать победу над врагами, объединившими силу, всегда сложнее, чем победить их поодиночке.",
				"Пауки. Мелкие, раздражающие твари. Они не наносят много урона, но когда их собирается множество, они могут стать серьезной проблемой, так что их лучше убивать как можно быстрее.",
				"Химеры... Крайне странные существа. Их сложно убить, и в целом они очень опасны. Особенно, если ты сражаешься в одиночку.",
				"В эти опасные, полные хаоса времена, никогда не следует отказываться от помощи."
			},
			new string[] //Naivety
			{
				"Почему кто-то решает тебе помочь? Как думаешь, почему бы не предать тебя при первой удобной возможности?",
				"Объединяйся с другими при первой же возможности, но никогда не следует всецело доверять даже своим союзникам.",
				"Действительно ли все так, как тебе кажется? Ты прошел уже два уровня лабиринта, наверняка ты уже прекрасно знаешь, как здесь все работает.",
				"Самоуверенность не лучшее качество для охотника. Из-за нее крайне легко потерять концентрацию и перестать видеть, что происходит вокруг. Ты просто повторяешь одни и те же бессмысленные и монотонные действия, даже не задумываясь о том, действительно ли стоит так поступать.",
				"Охотники должны быть сведущи в искусстве обмана. В конце концов, им часто необходимо собирать информацию об их целях, а напрямую задавать интересующие вопросы далеко не всегда лучший вариант получить информацию."
			},
			new string[] //Truth
			{
				"Действительно ли ты понимаешь, что значит стать охотником, присоединиться к Охоте?",
				"Многие беруться за это Испытание, так как они верят в дело Охотников. Является ли ты одним из таких людей?",
				"Возможно ты хочешь стать сильнее? Но для чего? Стоит ли оно всего этого?",
				"У тебя нет пути назад. Не было с тех пор, как ты впервые зашел в это место.",
				"Твой выбор сейчас не велик (да и выбор ли это):",
				"Стань полноценным охотником и посвяти себя исполнению долга охотника...",
				"...или продолжи блуждать по этим коридорам до скончания веков.",
				"Охота... она не для каждого. Здесь нет места ни доблестным героям, ни эгоистам, не думающим об общественном благе. Ты должен быть готов без вопросов следовать приказам. Твое задание может быть любым: от выслеживания какого-либо животного до вырезания деревни полной на первый взгляд ни в чем не повинных людей.",
				"Не важно какого ты мнения об Охотниках, ты всегда должен помнить одну вещь:\nВ этом погруженном в хаос мире не существует полностью невинных людей. И фракция Охотников несет ответственность за установление хоть какого-то примитивного порядка.",
				"Как только ты покинешь стены Лабиринта, у тебя останется лишь один шанс, одна жизнь. Здесь же ты условно бессмертен. Привыкни к чувтсву неизбежной гибели, познай агонию смерти. Запомни эти ощущения и сделай все, чтобы избежать их в будущем. Ведь намного проще избежать чего-то, с чем ты до боли хорошо знаком.",
				"Совы - великолепные, но жуткие существа...",
				//"Сфинксы - ужасающие существа...",
				"Мудрецы... интересны. Они довольно слабы, но все равно предпринимают отчаянные попытки скрыть знание от вторженцев, не позволить им пройти дальше."
			},
			new string[] //Exasperation
			{
				"Волки очень преданные существа. Это делает их чем-то похожими на охотников...",
				"Стражи не позволят недостойным пройти на самый нижний уровень. Они ценят свой долг и целиком посвящают себя его исполнению.",
				"Тебе не кажется, что этот уровень чем-то похож на первый? Все то же, только враги немного сильнее..."
			},
			new string[] //Resolve
			{
				"Здесь довольно пусто, не думаешь?",
				"Нет врагов?",
				"Эй, помнишь тот уровень с пауками? Вот там было весесло!"
			},
		};
		return hints[PlayerPrefs.GetInt("chapter_num") - 1][Random.Range(0, hints[PlayerPrefs.GetInt("chapter_num") - 1].Length)];
	}

	/// <summary>
	/// Ставит записку в конкретной ячейке лабиринта.
	/// </summary>
	/// <param name="row">Номер ряда ячейки.</param>
	/// <param name="column">Номер колонки ячейки.</param>
	/// <param name="hintPrefab">Префаб записки.</param>
	public static void Place(int row, int column, GameObject hintPrefab)
	{
		float adjust = MazeLoader.Size / 24f;
		if (MazeLoader.MazeCells[row, column].N && row < GameObject.Find("Game Manager").GetComponent<MazeLoader>().MazeRows)
		{
			hintPrefab = Instantiate(hintPrefab, new Vector3((row - 0.5f) * MazeLoader.Size + adjust, 0, column * MazeLoader.Size), Quaternion.identity);
			hintPrefab.transform.Rotate(Vector3.up * 180);
		}
		if (MazeLoader.MazeCells[row, column].E && column > 0)
		{
			hintPrefab = Instantiate(hintPrefab, new Vector3(row * MazeLoader.Size, 0, (column + 0.5f) * MazeLoader.Size - adjust), Quaternion.identity);
			hintPrefab.transform.Rotate(Vector3.up * 270);
		}
		if (MazeLoader.MazeCells[row, column].S && row > 0)
		{
			hintPrefab = Instantiate(hintPrefab, new Vector3((row + 0.5f) * MazeLoader.Size - adjust, 0, column * MazeLoader.Size), Quaternion.identity);
		}
		if (MazeLoader.MazeCells[row, column].W && column < GameObject.Find("Game Manager").GetComponent<MazeLoader>().MazeColumns)
		{
			hintPrefab = Instantiate(hintPrefab, new Vector3(row * MazeLoader.Size, 0, (column - 0.5f) * MazeLoader.Size + adjust), Quaternion.identity);
			hintPrefab.transform.Rotate(Vector3.up * 90);
		}
		hintPrefab.name = "Hint";
	}
}
