using System;
using System.Collections.Generic;

using UnityEngine;

namespace MapTileGridCreator.Core
{
	/// <summary>
	/// An abstract class that handle the main logic for any regular grid in 3D space.
	/// Permit to :
	///		-Handle the storage of cell in unity editor and in runtime.
	///		-Manipulation of map datas with contains, add, replace and delete functions for the main features.
	///		-Convert position/grid index automatically.
	///		-Iterate on the neigbours of a given cell.
	/// Inspired from voxel engines logic where a cell is a 3D volume, but it's less efficient for large map and must be optimized after the manipulation from the editor.
	/// </summary>
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public abstract class Grid3D : MonoBehaviour
	{
		#region Inspector
#pragma warning disable 0649

		[SerializeField]
		[Min(0.001f)]
		[Tooltip("Permit to handle and correct the children's size. ")]
		private float _size_cell = 1;

		[SerializeField]
		[Tooltip("Permit to handle and correct the children's space beetween them. ")]
		private float _gap_ratio = 1;

		[SerializeField]
		[Tooltip("Permit to handle and correct the children's default rotation. ")]
		private Vector3 _default_rotation;

#pragma warning restore 0649
		#endregion

		/// <summary>
		/// Awake for auto-initialization in runtime.
		/// </summary>
		private void Awake()
		{
			Initialize();
		}

		/// <summary>
		/// Initialize the grid. Must be implemented by child class to handle axes and connexity of cells.
		/// </summary>
		public abstract void Initialize();

		/// <summary>
		/// Get the type of the grid. Must be implemented in children and add the type to the enum TypeGrid3D, and in the extensions methods associated.
		/// </summary>
		/// <returns> The type of the grid.</returns>
		public abstract TypeGrid3D GetTypeGrid();

		/// <summary>
		/// The initialise base function. Use the Unity standart definition for axe's order. 
		/// That is why the y-axis is the up direction of the grid.
		/// WHEN USED IN CHILDREN, BE CAREFULL FOR AXES DEFINITIONS. 
		/// TO VERIFY :
		///		- Magnitude of the axes
		///		- Their direction
		///		- Their order
		///	It can cause undesirable behaviour when converting Vector3 position to index.
		/// </summary>
		/// <param name="axesConnexity"> The axes that determine the connex neighbours of a cell.</param>
		/// <param name="mirrored">Shortcut to replicate axes in opposite direction for connex axes, otherwise add it yourself to axesConnexity.</param>
		protected void Init(List<Vector3> axesConnexity, bool mirrored = true)
		{
			if (axesConnexity.Count < 3)
			{
				throw new ArgumentException("Not enough axes, must be at least of size 3.");
			}

			_map = new Dictionary<Vector3Int, Cell>();
			_axes = new List<Vector3>();
			_connex_axes = new List<Vector3Int>();

			foreach (Vector3 axe in axesConnexity)
			{
				_axes.Add(axe);
			}

			if (mirrored)
			{
				foreach (Vector3 axe in axesConnexity)
				{
					_axes.Add(-axe);
				}
			}

			foreach (Vector3 axe in _axes)
			{
				Vector3Int indexCanon = Vector3Int.RoundToInt(axe);
				Vector3Int axesGrid = Vector3Int.RoundToInt(GetMatrixGridToLocalPosition().inverse.MultiplyPoint3x4(indexCanon));
				_connex_axes.Add(axesGrid);
			}

			AddChildrenCells();
		}

		/// <summary>
		/// Add the cells child in the hierarchy of transform.
		/// </summary>
		private void AddChildrenCells()
		{
			foreach (Cell c in GetComponentsInChildren<Cell>())
			{
				try
				{
					AddCell(c.GetIndex(), c);
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message);
					GameObject.DestroyImmediate(c.gameObject);
				}
			}
		}

		private void CheckIsInitialised()
		{
			if (_map == null)
			{
				Initialize();
			}
		}

		public Dictionary<Vector3Int, Cell>.Enumerator GetEnumerator()
		{
			CheckIsInitialised();
			return _map.GetEnumerator();
		}

		/// <summary>
		/// Check if the grid contains the cell.
		/// </summary>
		/// <param name="index">The index of the cell to test.</param>
		public bool HaveCell(ref Vector3Int index)
		{
			CheckIsInitialised();
			return TryGetCellByIndex(ref index) != null;
		}

		public Quaternion GetDefaultRotation()
		{
			return Quaternion.Euler(_default_rotation);
		}

