using UnityEngine;

/// <summary>
/// Ячейка лабиринта.
/// </summary>
public class MazeCell
{
	/// <summary>
	/// Была ли ячейка посещена во время прохода по ней алгоритмом генерации.
	/// </summary>
	public bool Visited = false;

	/// <summary>
	/// Пол.
	/// </summary>
	public GameObject Floor { get; internal set; }
	/// <summary>
	/// Потолок.
	/// </summary>
	public GameObject Ceiling { get; internal set; }
	/// <summary>
	/// Колонна.
	/// </summary>
	public GameObject Pillar { get; internal set; }
	/// <summary>
	/// Северная стена.
	/// </summary>
	public GameObject WallNorth { get; internal set; }
	/// <summary>
	/// Восточная стена.
	/// </summary>
	public GameObject WallEast { get; internal set; }
	/// <summary>
	/// Южная стена.
	/// </summary>
	public GameObject WallSouth { get; internal set; }
	/// <summary>
	/// Западная стена.
	/// </summary>
	public GameObject WallWest { get; internal set; }

	/// <summary>
	/// Есть ли сундук.
	/// </summary>
	public bool CellChest { get; internal set; }
	/// <summary>
	/// Есть ли враг.
	/// </summary>
	public bool CellEnemy { get; internal set; }
	/// <summary>
	/// Есть ли записка.
	/// </summary>
	public bool CellHint { get; internal set; }
	/// <summary>
	/// Есть ли зелье.
	/// </summary>
	public bool CellBuffs { get; internal set; }
	/// <summary>
	/// Есть ли персонаж.
	/// </summary>
	public bool CellPlayer { get; internal set; }
	/// <summary>
	/// Есть ли дверь в финальную комнату.
	/// </summary>
	public bool BossDoor { get; internal set; }
	/// <summary>
	/// Есть ли НИП.
	/// </summary>
	public bool CellNPC { get; internal set; }

	/// <summary>
	/// Есть ли соответствующие стены в ячейке.
	/// </summary>
	public bool N = true, E = true, S = true, W = true;
	/// <summary>
	/// Была ли удалена южная стена.
	/// </summary>
	internal bool WallSouthEmpty = true;
	/// <summary>
	/// Была ли удалена восточная стена.
	/// </summary>
	internal bool WallEastEmpty = true;
	/// <summary>
	/// Была ли удалена северная стена.
	/// </summary>
	internal bool WallNorthEmpty = true;
	/// <summary>
	/// Была ли удалена западная стена.
	/// </summary>
	internal bool WallWestEmpty = true;

	/// <summary>
	/// Есть ли объект.
	/// </summary>
	/// <returns></returns>
	public bool HasObject()
	{
		bool[] gObjects = { CellChest, CellBuffs, CellEnemy, CellHint, CellNPC, CellPlayer, BossDoor };
		foreach (bool gObj in gObjects)
			if (gObj) return true;
		return false;
	}

	/// <summary>
	/// Считает количество стен в ячейке.
	/// </summary>
	/// <returns>Количество стен.</returns>
	public int Walls()
	{
		int walls = 0;
		if (N)
			walls++;
		if (E)
			walls++;
		if (S)
			walls++;
		if (W)
			walls++;
		return walls;
	}
}