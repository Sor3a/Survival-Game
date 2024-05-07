using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerContole : MonoBehaviour
{

    [HideInInspector] public DayAndNight day_night;
    [SerializeField] GameObject[] enemys;
    [SerializeField] int randomness;
    [SerializeField] LayerMask terrainLayer;
    bool CanSpawn = true;
    crafRock r;


    [System.Serializable]
    public class spawning
    {
        //public int day;
        public int start;
        public int end;
    }

    public List<spawning> enemysSpawning = new List<spawning>();

    private void OnEnable()
    {
        day_night = FindObjectOfType<DayAndNight>();
        r = GetComponent<crafRock>();
    }
    GameObject findEnemy()
    {
        int dayO = day_night.day;
        return enemys[Random.Range(enemysSpawning[dayO].start, enemysSpawning[dayO].end + 1)];
    }

    void spawnEnemys(GameObject enemy)
    {
        int max = Random.Range(0, 3) > 0 ? 2 : 3;
        for (int i = 0; i < max; i++)
        {
            Ray r = new Ray(transform.position + new Vector3(Random.Range(-randomness, randomness), 1000, Random.Range(-randomness, randomness)), -Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, Mathf.Infinity, terrainLayer))
            {
               GameObject b=  Instantiate(enemy, hit.point, Quaternion.identity);
                this.r.bla.Add(b);
            }
            else
                i--;
        }
    }

    public void spawn()
    {
        if(CanSpawn)
        {
            spawnEnemys(findEnemy());
            r.enabled = true;
            CanSpawn = false;
        }

    }
}
