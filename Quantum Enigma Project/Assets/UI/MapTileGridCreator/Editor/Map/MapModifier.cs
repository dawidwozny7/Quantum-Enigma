using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace MapTileGridCreator.Core
{

	public abstract class Modifier : ScriptableObject
	{
		#region Inspector
#pragma warning disable 0649

		[Header("Basics settings")]

		[SerializeField]
		[Tooltip("If true the modifier will be passed.")]
		private bool m_Pass = false;

		[SerializeField]
		[Min(0)]
		[Tooltip("The maximum number of nodes that will be handled, use it for a finish condition, or at least a security.")]
		private int m_MaxNodes = 10000;

		[SerializeField]
		[Tooltip("The start index of grid iteration. UsePreviousOrDefaultIndex overwrite it if option activate.")]
		private Vector3Int m_StartIndex;

		[Header("Map modifier options")]

		[SerializeField]
		[Min(0)]
		[Tooltip("Parameter used by MapModifier, else no effect. The number of repetition for a given modifier.")]
		private int m_NumberOfIterations = 1;

		[SerializeField]
		[Tooltip("Parameter used by MapModifier, else no effect. Permit to use the last index of the precedent modifier or repetition.")]
		private bool m_UsePreviousOrDefaultIndex = false;

		[SerializeField]
		[Tooltip("WATCHOUT, security for infinite loop in grid iteration. Permit to visit a cell only once. " +
			"Always set it at false if not sure.")]
		private bool m_AllowCircular = false;


#pragma warning restore 0649
		#endregion

		public Vector3Int StartIndex => m_StartIndex;
		public bool Pass { get => m_Pass; set => m_Pass = value; }
		public int NumberOfIterations { get => m_NumberOfIterations; set => m_NumberOfIterations = value; }

		/// <summary>
		/// Main iteration function. Permit to handle the modification.
		/// </summary>
		/// <param name="gridmap"> The grid to apply the modifier.</param>
		/// <param name="previousIndex"> The index given by the caller. With m_UsePreviousOrDefaultIndex, permit to overwrite the start index.</param>
		/// <returns> The last index used or previous if it was passed.</returns>
		public Vector3Int ApplyModifier(Grid3D gridmap, Vector3Int previousIndex)
		{
			if (!m_Pass)
			{
				//Initialisation

				Queue<Vector3Int> queue = new Queue<Vector3Int>();
				HashSet<Vector3Int> queueSet = new HashSet<Vector3Int>();
				HashSet<Vector3Int> workingSet = new HashSet<Vector3Int>();

				nb_iterations = 0;
				Vector3Int index;

				if (m_UsePreviousOrDefaultIndex)
				{
					index = previousIndex;
				}
				else
				{
					index = StartIndex;
				}
				last_index = index;

				BeforeModify(gridmap, previousIndex);

				queue.Enqueue(last_index);

				//Loop
				while (queue.Count > 0 && nb_iterations < m_MaxNodes)
				{
					EditorUtility.DisplayProgressBar("Apply modifier " + name, "Number of nodes treated : " + nb_iterations, nb_iterations / (1.0f * queue.Count + nb_iterations));
					index = queue.Dequeue();
					queueSet.Remove(index);
					Queue<Vector3Int> new_indexes = Modify(gridmap, index);
					workingSet.Add(index);

					if (new_indexes != null)
					{
						foreach (Vector3Int indexNext in new_indexes)
						{
							if (!m_AllowCircular && (workingSet.Contains(indexNext) || queueSet.Contains(indexNext)))
							{
								continue;
							}

							queueSet.Add(indexNext);
							queue.Enqueue(indexNext);
						}
					}

					last_index = index;
					nb_iterations++;
				}

				if (nb_iterations >= m_MaxNodes)
				{
					Debug.LogError("Not all map havebeen computed. Make a higher number of iteration or be carefull at circular case.");
				}

				EditorUtility.DisplayProgressBar("Apply modifier " + name, "Number of nodes treated : " + nb_iterations, 0.95f);
				AfterModify(gridmap);

				EditorUtility.ClearProgressBar();
				return last_index;
			}

			return previousIndex;
		}

		/// <summary>
		/// Permit to do some extra initialisation befor grid initialisation.
		/// </summary>
		/// <param name="grid">The grid that will be modified.</param>
		/// <param name="previousIndex"> The previous index given by the caller of modify.</param>
		protected virtual void BeforeModify(Grid3D grid, Vector3Int previousIndex) { }

		/// <summary>
		/// Function to be implemented in order to do various modifications.
		/// </summary>
		/// <param name="grid">The grid that is modified.</param>
		/// <param name="index">The current index of grid itiration.</param>
		/// <returns> 
		/// The next index that will be considered for the iteration by the ApplyModifier function.
		/// See the documentation of this function for more details.
		/// </returns>
		public abstract Queue<Vector3Int> Modify(Grid3D grid, Vector3Int index);

		/// <summary>
		/// Permit to do some extra finish after the grid iteration.
		/// </summary>
		protected virtual void AfterModify(Grid3D grid) { }

		protected Vector3Int last_index;
		protected int nb_iterations;

	}

	[CreateAssetMenu(fileName = "mapModifier", menuName = "MapTileGridCreator/MapModifier", order = 1)]
	public class MapModifier : ScriptableObject
	{
		#region Inspector
#pragma warning disable 0649

		[SerializeField]
		public List<Modifier> m_ModifiersList = new List<Modifier>();

#pragma warning restore 0649
		#endregion

		/// <summary>
		/// Apply the list of modifiers registered to a grid.
		/// </summary>
		/// <param name="grid">The grid to apply it.</param>
		public void ApplyModifiers(Grid3D grid)
		{
			Vector3Int last_Index = _default_index;
			foreach (Modifier tr in m_ModifiersList)
			{
				Undo.IncrementCurrentGroup();
				Undo.SetCurrentGroupName("MapModifier apply " + tr.name);
				int group = Undo.GetCurrentGroup();

				for (int i = 0; i < tr.NumberOfIterations; i++)
				{
					last_Index = tr.ApplyModifier(grid, last_Index);
				}

				Undo.CollapseUndoOperations(group);
			}
		}

		private Vector3Int _default_index = new Vector3Int(0, 0, 0);
	}
}