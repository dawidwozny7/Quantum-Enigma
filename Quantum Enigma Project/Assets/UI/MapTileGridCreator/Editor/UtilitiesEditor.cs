using System;
using System.Collections.Generic;

using MapTileGridCreator.Core;
using MapTileGridCreator.CubeImplementation;
using MapTileGridCreator.HexagonalImplementation;

using UnityEditor;

using UnityEngine;


namespace MapTileGridCreator.Utilities
{
	/// <summary>
	/// Static class containining utilities functions for editor.
	/// </summary>
	public static class FuncEditor
	{
		/// <summary>
		/// IUnstantiate an empty Grid3D.
		/// </summary>
		/// <param name="typegrid">The type of the grid.</param>
		/// <returns>The grid component associated to the gameobject.</returns>
		public static Grid3D InstantiateGrid3D(TypeGrid3D typegrid)
		{
			GameObject obj;
			Grid3D grid;

			switch (typegrid)
			{
				case TypeGrid3D.Cube:
					{
						obj = new GameObject("CubeGrid");
						grid = obj.AddComponent<CubeGrid>();
					}
					break;
				case TypeGrid3D.Hexagonal:
					{
						obj = new GameObject("HexagonalGrid");
						grid = obj.AddComponent<HexagonalGrid>();
					}

					break;
				default:
					throw new ArgumentException("No type implemented " + typegrid.ToString() + " inherit Grid3D");
			}

			grid.Initialize();
			Undo.RegisterCreatedObjectUndo(grid.gameObject, "Grid created");
			return grid;
		}

		/// <summary>
		/// Refresh a grid. Only work outside playMode.
		/// </summary>
		/// <param name="grid"> The grid to refresh.</param>
		public static void RefreshGrid(Grid3D grid)
		{
			if (!Application.isPlaying)
			{
				grid.Initialize();
			}
		}

		/// <summary>
		/// Debug a plane grid for helping to visualize the cells.
		/// </summary>
		/// <param name="grid">The grid to debug.</param>
		/// <param name="color"> The color of the grid.</param>
		/// <param name="offset_grid_y"> The offset of y position. </param>
		/// <param name="size_grid">The size of the grid.</param>
		public static void DebugGrid(Grid3D grid, Color color, int offset_grid_y = 0, int size_grid = 10)
		{
			if (grid.GetTypeGrid() == TypeGrid3D.Hexagonal)
			{
				DebugHexagonGrid(grid, color, offset_grid_y, size_grid);
			}
			else
			{
				DebugSquareGrid(grid, color, offset_grid_y, size_grid);
			}
		}

		/// <summary>
		/// Debug a square grid. Use this one if the editor performance is limited rather than other grid debug implementation.
		/// </summary>
		/// <param name="grid">The grid to debug.</param>
		/// <param name="color"> The color of the grid.</param>
		/// <param name="offset_grid_y"> The offset of y position. </param>
		/// <param name="size_grid">The size of the grid.</param>
		private static void DebugSquareGrid(Grid3D grid, Color color, int offset_grid_y = 0, int size_grid = 10)
		{
			using (new Handles.DrawingScope(color))
			{
				Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
				Vector3 pos = grid.transform.position;
				float CaseSize = grid.SizeCell * grid.GapRatio;
				pos.y += offset_grid_y - CaseSize / 2.0f;
				for (float z = -size_grid; z < size_grid; z++)
				{
					Handles.DrawLine(pos + new Vector3(-size_grid, 0, z + 0.5f) * CaseSize,
									pos + new Vector3(size_grid, 0, z + 0.5f) * CaseSize);
				}

				for (float x = -size_grid; x < size_grid; x++)
				{
					Handles.DrawLine(pos + new Vector3(x + 0.5f, 0, -size_grid) * CaseSize,
									pos + new Vector3(x + 0.5f, 0, size_grid) * CaseSize);
				}
			}
		}