		/// <summary>
		/// Get the total number of cells.
		/// </summary>
		/// <returns>0 if empty.</returns>
		public int GetNumberOfCells()
		{
			CheckIsInitialised();
			return _map.Count;
		}

		/// <summary>
		/// Get the maximum valency of a cell, also the number of axes that determine the connexity of a cell.
		/// For exemple, on a 2D square grid the valency is 4 without the diagonals.
		/// </summary>
		public int GetMaximumValency()
		{
			CheckIsInitialised();
			return _connex_axes.Count;
		}

		/// <summary>
		/// Get a cell by index. If it is not registered to the grid, throw an ItemException.
		/// Prefer this m�thode if you are sure the cell exists.
		/// </summary>
		/// <param name="index">The index's cell to get.</param>
		public Cell GetCellByIndex(ref Vector3Int index)
		{
			CheckIsInitialised();
			return _map[index];
		}

		/// <summary>
		/// Get a cell by index. If it is not registered to the grid, return null.
		/// </summary>
		/// <param name="index">The index's cell to get.</param>
		public Cell TryGetCellByIndex(ref Vector3Int index)
		{
			CheckIsInitialised();
			_map.TryGetValue(index, out Cell cell);
			return cell;
		}

		/// <summary>
		/// Get the neighbours of a given cell. It check the possibles neighbours and add it to the returning list if the cell exists.
		/// It's not necessary that the index passed was an existing cell in the grid.
		/// </summary>
		/// <param name="index">The index of which to get cell's neighbours.</param>
		/// <returns>A list of cells that exist in the neighbours of the index given.</returns>
		public List<Cell> GetNeighboursCell(ref Vector3Int index)
		{
			List<Cell> neighbours = new List<Cell>();
			foreach (Vector3Int i in GetConnexAxes())
			{
				Vector3Int newIndex = index + i;
				Cell c = TryGetCellByIndex(ref newIndex);
				if (c != null)
				{
					neighbours.Add(c);
				}
			}
			return neighbours;
		}

		/// <summary>
		/// Get the neighbours of a given cell. It check the possibles neighbours and add it to the returning list if the cell exists.
		/// It's not necessary that the cell passed was an existing cell in the grid.
		/// </summary>
		/// <param name="cell">The cell to check neighbours.</param>
		/// <returns>A list of cells that exist in the neighbours of the cell.</returns>
		public List<Cell> GetNeighboursCell(ref Cell cell)
		{
			Vector3Int index = cell.GetIndex();
			return GetNeighboursCell(ref index);
		}

		/// <summary>
		/// Get the neighbours index of a given cell.
		/// </summary>
		/// <param name="index">The index used to compute neighbours indexes.</param>
		/// <returns>A list of index corresponding to the neighbours index.</returns>
		public List<Vector3Int> GetNeighboursIndex(ref Vector3Int index)
		{
			List<Vector3Int> neighbours = new List<Vector3Int>();
			foreach (Vector3Int i in GetConnexAxes())
			{
				Vector3Int newIndex = index + i;
				neighbours.Add(newIndex);
			}
			return neighbours;
		}

		/// <summary>
		/// Convert a position to an index grid.
		/// </summary>
		/// <param name="position"> The position given in world coordinates.</param>
		/// <returns>The index corresponding.</returns>
		public Vector3Int GetIndexByPosition(ref Vector3 position)
		{
			Vector3 localPosition = position - Origin;
			localPosition /= (_size_cell * _gap_ratio);
			Vector3Int indexCanon = Vector3Int.RoundToInt(localPosition);
			Vector3Int index = Vector3Int.RoundToInt(GetMatrixGridToLocalPosition().inverse.MultiplyPoint3x4(indexCanon));
			return index;
		}

		/// <summary>
		/// Get a cell by position in world coordinates. If it is not registered to the grid, return null.
		/// </summary>
		/// <param name="position">The position supposed of the cell./param>
		public Cell TryGetCellByPosition(ref Vector3 position)
		{
			Vector3Int index = GetIndexByPosition(ref position);
			return TryGetCellByIndex(ref index);
		}

		/// <summary>
		/// Get the local position of the cell.
		/// </summary>
		/// <param name="index">The index supposed of the cell.</param>
		public Vector3 GetLocalPositionCell(ref Vector3Int index)
		{
			Vector3 localPosition = GetMatrixGridToLocalPosition().MultiplyPoint3x4(index);
			return localPosition * (_size_cell * _gap_ratio);
		}

