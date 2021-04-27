using MapTileGridCreator.Core;
using MapTileGridCreator.Utilities;
using UnityEditor;

using UnityEngine;

namespace MapTileGridCreator.CustomInpectors
{
	[CustomEditor(typeof(Cell), true)]
	public class CellInspector : Editor
	{
		private Cell _cell;

		private void OnEnable()
		{
			_cell = (Cell)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (_cell.transform.hasChanged && _cell.GetGridParent() != null)
			{
				Grid3D grid = _cell.GetGridParent();
				_cell.Initialize(_cell.GetIndex(), grid);
			}
			if (GUILayout.Button("Reset transform"))
			{
				_cell.ResetTransform();
			}
			if (GUILayout.Button("Go parent"))
			{
				Selection.SetActiveObjectWithContext(_cell.transform.parent, null);
			}
		}
	}
}