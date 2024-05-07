using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowHolder : holder
{
    selectItem select;
    setItemsInPlayer setItemsInPlayer;
    private void Awake()
    {
        select = FindObjectOfType<selectItem>();
        setItemsInPlayer = FindObjectOfType<setItemsInPlayer>();
    }
    public void set(item item)
    {
        setItem(item);
        setItemsInPlayer.setHolders();
        select.FixItem(1);
    }
}