		/// <summary>
		/// Debug a hexagon grid.
		/// </summary>
		/// <param name="grid">The grid to debug.</param>
		/// <param name="color"> The color of the grid.</param>
		/// <param name="offset_grid_y"> The offset of y position. </param>
		/// <param name="size_grid">The size of the grid.</param>
		private static void DebugHexagonGrid(Grid3D grid, Color color, int offset_grid_y = 0, int size_grid = 10)
		{
			using (new Handles.DrawingScope(color))
			{
				Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
				Vector3 pos = grid.transform.position;
				float CaseSize = grid.SizeCell * grid.GapRatio;
				pos.y += offset_grid_y - CaseSize / 2.0f;

				List<Vector3> axes = grid.GetAxes();
				float angle_xz = Vector3.Angle(axes[2], axes[0]) / 2;

				//Form 
				Vector3[] form = new Vector3[7];
				int p = 0;
				for (int i = 0; i < axes.Count * 3; i += 3)
				{
					if (axes[i % axes.Count].y == 0)
					{
						form[p] = pos + Quaternion.AngleAxis(angle_xz, axes[1]) * (axes[i % axes.Count] * CaseSize / 2.0f);
						p++;
					}
				}
				form[p] = form[0];

				//Grid
				for (int z = -size_grid; z <= size_grid; z++)
				{
					for (int x = -size_grid; x <= size_grid; x++)
					{
						Vector3 cellpos = (axes[0] * x + axes[2] * z) * CaseSize;

						Vector3[] points = new Vector3[7];
						for (int i = 0; i < 7; i++)
						{
							points[i] = cellpos + form[i];
						}
						Handles.DrawPolyLine(points);
					}
				}
			}
		}

		/// <summary>
		/// Instantiate a cell with a prefab as model.
		/// </summary>
		/// <param name="prefab">The prefab to instantiate as a cell.</param>
		/// <param name="grid">The grid the cell will belongs.</param>
		/// <param name="position"> The position of the cell.</param>
		/// <returns>The cell component associated to the gameobject.</returns>
		public static Cell InstantiateCell(GameObject prefab, Grid3D grid, Vector3 position)
		{
			Vector3Int index = grid.GetIndexByPosition(ref position);
			return InstantiateCell(prefab, grid, index);
		}

		/// <summary>
		/// Instantiate a cell with a prefab as model.
		/// </summary>
		/// <param name="prefab">The prefab to instantiate as a cell.</param>
		/// <param name="grid">The grid the cell will belongs.</param>
		/// <param name="index"> The index of the cell.</param>
		/// <returns>The cell component associated to the gameobject.</returns>
		public static Cell InstantiateCell(GameObject prefab, Grid3D grid, Vector3Int index)
		{
			GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab, grid.transform) as GameObject;
			gameObject.name = index.x + "_" + index.y + "_" + index.z + "_" + gameObject.name;

			Cell cell = gameObject.GetComponent<Cell>();
			if (cell == null)
			{
				cell = gameObject.AddComponent<Cell>();
			}
			grid.AddCell(index, cell);
			cell.ResetTransform();
			Undo.RegisterCreatedObjectUndo(cell.gameObject, "Cell created");
			return cell;
		}

		/// <summary>
		/// Replace a cell with an other cell, and delete the old, component and gameobject.
		/// </summary>
		/// <param name="source">The source gameobject, can be a prefab or an existing gameObject.</param>
		/// <param name="grid">The grid of the old cell.</param>
		/// <param name="old">The old cell to replace.</param>
		/// <returns>The cell component of the new gameobject.</returns>
		public static Cell ReplaceCell(GameObject source, Grid3D grid, Cell old)
		{
			Undo.SetCurrentGroupName("Replace Cell");
			int group = Undo.GetCurrentGroup();

			GameObject prefab = source;
			if (IsGameObjectInstancePrefab(source))
			{
				prefab = GetPrefabFromInstance(source);
			}

			GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab, grid.transform) as GameObject;

