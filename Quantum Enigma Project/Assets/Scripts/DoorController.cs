using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator doorAnimation;

    private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Player"))
		{
            doorAnimation.SetBool("isOpening", true);
        }
            
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
		{
            doorAnimation.SetBool("isOpening", false);
        }
            
    }

    void Start()
    {
        doorAnimation = this.transform.parent.GetComponent<Animator>();
    }
}
