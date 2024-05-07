using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner1 : MonoBehaviour
{
    [SerializeField] bool work;
    public delegate void Function();
    public Function SpawnEverything;
    float[,] HeightMap;
    CustomTerrain terrain;
    TerrainData terrainData;
    Transform player;
    DayAndNight date;
    [HideInInspector] public bool ended =false;
    [System.Serializable]

    public class Spawn
    {
        public int dayOfSpawn;
        public float hour;
        public int b5; //0
        public int b4;
        public int dragon;
        public int trex;
        public int strongSnake;
        public int rat;
        public int skeleton;
        public int Snake;
        public int Slim;
        public int spider;
        public int cow;
        public int rabbit;
        public int deer;
        public int human1;
        public int human2;
        public int dayOrNight;
        public bool remove = false;
    }
    public List<Spawn> spawnList = new List<Spawn>()
    {
        new Spawn()
    };
    int[] numberOfEnemysToSpaw = new int[15];

    [System.Serializable]
    public class enemys
    {
       public GameObject enemy;
        public bool remove = false;
    }

    public List<enemys> enemysPrefabs = new List<enemys>()
    {
        new enemys()
    };
    public LayerMask Layer;
    private void Awake()
    {
        player = FindObjectOfType<playerStats>().transform;
        date = FindObjectOfType<DayAndNight>();
        SpawnEverything += SPawnEnemys;
    }
    public void addBeg()
    {
        enemysPrefabs.Add(new enemys());
        enemys e =  new enemys();
        e.enemy = enemysPrefabs[0].enemy;
        e.remove = false;
        enemysPrefabs[0] = enemysPrefabs[enemysPrefabs.Count - 1];
        enemysPrefabs[1] = e;
        for (int i = enemysPrefabs.Count-1; i > 1; i--)
        {
            enemysPrefabs[i] = enemysPrefabs[i - 1];
        }
        
    }
    void setNumber()
    {
        Spawn s = spawnList[0];
        numberOfEnemysToSpaw[0] = s.b5;
        numberOfEnemysToSpaw[1] = s.b4;
        numberOfEnemysToSpaw[2] = s.dragon;
        numberOfEnemysToSpaw[3] = s.trex;
        numberOfEnemysToSpaw[4] = s.strongSnake;
        numberOfEnemysToSpaw[5] = s.rat;
        numberOfEnemysToSpaw[6] = s.skeleton;
        numberOfEnemysToSpaw[7] = s.Snake;
        numberOfEnemysToSpaw[8] = s.Slim;
        numberOfEnemysToSpaw[9] = s.spider;
        numberOfEnemysToSpaw[10] = s.cow;
        numberOfEnemysToSpaw[11] = s.rabbit;
        numberOfEnemysToSpaw[12] = s.deer;
        numberOfEnemysToSpaw[13] = s.human1;
        numberOfEnemysToSpaw[14] = s.human2;

    }
    int timeOfDay(float time)
    {
        return time >= 6f && time <= 24f ? 0 : 1;
    }
    private void Start()
    {
        SpawnEverything();
        setNumber();
    }

    void spawnA()
    {
        if(!ended && work)
        {
            if (date.day >= spawnList[0].dayOfSpawn && date.TimeOfDay >= spawnList[0].hour && (timeOfDay(date.TimeOfDay) == spawnList[0].dayOrNight))
            {
                for (int i = 0; i < numberOfEnemysToSpaw.Length; i++)
                {
                    for (int j = 0; j < numberOfEnemysToSpaw[i]; j++)
                    {
                        float XHV = 20000;
                        float ZHV = 20000;
                        while (XHV >= terrainData.size.x - 5 || ZHV >= terrainData.size.z - 5 || XHV <= 5 || ZHV <= 5)
                        {
                            ZHV = player.position.z + Random.Range(-150f, 150f);
                            XHV = player.position.x + Random.Range(-150f, 150f);
                        }

                        Vector3 position = new Vector3(XHV, 100f, ZHV);

                        if (Physics.BoxCast(position + new Vector3(0, 1000f, 0), enemysPrefabs[i].enemy.transform.localScale, -Vector3.up, Quaternion.identity, Mathf.Infinity, Layer))
                            j--;
                        else
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(position + new Vector3(0, 1000f, 0), -Vector3.up, out hit, Mathf.Infinity))
                            {
                                GameObject b = Instantiate(enemysPrefabs[i].enemy, hit.point, Quaternion.identity);
                                b.transform.up = hit.normal;
                                // Debug.Log(Vector3.Distance(player.position, position));
                            }
                            else
                                j--;
                        }
                    }
                }
                spawnList[0].remove = true;
                RemoveSpawner();
                setNumber();
            }
        } 
    }
    private void Update()
    {
        spawnA();
    }
    void SPawnEnemys()
    {
        terrain = FindObjectOfType<CustomTerrain>();
        terrainData = terrain.terrainData;
        
    }


    public void spawnPlayer()
    {
        terrain = FindObjectOfType<CustomTerrain>();
        terrainData = terrain.terrainData;
        if (HeightMap == null)
        {

            HeightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        }
        int x;
        int z;
        float y;
        float waterHeight = terrain.WaterHeight;
        do
        {
             x = (int)Random.Range(5f, terrainData.heightmapResolution - 5f);
             z = (int)Random.Range(5f, terrainData.heightmapResolution - 5f);
             y = HeightMap[x, z];

        } while (y <= waterHeight);

        int XHV = (int)(x / (float)terrainData.heightmapResolution * terrainData.size.x);
        int ZHV = (int)(z / (float)terrainData.heightmapResolution * terrainData.size.z);
        Vector3 position = new Vector3(ZHV, y * terrainData.size.y + 4f, XHV);
        player.SetPositionAndRotation(position, Quaternion.identity);
        StartCoroutine(PlayerCanMove());
    }
    IEnumerator PlayerCanMove()
    {
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<playerMVT>().playerCanMove = true;
    }


    public void AddSpawner()
    {
        spawnList.Add(new Spawn());
    }
    public void AddEnemy()
    {
        enemysPrefabs.Add(new enemys());
    }
    public void RemoveEnemy()
    {
        
        List<enemys> temp = new List<enemys>();
        for (int i = 0; i < enemysPrefabs.Count; i++)
        {
            if (!enemysPrefabs[i].remove)
                temp.Add(enemysPrefabs[i]);
            
                
        }
        if (temp.Count == 0)
            temp.Add(enemysPrefabs[0]);
        enemysPrefabs = temp;
    }
    public void RemoveSpawner()
    {
        List<Spawn> temp = new List<Spawn>();
        for (int i = 0; i < spawnList.Count; i++)
        {
            if (!spawnList[i].remove)
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
   
    

}
