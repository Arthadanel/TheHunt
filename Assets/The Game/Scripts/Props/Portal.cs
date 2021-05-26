using UnityEngine;

/// <summary>
/// Класс, отвечающий за двери для прохода в финальную комнату.
/// </summary>
public class Portal : MonoBehaviour
{
	/// <summary>
	/// Открыта ли дверь.
	/// </summary>
	public bool Open { get; set; }

	/// <summary>
	/// Ставит дверь в конкретную ячейку лабиринта.
	/// </summary>
	/// <param name="row">Номер ряда ячейки.</param>
	/// <param name="column">Номер колонки ячейки.</param>
	/// <param name="door">Префаб двери.</param>
	public static void Place(int row, int column, GameObject door)
	{
		Destroy(MazeLoader.MazeCells[row, column].Floor);
		MazeLoader.MazeCells[row, column].Ceiling.SetActive(true);
		if (!MazeLoader.MazeCells[row, column].S)
		{
			door = Instantiate(door, new Vector3(row * MazeLoader.Size + (MazeLoader.Size / 2f), 0, (column * MazeLoader.Size)), Quaternion.identity);
			MazeLoader.MazeCells[row, column].S = true;
		}
		else if (!MazeLoader.MazeCells[row, column].W)
		{
			door = Instantiate(door, new Vector3(row * MazeLoader.Size, 0, (column * MazeLoader.Size) - (MazeLoader.Size / 2f)), Quaternion.identity);
			door.transform.Rotate(Vector3.up * 90);
			MazeLoader.MazeCells[row, column].W = true;
		}
		else if (!MazeLoader.MazeCells[row, column].N)
		{
			door = Instantiate(door, new Vector3(row * MazeLoader.Size - (MazeLoader.Size / 2f), 0, (column * MazeLoader.Size)), Quaternion.identity);
			door.transform.Rotate(Vector3.up * 180);
			MazeLoader.MazeCells[row, column].N = true;
		}
		else if (!MazeLoader.MazeCells[row, column].E)
		{
			door = Instantiate(door, new Vector3(row * MazeLoader.Size, 0, (column * MazeLoader.Size) + (MazeLoader.Size / 2f)), Quaternion.identity);
			door.transform.Rotate(Vector3.up * 270);
			MazeLoader.MazeCells[row, column].S = true;
		}
		door.name = "Door";
	}
}