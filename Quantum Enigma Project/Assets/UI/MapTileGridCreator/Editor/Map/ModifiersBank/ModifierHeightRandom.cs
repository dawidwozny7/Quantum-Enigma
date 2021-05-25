using System.Collections.Generic;

using MapTileGridCreator.Core;
using MapTileGridCreator.Utilities;

using UnityEngine;

namespace MapTileGridCreator.TransformationsBank
{

	[CreateAssetMenu(fileName = "modif_HeightRandom", menuName = "MapTileGridCreator/Modifiers/HeightRandom")]
	public class ModifierHeightRandom : Modifier
	{
		#region Inspector
#pragma warning disable 0649

		[Header("ModifierHeightRandom")]

		//TODO Distribution curve
		[SerializeField]
		[Min(0)]
		private int Min_Random;

		[SerializeField]
		[Min(0)]
		private int Max_Random;

#pragma warning restore 0649
		#endregion

		public override Queue<Vector3Int> Modify(Grid3D grid, Vector3Int index)
		{
			Cell root;
			if ((root = grid.TryGetCellByIndex(ref index)) == null)
			{
				return null;
			}

			Queue<Vector3Int> newIndexes = new Queue<Vector3Int>();
			List<Cell> neighb = grid.GetNeighboursCell(ref index);
			foreach (Cell cell in neighb)
			{
				newIndexes.Enqueue(cell.GetIndex());
			}

			//Modif
			Vector3Int upIndex = root.GetIndex() + grid.GetConnexAxes()[1];
			if (!grid.HaveCell(ref upIndex))
			{
				int height = Random.Range(Min_Random, Max_Random);
				GameObject prefab = FuncEditor.GetPrefabFromInstance(root.gameObject);
				for (int i = 0; i < height; i++)
				{
					if (!grid.HaveCell(ref upIndex))
					{
						FuncEditor.InstantiateCell(prefab, grid, upIndex);
					}
					upIndex += grid.GetConnexAxes()[1];
				}
			}
			return newIndexes;
		}
	}
}
