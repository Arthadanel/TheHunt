using UnityEngine;

/// <summary>
/// Класс генерации лабиринта.
/// </summary>
public class HuntAndKillAlgorithm
{
	/// <summary>
	/// Текущий ряд.
	/// </summary>
	private int currentRow = 0;
	/// <summary>
	/// Текущий столбец.
	/// </summary>
	private int currentColumn = 0;

	/// <summary>
	/// Завершена ли генерация.
	/// </summary>
	private bool courseComplete = false;

	/// <summary>
	/// Массив ячеек лабиринта.
	/// </summary>
	protected MazeCell[,] mazeCells;
	/// <summary>
	/// Колличество рядав, колонок.
	/// </summary>
	protected int mazeRows, mazeColumns;

	/// <summary>
	/// Конструктор (начальная инициализация параметров).
	/// </summary>
	/// <param name="mazeCells"></param>
	internal HuntAndKillAlgorithm(MazeCell[,] mazeCells)
	{
		this.mazeCells = mazeCells;
		mazeRows = mazeCells.GetLength(0);
		mazeColumns = mazeCells.GetLength(1);
	}

	/// <summary>
	/// Создать лабиринт.
	/// </summary>
	public void CreateMaze()
	{
		HuntAndKill();
	}

	/// <summary>
	/// Алгоритм.
	/// </summary>
	private void HuntAndKill()
	{
		mazeCells[currentRow, currentColumn].Visited = true;

		while (!courseComplete)
		{
			Kill();
			Hunt();
		}
	}

	/// <summary>
	/// Уничтожает данную стену.
	/// </summary>
	/// <param name="wall"></param>
	private void DestroyWallIfItExists(GameObject wall)
	{
		if (wall != null)
		{
			GameObject.Destroy(wall);
		}
	}

	/// <summary>
	/// Есть ли доступные рути из заданной ячейки.
	/// </summary>
	/// <param name="row"></param>
	/// <param name="column"></param>
	/// <returns></returns>
	private bool RouteStillAvailable(int row, int column)
	{
		if (row > 0 && !mazeCells[row - 1, column].Visited)
			return true;

		if (row < mazeRows - 1 && !mazeCells[row + 1, column].Visited)
			return true;

		if (column > 0 && !mazeCells[row, column - 1].Visited)
			return true;

		if (column < mazeColumns - 1 && !mazeCells[row, column + 1].Visited)
			return true;

		return false;
	}

	/// <summary>
	/// Доступна ли ячейка для перехода.
	/// </summary>
	/// <param name="row"></param>
	/// <param name="column"></param>
	/// <returns></returns>
	private bool CellIsAvailable(int row, int column)
	{
		if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells[row, column].Visited)
			return true;
		else
			return false;
	}

	/// <summary>
	/// Проходит произвольную тропу по лабиринту. Остановится, когда зайдет в тупик.
	/// </summary>
	private void Kill()
	{
		while (RouteStillAvailable(currentRow, currentColumn))
		{
			int direction = Random.Range(0, 4);

			if (direction == 0 && CellIsAvailable(currentRow - 1, currentColumn))
			{
				// Север
				DestroyWallIfItExists(mazeCells[currentRow - 1, currentColumn].WallSouth);
				mazeCells[currentRow, currentColumn].N = false;
				mazeCells[currentRow - 1, currentColumn].S = false;
				currentRow--;
			}
			else if (direction == 1 && CellIsAvailable(currentRow, currentColumn + 1))
			{
				// Восток
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].WallEast);
				mazeCells[currentRow, currentColumn].E = false;
				mazeCells[currentRow, currentColumn + 1].W = false;
				currentColumn++;
			}
			else if (direction == 2 && CellIsAvailable(currentRow + 1, currentColumn))
			{
				// Юг
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn].WallSouth);
				mazeCells[currentRow, currentColumn].S = false;
				mazeCells[currentRow + 1, currentColumn].N = false;
				currentRow++;
			}
			else if (direction == 3 && CellIsAvailable(currentRow, currentColumn - 1))
			{
				// Запад
				DestroyWallIfItExists(mazeCells[currentRow, currentColumn - 1].WallEast);
				mazeCells[currentRow, currentColumn].W = false;
				mazeCells[currentRow, currentColumn - 1].E = false;
				currentColumn--;
			}

			mazeCells[currentRow, currentColumn].Visited = true;
		}
	}

	/// <summary>
	/// Находит следующую непосещенную ячейку, стоящую рядом с посещенной. Если таких нет, то значит алгоритм завершен, и полю courseComplete присваевается значение true.
	/// </summary>
	private void Hunt()
	{
		courseComplete = true;

		for (int r = 0; r < mazeRows; r++)
		{
			for (int c = 0; c < mazeColumns; c++)
			{
				if (!mazeCells[r, c].Visited && CellHasAnAdjacentVisitedCell(r, c))
				{
					courseComplete = false;
					currentRow = r;
					currentColumn = c;
					DestroyAdjacentWall(currentRow, currentColumn);
					mazeCells[currentRow, currentColumn].Visited = true;
					return;
				}
			}
		}
	}

	/// <summary>
	/// Проверяет, есть ли рядом с ячейкой посещенная ячейка.
	/// </summary>
	/// <param name="row"></param>
	/// <param name="column"></param>
	/// <returns></returns>
	private bool CellHasAnAdjacentVisitedCell(int row, int column)
	{
		//(north) if on row 1 or greater
		if (row > 0 && mazeCells[row - 1, column].Visited)
			return true;

		//(south) if the second-to-last row (or less)
		if (row < (mazeRows - 2) && mazeCells[row + 1, column].Visited)
			return true;

		//(west) if column 1 or greater
		if (column > 0 && mazeCells[row, column - 1].Visited)
			return true;

		//(east) if the second-to-last column (or less)
		if (column < (mazeColumns - 2) && mazeCells[row, column + 1].Visited)
			return true;

		//true if there are any adjacent visited cells to this one
		return false;
	}

	/// <summary>
	/// Уничтожает сопряженную стену.
	/// </summary>
	/// <param name="row"></param>
	/// <param name="column"></param>
	private void DestroyAdjacentWall(int row, int column)
	{
		bool wallDestroyed = false;

		while (!wallDestroyed)
		{
			int direction = Random.Range(0, 4);

			if (direction == 0 && row > 0 && mazeCells[row - 1, column].Visited)
			{
				DestroyWallIfItExists(mazeCells[row - 1, column].WallSouth);
				wallDestroyed = true;
				mazeCells[row, column].N = false;
				mazeCells[row - 1, column].S = false;
			}
			else if (direction == 3 && column < (mazeColumns - 2) && mazeCells[row, column + 1].Visited)
			{
				DestroyWallIfItExists(mazeCells[row, column].WallEast);
				wallDestroyed = true;
				mazeCells[row, column].E = false;
				mazeCells[row, column + 1].W = false;
			}
			else if (direction == 1 && row < (mazeRows - 2) && mazeCells[row + 1, column].Visited)
			{
				DestroyWallIfItExists(mazeCells[row, column].WallSouth);
				wallDestroyed = true;
				mazeCells[row, column].S = false;
				mazeCells[row + 1, column].N = false;
			}
			else if (direction == 2 && column > 0 && mazeCells[row, column - 1].Visited)
			{
				DestroyWallIfItExists(mazeCells[row, column - 1].WallEast);
				wallDestroyed = true;
				mazeCells[row, column].W = false;
				mazeCells[row, column - 1].E = false;
			}
		}
	}
}