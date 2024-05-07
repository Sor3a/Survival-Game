using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sensebility : MonoBehaviour
{
    playerMVT mvt;
    Slider bar;
    private void Awake()
    {
        if(!mvt)
        mvt = FindObjectOfType<playerMVT>();
        if (!bar)
            bar = GetComponent<Slider>();
    }
    public void setSens()
    {
        if(bar.value!=0)
        mvt.setSensibility(bar.value);
        else
            mvt.setSensibility(.1f);
    }

}
