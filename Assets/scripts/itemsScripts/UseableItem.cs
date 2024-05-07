using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseableItem : MonoBehaviour
{
    playerMVT mvt;
    playerStats stats;
    [SerializeField] float time;
    public ParticleSystem p;
    public ParticleSystem[] PickItemsEffects;
    private void Awake()
    {
        mvt = GetComponent<playerMVT>();
        stats = GetComponent<playerStats>();
    }

    public ParticleSystem p2()
    {
        return PickItemsEffects[Random.Range(0, PickItemsEffects.Length)];
    }
    public void usePotion(potions Potions)
    {
        switch (Potions.type)
        {
            case 1:
                {
                    mvt.addSpeed(Potions.speed, time);
                    break;
                }
            case 2:
                {
                    stats.AddDmg(Potions.force, time);
                    break;
                }
            case 3:
                {
                    mvt.addJump(Potions.jump, time);
                    break;
                }
            case 4:
                {
                    stats.Addhealth(Potions.health);
                    break;
                }
            case 5:
                {
                    stats.AddEating((float)Potions.force, Potions.health);
                    break;
                }
        }
    }
}
