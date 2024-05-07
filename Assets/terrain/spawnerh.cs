using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerh : MonoBehaviour
{
    public delegate void Function();
    public Function SpawnEverything;
    float[,] HeightMap;
    CustomTerrain terrain;
    TerrainData terrainData;
    Transform player;
    DayAndNight date;
    [System.Serializable]

    public class Spawn
    {
        public GameObject enemy;
        public float timeToNextSpawnD;//time to next spawn day
        public float timeToNextSpawnN;
        //public int numberOfEnemys;
        public int NightSpawn; //Number of enemys night
        public int DaySpawn; //Number of enemys day
        public int DaySpawnStart; // the day they start spawning
        public int adder; // how much we add every numberofDaysToAdd
        public int NumberOfDayWeAdd; // when we add every NumberOfDayWeAdd we add the adder
        
        [HideInInspector] public int LastDay=0;
        public bool remove = false;
    }

    public LayerMask Layer;
    void SPawnEnemys()
    {
        terrain = FindObjectOfType<CustomTerrain>();
        terrainData = terrain.terrainData;
        SpawnE();
    }
    private void Awake()
    {
        player = FindObjectOfType<playerStats>().transform;
        date = FindObjectOfType<DayAndNight>();
        SpawnEverything += SPawnEnemys;
    }
    public void spawnPlayer()
    {
        terrain = FindObjectOfType<CustomTerrain>();
        terrainData = terrain.terrainData;
        if (HeightMap == null)
        {

            HeightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        }

        int x = (int)Random.Range(5f, terrainData.heightmapResolution - 5f);
        int z = (int)Random.Range(5f, terrainData.heightmapResolution - 5f);
        float y = HeightMap[x, z];
        int XHV = (int)(x / (float)terrainData.heightmapResolution * terrainData.size.x);
        int ZHV = (int)(z / (float)terrainData.heightmapResolution * terrainData.size.z);
        Vector3 position = new Vector3(ZHV, y * terrainData.size.y + 4f, XHV);
        player.SetPositionAndRotation(position, Quaternion.identity);
        StartCoroutine(PlayerCanMove());
    }
    IEnumerator PlayerCanMove()
    {
        yield return new WaitForEndOfFrame();
        FindObjectOfType<playerMVT>().playerCanMove = true;
    }
    private void Start()
    {
        SpawnEverything();
    }
    public List<Spawn> spawnList = new List<Spawn>()
    {
        new Spawn()
    };
    public void AddSpawner()
    {
        spawnList.Add(new Spawn());
    }
    public void RemoveSpawner()
    {
        List<Spawn> temp = new List<Spawn>();
        for (int i = 0; i < spawnList.Count; i++)
        {
            if (spawnList[i].remove)
                temp.Add(spawnList[i]);
        }
        if (temp.Count == 0)
            temp.Add(spawnList[0]);
        spawnList = temp;
    }

    bool Isday()
    {
        return date.TimeOfDay < 18 && date.TimeOfDay > 6;
    }
    IEnumerator Creation(Spawn EnemyToSpawn, float time)
    {
        yield return new WaitForSeconds(time);

        bool isDay = Isday();
        if (HeightMap == null)
            HeightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        if (EnemyToSpawn.DaySpawnStart <= date.day)
        {
            if (((date.day - EnemyToSpawn.DaySpawnStart) % EnemyToSpawn.NumberOfDayWeAdd) == 0 && date.day != EnemyToSpawn.LastDay && (date.day - EnemyToSpawn.DaySpawnStart)!=0)
            {
                EnemyToSpawn.adder++;
                EnemyToSpawn.LastDay = date.day;
            }
                
            for (int i = 0; i < ((isDay ? EnemyToSpawn.DaySpawn : EnemyToSpawn.NightSpawn)+ EnemyToSpawn.adder); i++)
            {
                //int x = (int)Random.Range(5f, terrainData.heightmapResolution - 5f);
                //int z = (int)Random.Range(5f, terrainData.heightmapResolution - 5f);
                //float y = HeightMap[x, z];
                //int XHV = (int)(x / (float)terrainData.heightmapResolution * terrainData.size.x);
                //int ZHV = (int)(z / (float)terrainData.heightmapResolution * terrainData.size.z);
                float XHV = 20000;
                float ZHV = 20000;
                while (XHV >= terrainData.size.x - 5 || ZHV >= terrainData.size.z - 5 || XHV <= 5 || ZHV <= 5)
                {
                    ZHV = player.position.z + Random.Range(-80f, 80f);
                    XHV = player.position.x + Random.Range(-80f, 80f);
                }

                Vector3 position = new Vector3(XHV, 100f, ZHV);

                if (Physics.BoxCast(position + new Vector3(0, 1000f, 0), EnemyToSpawn.enemy.transform.localScale, -Vector3.up, Quaternion.identity, Mathf.Infinity, Layer))
                    i--;
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(position + new Vector3(0, 1000f, 0), -Vector3.up, out hit, Mathf.Infinity))
                    {
                        GameObject b = Instantiate(EnemyToSpawn.enemy, hit.point, Quaternion.identity);
                        b.transform.up = hit.normal;
                       // Debug.Log(Vector3.Distance(player.position, position));
                    }
                    else
                        i--;
                }

            }
        }
        if (isDay)
            StartCoroutine(Creation(EnemyToSpawn, EnemyToSpawn.timeToNextSpawnD));
        else
            StartCoroutine(Creation(EnemyToSpawn, EnemyToSpawn.timeToNextSpawnN));
    }
    void SpawnE()
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            if (Isday())
                StartCoroutine(Creation(spawnList[i], spawnList[i].timeToNextSpawnD));
            else
                StartCoroutine(Creation(spawnList[i], spawnList[i].timeToNextSpawnN));
        }
    }

}
