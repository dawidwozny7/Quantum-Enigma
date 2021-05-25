using System;
using System.Collections.Generic;

using MapTileGridCreator.Core;
using MapTileGridCreator.Procedural;

using UnityEditor;

using UnityEngine;


namespace MapTileGridCreator.Utilities
{

#if UNITY_EDITOR
	public class FuncDebugGizmos
	{
		#region Auto-Draw Gizmos

		/// <summary>
		/// Debug empty cells in the scene.
		/// </summary>
		/// <param name="cell"> The cell to test if empty.</param>
		/// <param name="gizmoType">The gizmo type.</param>
		[DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Active)]
		public static void DebugEmptyCell(Cell cell, GizmoType gizmoType)
		{
			if (LayerMask.NameToLayer("EmptyCell") == cell.gameObject.layer)
			{
				DebugCell(cell.GetIndex(), cell.GetGridParent(), DebugsColor.empty_cell, -0.01f);
			}
		}
		#endregion

		#region Funtions Script

		/// <summary>
		/// Gizmo for debuging a cell.
		/// </summary>
		/// <param name="cell">The cell to display.</param>
		/// <param name="color">Color of the gizmo.</param>
		/// <param name="offsetSize"> The scale amount add to the grid size cell.</param>
		public static void DebugCell(Cell cell, Color color, float offsetSize = 0.01f)
		{
			DebugCell(cell.GetIndex(), cell.GetGridParent(), color, offsetSize);
		}


		/// <summary>
		/// Gizmo for debuging a cell.
		/// </summary>
		/// <param name="index">The index of the cell.</param>
		/// <param name="grid">The grid it belongs</param>
		/// <param name="color">Color of the gizmo.</param>
		/// <param name="offsetSize"> The scale amount add to the grid size cell.</param>
		public static void DebugCell(Vector3Int index, Grid3D grid, Color color, float offsetSize = 0.01f)
		{
			if (grid == null)
			{
				return;
			}
			Gizmos.color = color;
			TypeGrid3D typegrid = grid.GetTypeGrid();
			switch (typegrid)
			{
				case TypeGrid3D.Cube:
					{
						Gizmos.DrawWireCube(grid.GetPositionCell(index), Vector3.one * (grid.SizeCell + offsetSize));
					}
					break;
				case TypeGrid3D.Hexagonal:
					{
						Gizmos.DrawMesh(ProceduralMesh.GetHexagonMesh(), grid.GetPositionCell(index), Quaternion.identity, Vector3.one * (grid.SizeCell + offsetSize));
					}
					break;
				default:
					throw new ArgumentException("No type implemented " + typegrid.ToString() + " inherit Grid3D");
			}
		}

		/// <summary>
		/// Gizmo for debuging the neighbours of a cell.
		/// </summary>
		/// <param name="cell">The cell use to compute neighbours.</param>
		/// <param name="color">Color of the gizmo.</param>
		/// <param name="offsetSize"> The scale amount add to the grid size cell.</param>
		public static void DebugNeigbours(Cell cell, Color color, float offsetSize = 0.01f)
		{
			DebugNeigbours(cell.GetIndex(), cell.GetGridParent(), color, offsetSize);
		}

		/// <summary>
		/// Gizmo for debuging the neighbours of a cell.
		/// </summary>
		/// <param name="index">The index of the cell.</param>
		/// <param name="grid">The grid it belongs</param>
		/// <param name="color">Color of the gizmo.</param>
		/// <param name="offsetSize"> The scale amount add to the grid size cell.</param>
		public static void DebugNeigbours(Vector3Int index, Grid3D grid, Color color, float offsetSize = 0.01f)
		{
			DebugCells(grid.GetNeighboursCell(ref index), color, offsetSize);
		}

		/// <summary>
		/// Gizmo for debuging a list of cells.
		/// </summary>
		/// <param name="index">The index of the cell.</param>
		/// <param name="grid">The grid it belongs</param>
		/// <param name="color">Color of the gizmo.</param>
		/// <param name="offsetSize"> The scale amount add to the grid size cell.</param>
		public static void DebugCells(List<Cell> cells, Color color, float offsetSize = 0.01f)
		{
			foreach (Cell cell in cells)
			{
				DebugCell(cell, color, offsetSize);
			}
		}
		#endregion

	}
#endif
}
