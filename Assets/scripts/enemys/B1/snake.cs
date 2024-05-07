using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snake : enemy
{
    [SerializeField] int dmg;

    void attackPlayer()
    {
        stats.getHealth(dmg);
        speedToAttackR = speedToAttack;
    }

}