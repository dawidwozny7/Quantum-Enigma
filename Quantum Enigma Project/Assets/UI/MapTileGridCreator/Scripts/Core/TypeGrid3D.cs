using System;

using MapTileGridCreator.CubeImplementation;
using MapTileGridCreator.HexagonalImplementation;


namespace MapTileGridCreator.Core
{
	/// <summary>
	/// An enum for repertoring all tyhe implementations of Grid3D. Permit to specialyse function like debug or other depending on this type.
	/// </summary>
	public enum TypeGrid3D
	{
		Cube,
		Hexagonal
	}

	/// <summary>
	/// The method extension of the enum TypeGrid3D.
	/// </summary>
	public static class TypeGrid3DMethods
	{
		public static string[] GetTypesGrid(this TypeGrid3D typegrid)
		{
			return new string[] { "Cube", "Hexagonal" };
		}

		public static Type GetTypeGrid(this TypeGrid3D typegrid)
		{
			switch (typegrid)
			{
				case TypeGrid3D.Cube:
					return typeof(CubeGrid);
				case TypeGrid3D.Hexagonal:
					return typeof(HexagonalGrid);
				default:
					break;
			}
			throw new ArgumentException("Type is not implemented.");
		}
	}
}
