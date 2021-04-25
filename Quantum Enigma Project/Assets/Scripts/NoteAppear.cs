using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour
{
    public Image noteImage;

    void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Player"))
		{
			noteImage.enabled = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			noteImage.enabled = false;
		}
	}
}
