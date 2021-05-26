using UnityEngine;

/// <summary>
/// Класс, отвечающий за зелья с положительными эффектами.
/// </summary>
public class Buffs : MonoBehaviour
{
	/// <summary>
	/// Тип зелья.
	/// </summary>
	[SerializeField] private string type;

	/// <summary>
	/// Применение эффектов зелья заданного типа на игрока и уничтожение объекта.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			PlayerInfo player = other.GetComponent<PlayerInfo>();
			switch (type)
			{
				case "Heal":
					if (player.HP < player.maxHP)
					{
						player.HP += player.maxHP * 0.2f;
						if (player.HP > player.maxHP)
							player.HP = player.maxHP;
						Destroy(gameObject);
					}
					break;
				case "Stamina":
					if (player.ExtraSP < player.maxExtraSP)
					{
						player.ExtraSP++;
						Destroy(gameObject);
					}
					break;
				case "Magic":
					if (player.MgcCharges < 25)
					{
						player.MgcCharges += 1;
						Destroy(gameObject);
					}
					break;
			}
		}
	}

	/// <summary>
	/// Ставит зелье в заданной ячейке лабиринта.
	/// </summary>
	/// <param name="row">Ряд ячейки.</param>
	/// <param name="column">Столбец ячейки.</param>
	/// <param name="buffPrefab">Префаб зелья.</param>
	public static void Place(int row, int column, GameObject buffPrefab)
	{
		buffPrefab = Instantiate(buffPrefab, new Vector3(row * MazeLoader.Size, MazeLoader.Size / 5f, column * MazeLoader.Size), Quaternion.identity);
		buffPrefab.name = "Buff";
	}
}
