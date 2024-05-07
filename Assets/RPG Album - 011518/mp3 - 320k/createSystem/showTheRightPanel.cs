using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showTheRightPanel : MonoBehaviour
{
    [SerializeField] GameObject[] panels;


    private void Start()
    {
        foreach (var item in panels)
        {
            item.SetActive(false);
        }
        panels[0].SetActive(true);
    }
    public void setGameObjectActive(int obj)
    {
        foreach (var item in panels)
        {
            item.SetActive(false);
        }
        panels[obj].SetActive(true);
    }

}
