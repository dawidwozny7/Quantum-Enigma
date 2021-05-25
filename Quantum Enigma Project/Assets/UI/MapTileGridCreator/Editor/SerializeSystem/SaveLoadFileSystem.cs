using System.IO;

using MapTileGridCreator.Core;

using UnityEngine;

namespace MapTileGridCreator.SerializeSystem
{
	public static class SaveLoadFileSystem
	{
		/// <summary>
		/// Save a grid to a raw file in JSON.
		/// </summary>
		/// <param name="grid"> The grid to save.</param>
		/// <param name="pathFile"> The file output name.</param>
		public static async void SaveAsyncRawJSON(Grid3D grid, string pathFile)
		{
			using (StreamWriter writer = new StreamWriter(pathFile))
			{
				Grid3DDTO griddto = new Grid3DDTO(grid);
				await writer.WriteAsync(JsonUtility.ToJson(griddto, true));
			}
			Debug.Log("Write " + grid.name + " map to JSON file at " + pathFile);
		}

		/// <summary>
		/// Load a grid save in a raw file JSON.
		/// </summary>
		/// <param name="path">The name of the JSON file containing grid data.</param>
		/// <returns>The grid reconstructed.</returns>
		public static Grid3D LoadRawJSON(string path)
		{
			string content;
			using (StreamReader reader = new StreamReader(path))
			{
				content = reader.ReadToEnd();
			}
			Grid3DDTO griddto = JsonUtility.FromJson<Grid3DDTO>(content);

			Grid3D grid = griddto.ToGrid3D();

			Debug.Log("Load " + grid.name + " map from JSON file at " + path);
			return grid;
		}
	}
}