		/// <summary>
		/// Get the position of a cell.
		/// </summary>
		/// <param name="index">The index cell</param>
		public Vector3 GetPositionCell(Vector3Int index)
		{
			return Origin + GetLocalPositionCell(ref index);
		}

		/// <summary>
		/// Convert a postion to a grid position in world coordinates.
		/// </summary>
		/// <param name="position">The position to convert.</param>
		public Vector3 TransformPositionToGridPosition(Vector3 position)
		{
			Vector3Int index = GetIndexByPosition(ref position);
			return GetPositionCell(index);
		}

		/// <summary>
		/// Add a cell at a given index to the grid and initialize it.
		/// </summary>
		/// <param name="index">The index of the cell.</param>
		/// <param name="cell">The cell data to initialize and register.</param>
		public void AddCell(Vector3Int index, Cell cell)
		{
			CheckIsInitialised();
			cell.Initialize(index, this);
			_map.Add(index, cell);
		}

		/// <summary>
		/// Add a cell to the grid and initialize it.
		/// Use the transform component of the cell to get the position.
		/// </summary>
		/// <param name="cell">The cell data to initialize and register.</param>
		public void AddCellByPosition(Cell cell)
		{
			Vector3 pos = cell.transform.position;
			Vector3Int index = GetIndexByPosition(ref pos);
			AddCell(index, cell);
		}

		/// <summary>
		/// Replace a cell at a given index to the grid and initialize it.
		/// Don't destroy the duplicate in the processus and must be handle by the user with the return value.
		/// </summary>
		/// <param name="index">The index of the cell to replace.</param>
		/// <param name="cell">The cell data to initialize and replace.</param>
		/// <returns>The previous cell registered, or null if there was any cell.</returns>
		public Cell ReplaceCell(Vector3Int index, Cell cell)
		{
			CheckIsInitialised();

			if (HaveCell(ref index))
			{
				cell.Initialize(index, this);
				Cell delete = _map[index];
				_map[index] = cell;
				return delete;
			}
			return null;
		}

		/// <summary>
		/// Delete the cell at the given index.
		/// The user must handle the destructiuon of the component and the the gameobject associated. 
		/// </summary>
		/// <param name="index"></param>
		public void DeleteCell(Cell cell)
		{
			if (_map != null)
			{
				_map.Remove(cell.GetIndex());
			}
		}

		/// <summary>
		/// Return the list of axes for connexity in index space.
		/// </summary>
		/// <returns>A list of Vector3Int representing the axes in index coordinates.</returns>
		public List<Vector3Int> GetConnexAxes()
		{
			return _connex_axes;
		}

		/// <summary>
		/// Return the list of axes for connexity in world space.
		/// </summary>
		/// <returns>A list of Vector3 representing the axes in world coordinates.</returns>
		public List<Vector3> GetAxes()
		{
			return _axes;
		}


		/// <summary>
		/// Get a special axe in world coodinates. 
		/// </summary>
		/// <param name="indexAxe">The index of the axe asked</param>
		/// <returns>A Vector3.</returns>
		public Vector3 GetAxe(int indexAxe)
		{
			return _axes[indexAxe];
		}

		/// <summary>
		/// Get the matrix to pass from grid coordinates to local position.
		/// </summary>
		/// <returns>A Matrix4x4.</returns>
		protected Matrix4x4 GetMatrixGridToLocalPosition()
		{
			//Transpose for reading
			return new Matrix4x4(
				new Vector4(_axes[0].x, _axes[0].y, _axes[0].z, 0),
				new Vector4(_axes[1].x, _axes[1].y, _axes[1].z, 0),
				new Vector4(_axes[2].x, _axes[2].y, _axes[2].z, 0),
				new Vector4(0, 0, 0, 1));
		}

		#region Getter/Setter

		public Vector3 Origin => transform.position;

		/// <summary>
		/// The size cell is the true place occupied by a single cell. The total volume for grid cell is the cubed of (SizeCell + 2* Gap)
		/// </summary>
		public float SizeCell { get => _size_cell; set => _size_cell = value; }


		/// <summary>
		/// The Gap is the gap between two cells. The total volume for grid cell is the cubed of (SizeCell + 2* Gap)
		/// </summary>
		public float GapRatio { get => _gap_ratio; set => _gap_ratio = value; }

		#endregion

		#region Private

		private List<Vector3> _axes;
		private List<Vector3Int> _connex_axes;
		private Dictionary<Vector3Int, Cell> _map;

		#endregion
	}
}

