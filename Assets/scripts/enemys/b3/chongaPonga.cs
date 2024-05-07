using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chongaPonga : enemy
{
    [SerializeField] GameObject rock;
    [SerializeField] int walknigDmg, rockDmg;
    [SerializeField] float timeToAttack, RockSpeed,radiousOfMVTDMG;
    [SerializeField] Transform shootingPos,legsPos;

    void createRock()
    {
        GameObject b = Instantiate(rock, shootingPos.position, Quaternion.identity);
        b.GetComponent<moveFire>().setStuff(rockDmg, (player.position - shootingPos.position).normalized, RockSpeed);
        Destroy(b, 6f);
    }
    void dmgWalking()
    {
        Collider[] players = Physics.OverlapSphere(legsPos.position, radiousOfMVTDMG, playerLayer);
        if (players.Length > 0)
        {
            foreach (var item in players)
            {
                playerStats stats = item.gameObject.GetComponent<playerStats>();
                if (stats != null)
                    stats.getHealth(walknigDmg);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(legsPos.position, radiousOfMVTDMG);
    }
}
