using System.Collections.Generic;

using MapTileGridCreator.Core;
using MapTileGridCreator.Utilities;

using UnityEngine;


namespace MapTileGridCreator.SerializeSystem
{
	/// <summary>
	/// Class use to transfer grid data mapping. Use only inside the SerializeSystem.
	/// </summary>
	[System.Serializable]
	internal class Grid3DDTO
	{
		[SerializeField]
		public TypeGrid3D _type;

		[SerializeField]
		public string _name;

		[SerializeField]
		public float _size_cell = 1;

		[SerializeField]
		public float _gap = 0;

		[SerializeField]
		public List<CellDTO> _map;

		public Grid3DDTO(Grid3D grid)
		{
			_type = grid.GetTypeGrid();
			_name = grid.name;
			_size_cell = grid.SizeCell;
			_gap = grid.GapRatio;

			_map = new List<CellDTO>();
			Dictionary<Vector3Int, Cell>.Enumerator it = grid.GetEnumerator();
			while (it.MoveNext())
			{
				CellDTO dtocell = new CellDTO(it.Current.Value);
				_map.Add(dtocell);
			}
		}

		public Grid3D ToGrid3D()
		{
			Grid3D grid = FuncEditor.InstantiateGrid3D(_type);
			grid.name = _name;
			grid.SizeCell = _size_cell;
			grid.GapRatio = _gap;

			foreach (CellDTO celldto in _map)
			{
				celldto.ToCell(grid);
			}

			return grid;
		}
	}
}
