using System;
using System.Collections.Generic;

using UnityEngine;

namespace MapTileGridCreator.Procedural
{
	public static class ProceduralMesh
	{
		/// <summary>
		/// Code from https://answers.unity.com/questions/514293/changing-a-gameobjects-primitive-mesh.html
		/// Permit to get Unity build-in meshs.
		/// Acessed 06/05/2020
		/// </summary>
		#region Unity Primitives

		public static Mesh GetUnityPrimitiveMesh(PrimitiveType primitiveType)
		{
			switch (primitiveType)
			{
				case PrimitiveType.Sphere:
					return GetCachedPrimitiveMesh(ref _unitySphereMesh, primitiveType);
				case PrimitiveType.Capsule:
					return GetCachedPrimitiveMesh(ref _unityCapsuleMesh, primitiveType);
				case PrimitiveType.Cylinder:
					return GetCachedPrimitiveMesh(ref _unityCylinderMesh, primitiveType);
				case PrimitiveType.Cube:
					return GetCachedPrimitiveMesh(ref _unityCubeMesh, primitiveType);
				case PrimitiveType.Plane:
					return GetCachedPrimitiveMesh(ref _unityPlaneMesh, primitiveType);
				case PrimitiveType.Quad:
					return GetCachedPrimitiveMesh(ref _unityQuadMesh, primitiveType);
				default:
					throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null);
			}
		}

		private static Mesh GetCachedPrimitiveMesh(ref Mesh primMesh, PrimitiveType primitiveType)
		{
			if (primMesh == null)
			{
				Debug.Log("Getting Unity Primitive Mesh: " + primitiveType);
				primMesh = Resources.GetBuiltinResource<Mesh>(GetPrimitiveMeshPath(primitiveType));

				if (primMesh == null)
				{
					Debug.LogError("Couldn't load Unity Primitive Mesh: " + primitiveType);
				}
			}

			return primMesh;
		}

