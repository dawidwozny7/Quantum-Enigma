using UnityEditor;

namespace MapTileGridCreator.CustomInpectors
{
	[CustomEditor(typeof(PlayerGridMovement), true)]
	public class CharacterGridInspector : Editor
	{
		private PlayerGridMovement _character;

		private void OnEnable()
		{
			_character = (PlayerGridMovement)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		}

		private void OnSceneGUI()
		{
			using (new Handles.DrawingScope(DebugsColor.character))
			{
				Handles.DrawWireDisc(_character.transform.position, _character.transform.up, _character.transform.localScale.magnitude / 2);
			}
		}
	}
}
