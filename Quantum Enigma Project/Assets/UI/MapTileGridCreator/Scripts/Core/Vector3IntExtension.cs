using UnityEngine;


namespace MapTileGridCreator.Core
{
	public struct Vector3IntExt
	{
		/// <summary>
		/// Extend Vector3Int with forward property.
		/// </summary>
		public static Vector3Int forward => new Vector3Int(0, 0, 1);

		/// <summary>
		/// Extend Vector3Int with backward property.
		/// </summary>
		public static Vector3Int backward => new Vector3Int(0, 0, -1);
	}
}
