
using MapTileGridCreator.Core;
using MapTileGridCreator.Utilities;

using UnityEditor;

using UnityEngine;

namespace MapTileGridCreator.CustomInpectors
{
	[CustomEditor(typeof(MapModifier), true)]
	public class MapModifierInspector : Editor
	{
		private MapModifier _map_modif;

		public void OnEnable()
		{
			_map_modif = (MapModifier)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			EditorGUILayout.BeginVertical();
			FuncEditor.DrawUILine(Color.gray, 1);

			foreach (Modifier modif in _map_modif.m_ModifiersList)
			{
				if (modif != null && !modif.Pass)
				{
					GUILayout.Label(modif.name, EditorStyles.boldLabel);

					FuncEditor.DrawUILine(Color.gray, 1);
					CreateEditor(modif).OnInspectorGUI();
				}
				else if (modif != null && modif.Pass)
				{
					GUILayout.Label(modif.name + " is passed.", EditorStyles.boldLabel);
					modif.Pass = EditorGUILayout.Toggle(modif.Pass);
				}
				else
				{
					EditorGUILayout.HelpBox("Null modifier will be ignored.", MessageType.Warning);
				}
				FuncEditor.DrawUILine(Color.gray, 1);
			}
			EditorGUILayout.EndVertical();
		}
	}
}
