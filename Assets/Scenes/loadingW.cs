using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingW : MonoBehaviour
{
    playerMVT p;
    private void Update()
    {
        if (!p)
            p = FindObjectOfType<playerMVT>();
        if (p.playerCanMove)
            gameObject.SetActive(false);
    }

}