			Vector3Int index = old.GetIndex();
			gameObject.transform.position = old.transform.position;
			gameObject.name = gameObject.name + "_" + index.x + "_" + index.y + "_" + index.z;

			Cell cell = gameObject.GetComponent<Cell>();
			if (cell == null)
			{
				gameObject.AddComponent<Cell>();
			}
			grid.ReplaceCell(index, cell);
			Undo.RegisterCreatedObjectUndo(cell.gameObject, "Cell replaced");
			Undo.DestroyObjectImmediate(old.gameObject);
			Undo.CollapseUndoOperations(group);
			return cell;
		}

		/// <summary>
		/// Destroy a cell with his gameobject. Do nothing if not instantiated.
		/// </summary>
		/// <param name="cell"></param>
		public static void DestroyCell(Cell cell)
		{
			if (IsGameObjectInstancePrefab(cell.gameObject))
			{
				Undo.DestroyObjectImmediate(cell.gameObject);
			}
		}

		/// <summary>
		/// Stamp cells is to copy and paste a list of cell to an other location.
		/// </summary>
		/// <param name="listCell"> The list of cells to copy.</param>
		/// <param name="grid"> The grid in which place the copy.</param>
		/// <param name="displacement"> The displacement relative to the current position of cells.</param>
		/// <param name="overwrite"> Option if we overwrite an existing cell at destination of copy</param>
		public static void StampCells(List<Cell> listCell, Grid3D grid, Vector3Int destinationIndex, bool overwrite = true)
		{
			Vector3Int displacement = destinationIndex - listCell[0].GetIndex();
			foreach (Cell c in listCell)
			{
				Vector3Int index = displacement + c.GetIndex();
				Cell cdest = grid.TryGetCellByIndex(ref index);

				GameObject prefabInstance = c.gameObject;
				GameObject prefab = GetPrefabFromInstance(prefabInstance);
				if (cdest == null)
				{
					InstantiateCell(prefab, grid, index);
				}
				else if (overwrite)
				{
					ReplaceCell(prefab, grid, cdest);
				}
			}
		}

		/// <summary>
		/// Ui funtions to draw separator in Editors.
		/// </summary>
		/// <param name="color"> The color of the separation</param>
		/// <param name="thickness">The thickness.</param>
		/// <param name="padding"> The padding.</param>
		public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
		{
			Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
			r.height = thickness;
			r.y += padding / 2;
			r.x -= 2;
			r.width += 6;
			EditorGUI.DrawRect(r, color);
		}

		/// <summary>
		/// Test if the given object is an instance of prefab.
		/// </summary>
		public static bool IsGameObjectInstancePrefab(UnityEngine.Object obj)
		{
			PrefabAssetType type = PrefabUtility.GetPrefabAssetType(obj);
			return type == PrefabAssetType.Regular && PrefabUtility.IsPartOfNonAssetPrefabInstance(obj);
		}

		/// <summary>
		/// Test if the given object is an instantiated object in scene view.
		/// </summary>
		public static bool IsGameObjectSceneView(UnityEngine.Object obj)
		{
			PrefabAssetType type = PrefabUtility.GetPrefabAssetType(obj);
			return IsGameObjectInstancePrefab(obj) || type == PrefabAssetType.NotAPrefab;
		}

		/// <summary>
		/// Get the prefab from an instance.
		/// </summary>
		/// <param name="prefabInstance"> The instance of a prefab.</param>
		/// <returns>The prefab, or null if not founded.</returns>
		public static GameObject GetPrefabFromInstance(GameObject prefabInstance)
		{
			if (IsGameObjectInstancePrefab(prefabInstance))
			{
				return PrefabUtility.GetCorrespondingObjectFromSource(prefabInstance);
			}
			return null;
		}
	}

}

