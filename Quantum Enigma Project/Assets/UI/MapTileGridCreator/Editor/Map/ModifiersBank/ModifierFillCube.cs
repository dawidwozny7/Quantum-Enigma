using System.Collections.Generic;

using MapTileGridCreator.Core;
using MapTileGridCreator.Utilities;

using UnityEngine;

namespace MapTileGridCreator.TransformationsBank
{
	using SizeGrid = UnityEngine.Vector3Int;

	[CreateAssetMenu(fileName = "modif_FillCube", menuName = "MapTileGridCreator/Modifiers/FillCube")]
	public class ModifierFillCube : Modifier
	{
		#region Inspector
#pragma warning disable 0649

		[Header("ModifierFillCube")]

		[SerializeField]
		[Tooltip("Size of the cube centered to the start index.")]
		private SizeGrid sizeCube = new SizeGrid(10, 1, 10);

		[SerializeField]
		[Tooltip("The filling prefab.")]
		private GameObject prefab;

#pragma warning restore 0649
		#endregion

		private Queue<Vector3Int> _to_instantiate = new Queue<Vector3Int>();

		protected override void BeforeModify(Grid3D grid, Vector3Int index)
		{
			_to_instantiate.Clear();
		}

		protected override void AfterModify(Grid3D grid)
		{
			foreach (Vector3Int index in _to_instantiate)
			{
				FuncEditor.InstantiateCell(prefab, grid, index);
			}
		}

		public override Queue<Vector3Int> Modify(Grid3D grid, Vector3Int index)
		{
			Queue<Vector3Int> newIndexes = new Queue<SizeGrid>();

			for (int x = -sizeCube.x; x <= sizeCube.x; x++)
			{
				for (int z = -sizeCube.z; z <= sizeCube.z; z++)
				{
					for (int y = 0; y < sizeCube.y; y++)
					{
						Vector3Int new_index = index + new Vector3Int(x, y, z);
						if (grid.TryGetCellByIndex(ref new_index) == null)
						{
							_to_instantiate.Enqueue(new_index);
						}
					}
				}
			}
			return newIndexes;
		}
	}
}
