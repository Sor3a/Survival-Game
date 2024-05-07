using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyStone : MonoBehaviour
{
    public List<GameObject> enemys = new List<GameObject>();

    GameObject randomEnemy()
    {
        return enemys[Random.Range(0, enemys.Count)];
    }
    private void OnEnable()
    {
        GetComponent<craft>().Enemy = randomEnemy();
    }
}
