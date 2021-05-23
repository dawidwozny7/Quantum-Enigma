using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchboard : MonoBehaviour
{

    public static void HideEight(){
        GameObject.Find("BoardPlane").SetActive(false);
    }
}
