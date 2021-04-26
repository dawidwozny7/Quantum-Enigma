using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHover : MonoBehaviour
{
	public GameObject cursor;

	private void Update()
	{
		RaycastHit hit;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100.0f) && (hit.transform.gameObject.GetComponents<Interaction>().Length != 0))
		{
			print(hit.transform.gameObject.name);
			cursor.SetActive(false);
		}
		else
		{
			cursor.SetActive(true);
		}
	}
    



}
