using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TANKMf : enemy
{
    [SerializeField] int dmg;

    void attackPlayer()
    {
        stats.getHealth(dmg);
    }

}
