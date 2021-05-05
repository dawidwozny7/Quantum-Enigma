using System.Collections.Generic;

using UnityEngine;

namespace MapTileGridCreator.CubeImplementation
{
	using MapTileGridCreator.Core;

	/// <summary>
	/// Flag for using diagonals in the connexity.
	/// </summary>
	public enum CubeConnexity
	{
		Cube,
		CubeWithDiag
	};

	public class CubeGrid : Grid3D
	{

		#region Inspector
#pragma warning disable 0649

		[SerializeField]
		private CubeConnexity _connexity;

#pragma warning restore 0649
		#endregion

		public override TypeGrid3D GetTypeGrid()
		{
			return TypeGrid3D.Cube;
		}

		public override void Initialize()
		{
			List<Vector3> axes = new List<Vector3>() { Vector3.right, Vector3.up, Vector3.forward };
			if (_connexity == CubeConnexity.CubeWithDiag)
			{
				axes.Add(new Vector3(1, 0, 1));
				axes.Add(new Vector3(1, 0, -1));
				axes.Add(new Vector3(0, 1, 1));
				axes.Add(new Vector3(0, 1, -1));
			}
			Init(axes);
		}
	}
}