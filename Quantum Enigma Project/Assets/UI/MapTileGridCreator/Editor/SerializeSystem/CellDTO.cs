using MapTileGridCreator.Core;
using MapTileGridCreator.Utilities;

using UnityEditor;

using UnityEngine;

namespace MapTileGridCreator.SerializeSystem
{
	/// <summary>
	/// Class use to transfer data map. Use only inside the SerializeSystem. 
	/// Instead of gameobject serialisation, use the prefab linked to save datas,
	/// permit to share the data maps more freely.
	/// </summary>
	[System.Serializable]
	internal class CellDTO
	{
		[SerializeField]
		public string _pathPrefab;

		[SerializeField]
		public Vector3Int _index;

		[SerializeField]
		public Vector3 _localposition;

		[SerializeField]
		public Vector3 _localrotation;

		[SerializeField]
		public Vector3 _localscale;

		public CellDTO(Cell cell)
		{
			GameObject prefab = FuncEditor.GetPrefabFromInstance(cell.gameObject);
			_pathPrefab = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
			_index = cell.GetIndex();
			_localposition = cell.transform.localPosition;
			_localrotation = cell.transform.localRotation.eulerAngles;
			_localscale = cell.transform.localScale;
		}

		public Cell ToCell(Grid3D grid)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_pathPrefab);
			Cell cell = FuncEditor.InstantiateCell(prefab, grid, _index);
			cell.transform.localPosition = _localposition;
			cell.transform.localRotation = Quaternion.Euler(_localrotation);
			cell.transform.localScale = _localscale;
			return cell;
		}
	}
}
