using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spell : MonoBehaviour
{
    public bool isPlayerIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            isPlayerIn = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            isPlayerIn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            isPlayerIn = false;
    }
}
