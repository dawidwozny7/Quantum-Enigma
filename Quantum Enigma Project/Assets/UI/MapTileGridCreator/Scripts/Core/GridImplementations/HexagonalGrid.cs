using System;
using System.Collections.Generic;

using MapTileGridCreator.Core;

using UnityEngine;

namespace MapTileGridCreator.HexagonalImplementation
{

	public class HexagonalGrid : Grid3D
	{
		public override TypeGrid3D GetTypeGrid()
		{
			return TypeGrid3D.Hexagonal;
		}

		public override void Initialize()
		{
			List<Vector3> axes = new List<Vector3>()
			{
				new Vector3((float)(Math.Sqrt(3.0)), 0, 0),
				new Vector3(0, 1, 0),
				new Vector3((float)(Math.Sqrt(3.0)/2.0), 0, (float)(3.0/2.0)),
				new Vector3((float)(Math.Sqrt(3.0)/2.0), 0, (float)(-3.0/2.0))
			};
			Init(axes, true);
		}
	}
}