		private static string GetPrimitiveMeshPath(PrimitiveType primitiveType)
		{
			switch (primitiveType)
			{
				case PrimitiveType.Sphere:
					return "New-Sphere.fbx";
				case PrimitiveType.Capsule:
					return "New-Capsule.fbx";
				case PrimitiveType.Cylinder:
					return "New-Cylinder.fbx";
				case PrimitiveType.Cube:
					return "Cube.fbx";
				case PrimitiveType.Plane:
					return "New-Plane.fbx";
				case PrimitiveType.Quad:
					return "Quad.fbx";
				default:
					throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null);
			}
		}

		private static Mesh _unityCapsuleMesh = null;
		private static Mesh _unityCubeMesh = null;
		private static Mesh _unityCylinderMesh = null;
		private static Mesh _unityPlaneMesh = null;
		private static Mesh _unitySphereMesh = null;
		private static Mesh _unityQuadMesh = null;

		#endregion

		/// <summary>
		/// Get the cached hewxagonal regular plane. Or if null, construct it.
		/// </summary>
		/// <returns> An hexagonal plane mesh with shared vertices and two side face. </returns>
		public static Mesh GetHexagonPlaneMesh()
		{
			if (_hexagonPlaneMesh == null)
			{
				Mesh hex = new Mesh();

				List<Vector3> vertex = new List<Vector3>();
				List<int> faces = new List<int>();

				/*		   <-1->
				 *		   
				 *		  1	 _	2
				 *		/		  \
				 *	   6	 0	   3
				 *	    \		  /
				 *	  ¦	  5	 _	4	¦	↑
				 *	  ¦				¦	0 (plane with back face)
				 *	  ¦	 12	 _  11	¦	↓
				 *		/		  \
				 *	   13	 7	   10
				 *	    \		  /
				 *		  9	 _	9
				 */
				for (int f = 0; f <= 1; f++)
				{
					//Vertex creation
					FillHexagonVertex(vertex, new Vector3(), Mathf.Sign(Mathf.Pow(-1, f)));

					//Face creation
					faces.Add(6 * f + f);
					faces.Add(6 * (f + 1) + f);
					faces.Add(6 * f + 1 + f);

					for (int i = 1; i < 6; i++)
					{
						faces.Add(6 * f + f);
						faces.Add(6 * f + i + f);
						faces.Add(6 * f + i + 1 + f);
					}
				}
				hex.vertices = vertex.ToArray();
				hex.triangles = faces.ToArray();

				hex.RecalculateNormals();
				hex.RecalculateBounds();
				hex.RecalculateTangents();
				hex.Optimize();
				_hexagonPlaneMesh = hex;
			}

			return _hexagonPlaneMesh;
		}

		/// <summary>
		/// Get the cached hewxagonal regular mesh. Or if null, construct it.
		/// </summary>
		/// <returns> An hexagonal mesh. The veritces are duplicated for flat shading. </returns>
		public static Mesh GetHexagonMesh()
		{
			if (_hexagonMesh == null)
			{
				Mesh hex = new Mesh();

				List<Vector3> vertex = new List<Vector3>();
				List<int> faces = new List<int>();

				//Up and down hexagon face
				/*		   <-1->
				 *		   
				 *		  1	 _	2
				 *		/		  \
				 *	   6	 0	   3
				 *	    \		  /
				 *	  ¦	  5	 _	4	¦	↑
				 *	  ¦				¦	1
				 *	  ¦	 12	 _  11	¦	↓
				 *		/		  \
				 *	   13	 7	   10
				 *	    \		  /
				 *		  9	 _	9
				 */
				for (int f = 0; f <= 1; f++)
				{
					//Vertex creation
					FillHexagonVertex(vertex, new Vector3(0, Mathf.Pow(-1, f) / 2, 0), Mathf.Sign(Mathf.Pow(-1, f)));

					//Face creation
					faces.Add(6 * f + f);
					faces.Add(6 * (f + 1) + f);
					faces.Add(6 * f + 1 + f);

					for (int i = 1; i < 6; i++)
					{
						faces.Add(6 * f + f);
						faces.Add(6 * f + i + f);
						faces.Add(6 * f + i + 1 + f);
					}
				}

				//Borders
				/*	1	_	6	<- Upper triangle
				 *	|	\	|
				 *	12	_	13
				 */
				faces.Add(13);
				faces.Add(1);
				faces.Add(6);

				for (int b = 0; b < 6; b++)
				{
					faces.Add(14 - b - 2);
					faces.Add(b + 2);
					faces.Add(b + 1);
				}

				/*	6	_	5
				 *	|	\	|
				 *	13	_	8
				 *	^
				 *	Bottom triangle
				 */
				faces.Add(8);
				faces.Add(13);
				faces.Add(6);

				for (int b = 8; b < 13; b++)
				{
					faces.Add(b + 1);
					faces.Add(b);
					faces.Add(13 - b);
				}

				//Flat shading with duplicate vertices
				HashSet<int> sharedVert = new HashSet<int>();
				for (int face = 0; face < faces.Count; face += 3)
				{
					for (int i = 0; i < 3; i++)
					{
						int vertInd = faces[i];
						if (sharedVert.Contains(vertInd))
						{
							faces[i] = vertex.Count;
							vertex.Add(vertex[vertInd]);
						}
					}
				}

				hex.vertices = vertex.ToArray();
				hex.triangles = faces.ToArray();

				hex.RecalculateNormals();
				hex.RecalculateBounds();
				hex.RecalculateTangents();
				hex.Optimize();
				_hexagonMesh = hex;
			}

			return _hexagonMesh;
		}

		/// <summary>
		/// Add to the given list hexagon's vertex.
		/// </summary>
		/// <param name="vertex">The list in which add the vertex.</param>
		/// <param name="centerPosition">The center position of the face, used also as a point.</param>
		/// <param name="trigoOrder"> The order of filling, influence the normel direction after so be carefull.</param>
		public static void FillHexagonVertex(List<Vector3> vertex, Vector3 centerPosition, float trigoOrder, float halfsize = 1f)
		{
			/*	Forward direction (0, 0, 1)
			 *		\
			 *		  1	 _	2
			 *		/	\ /	  \
			 *	   6  _	 0	_  3	Index in positive trigonometric order
			 *	    \	/ \	  /
			 *	 	  5	 _	4
			 */
			vertex.Add(centerPosition);
			Vector3 dir = new Vector3(0, 0, 1) * halfsize;
			for (int i = 0; i < 6; i++)
			{
				dir = Quaternion.Euler(0, trigoOrder * 60, 0) * dir;
				vertex.Add(centerPosition + dir);
			}
		}

		private static Mesh _hexagonPlaneMesh = null;
		private static Mesh _hexagonMesh = null;
	}
}
