using MapTileGridCreator.Core;
using MapTileGridCreator.Utilities;

using UnityEngine;

/// <summary>
/// Class that permit to test the gameplay associated with grid.
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody)/*, typeof(PlayerInput)*/)]
public class PlayerGridMovement : MonoBehaviour
{
	#region Inspector
#pragma warning disable 0649

	[SerializeField]
	private Vector3Int _index_grid;

	[SerializeField]
	private Grid3D _grid;

	[SerializeField]
	private LayerMask _ground_layermask;

	[SerializeField]
	private bool _debug_neigbhours;

	[SerializeField]
	[Range(0.1f, 1)]
	private float _speed_move = 0.5f;

#pragma warning restore 0649
	#endregion

	private bool _is_moving;
	private Rigidbody _rb;

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		Vector3 dest = _grid.GetPositionCell(_index_grid);
		_rb.MovePosition(dest);
		_is_moving = false;
	}


	/// <summary>
	/// Check if it's a authorized move :
	///		-Destination is not a cell existing (occupied).
	///		-Destination above an existing cell that have a layer in ground's layermask
	/// </summary>
	/// <param name="futureIndex"> The index to test.</param>
	/// <returns>True if it's ok, false otherwise.</returns>
	private bool CheckIndexMovable(Vector3Int futureIndex)
	{
		Vector3Int groundInd = futureIndex;
		groundInd.y--;
		Cell ground = _grid.TryGetCellByIndex(ref groundInd);
		return _grid.HaveCell(ref groundInd) && !_grid.HaveCell(ref futureIndex) && _ground_layermask.HaveLayer(ground.gameObject.layer);
	}

	/// <summary>
	/// Input function from the new input system.
	/// </summary>
	/// <param name="input"></param>
	//public void OnMove(InputValue input)
	//{
	//	Vector2Int move = Vector2Int.CeilToInt(input.Get<Vector2>());
	//	Vector3Int futureIndex = _index_grid;
	//	Move( move.x, move.y);
	//}


	private void Update()
	{

		if (!_is_moving)
		{
			float h = (Input.GetKeyDown(KeyCode.D) ? 1 : 0) + (Input.GetKeyDown(KeyCode.Q) ? -1 : 0);
			float v = (Input.GetKeyDown(KeyCode.Z) ? 1 : 0) + (Input.GetKeyDown(KeyCode.S) ? -1 : 0);
			if ((h != 0 || v != 0))
			{
				MoveStart(h, v);
			}
		}
		else
		{
			Moving();
		}
	}

	/// <summary>
	/// Take the input and begin the movement if authorized.
	/// </summary>
	/// <param name="h">The horizontal axis.</param>
	/// <param name="v">The vertical axis.</param>
	private void MoveStart(float h, float v)
	{
		Vector3Int futureIndex = _index_grid;
		futureIndex.x += Mathf.RoundToInt(h);
		futureIndex.z += Mathf.RoundToInt(v);
		if (CheckIndexMovable(futureIndex))
		{
			_index_grid = futureIndex;
			_is_moving = true;
		}
	}

	/// <summary>
	/// Place the gameobject to his index grid position with lerping. Must be call from Update.
	/// </summary>
	private void Moving()
	{
		Vector3 dest = _grid.GetPositionCell(_index_grid);
		if (Vector3.Distance(dest, transform.position) > 0.1f)
		{
			Vector3 lerp = Vector3.Lerp(dest, transform.position, _speed_move);
			_rb.MovePosition(lerp);
		}
		else
		{
			_rb.MovePosition(dest);
			_is_moving = false;
		}
	}


#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (_grid != null && _debug_neigbhours)
		{
			Vector3Int index = _index_grid;
			index.y--;
			FuncDebugGizmos.DebugNeigbours(index, _grid, DebugsColor.character);
		}
	}
#endif
}
