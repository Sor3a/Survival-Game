using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class armorHolder : holder
{
    setItemsInPlayer setItemsInPlayer;
    private void Awake()
    {
       
        setItemsInPlayer = FindObjectOfType<setItemsInPlayer>();
    }
    public void set(item item)
    {
        setItem(item);

        if (item is armor Armore)
            FindObjectOfType<playerStats>().armor = Armore.armorAdd;
        setItemsInPlayer.setHolders();
    }

}
