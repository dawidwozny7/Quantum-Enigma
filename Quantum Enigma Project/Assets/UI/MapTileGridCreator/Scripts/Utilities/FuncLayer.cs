using UnityEngine;

namespace MapTileGridCreator.Utilities
{
	public static class FuncLayer
	{
		/// <summary>
		/// LayerMask extension method for testing if a layer is inside the mesk.
		/// </summary>
		/// <param name="layerMask"> The layermask in which to test.</param>
		/// <param name="layer">The layer to test.</param>
		/// <returns>True if the layermask have the layer inside.</returns>
		public static bool HaveLayer(this LayerMask layerMask, int layer)
		{
			return (layerMask == (layerMask | (1 << layer)));
		}
	}
}
