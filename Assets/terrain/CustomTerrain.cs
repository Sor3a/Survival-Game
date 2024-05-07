using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using System.Collections;
using System.Threading.Tasks;

//[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
public class CustomTerrain : MonoBehaviour
{
    public Vector2 randomHeightRange = new Vector2(0, 0.1f);
    public Terrain terrain;
    public TerrainData terrainData;
    public Texture2D heightMapImage;
    public Vector3 heightMapScale = new Vector3(1, 1, 1);

    public bool restartData = true;

    //single perlin noise
    public float perlinXScale = 0.003f;
    public float perlinYScale = .003f;
    public int perlinOffsetX = 0;
    public int perlinOffsetY = 0;
    public int perlinOctaves = 3;
    public float perlinPersistance = 8;
    public float perlingHeightScale = 0.009f;

    //multiple perlin noise
    [System.Serializable]
    public class PerlinParameters
    {
        public float mperlinXScale = .01f;
        public float mperlinYScale = .01f;
        public int mperlinOffsetX = 0;
        public int mperlinOffsetY = 0;
        public int mperlinOctaves = 3;
        public float mperlinPersistance = 8;
        public float mperlingHeightScale = 0.09f;
        public bool remove = false;
    }
    public List<PerlinParameters> perlinParameters = new List<PerlinParameters>()
    {
        new PerlinParameters()
    };

    //Voronnoi
    public int count = 1;
    public float MinHeight;
    public float maxHeight;
    public float fallOff;
    public float DropOff;

    //midPoint
    public float height;
    public float roughness;
    //smooth
    public int smoothAmmount;

    [System.Serializable]
    public class SplatHeights
    {
        public Texture2D texture = null;
        public float minHeight = .1f;
        public float maxHeight = .2f;
        public float minSlope = 0;
        public float maxSlope = 90f;
        public Vector2 titleOffset;
        public Vector2 titleSize = new Vector2(50, 50);
        public bool remove = false;
    }
    public float noiseStrenght;
    public List<SplatHeights> splatHeights = new List<SplatHeights>()
    {
        new SplatHeights()
    };

    public Texture2D Htexture;
    public float terrainHeight = 600f;


    //vegetation
    public int MaxTrees = 1000;
    public int treesSpacing = 5;
    public int groundLayer;
    [System.Serializable]
    public class trees
    {
        public GameObject prefab = null;
        public float minHeight = .1f;
        public float maxHeight = .2f;
        public float maxHeightScale = 1.2f;
        public float minHeightScale = .5f;
        public Color color;
        public Color lightColor;
        public float minSlope = 0;
        public float maxSlope = 90f;
        public float density;
        public bool remove = false;
    }
    public List<trees> stuff = new List<trees>
    {
        new trees()
    };

    //Details
    [System.Serializable]
    public class Details
    {
        public GameObject prototype = null;
        public Texture2D prototypeTexture = null;
        public float minHeight = .1f;
        public float maxHeight = .2f;
        public float minSlope = 0;
        public float maxSlope = 90f;
        public float overlape = .01f;
        public float feather = .05f;
        public float density = .5f;
        public int randomness = 3;
        public Color HealthyColor;
        public Color DryColor;
        public float edgePadding;
        public float noiseSpread;
        public bool remove = false;
    }
    public List<Details> details = new List<Details>()
    { new Details()};
    public int MaxDetails = 5000;
    public int detailsSpacing = 5;

    //water
    public float WaterHeight;
    public GameObject WaterGo;

    //Erosion
    public enum ErosionType
    {
        Rain = 0, Thermal = 1, Tridal = 2, River = 3, Wind = 4
    }
    public ErosionType erosionType = ErosionType.Rain;
    public float ecrosionStrngth = .1f;
    public int springsPerRiver = 5;
    public float solubility = .01f;
    public int droplets = 10;
    public int ecrosionSmooth = 5;
    public GameObject rain;


    //Random
    public Vector2 ScalesRange = new Vector2(0.001f, 0.002f);
    public Vector2 OffsetRange = new Vector2(0, 10000);
    public Vector2 OctavesRange = new Vector2(3, 4);
    public Vector2 HeightScaleRange = new Vector2(0.2f, 0.5f);
    public Vector2 PresistanceRange = new Vector2(.1f, 10f);
    [System.Serializable]
    public class treeSpawn
    {
        public GameObject tree;
        public int treeNumber = 0;
        public float Adjustment;
        public Vector2 stepnessRange = new Vector2(0,90);
        public bool remove = false;
    }
    public List<treeSpawn> TreeList = new List<treeSpawn>()
    {
        new treeSpawn()
    };
    public LayerMask layer;
    public LayerMask terrainLayere;
    spawner1 ss;
    public void AddTree()
    {
        TreeList.Add(new treeSpawn());
    }
    public void RemoveTree()
    {
        List<treeSpawn> temp = new List<treeSpawn>();
        for (int i = 0; i < TreeList.Count; i++)
        {
            if (!TreeList[i].remove)
                temp.Add(TreeList[i]);
        }
        if(temp.Count ==0)
            temp.Add(TreeList[0]);
        TreeList = temp;
    }
    private void Awake()
    {
        //SerializedObject tagManger = new SerializedObject(
        //    AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        //SerializedProperty tagsProp = tagManger.FindProperty("tags");
        //AddTag(tagsProp, "Terrain", 0);
        //AddTag(tagsProp, "Cloud", 0);
        //AddTag(tagsProp, "Shore", 0);
        //tagManger.ApplyModifiedProperties();
        //SerializedProperty layerProp = tagManger.FindProperty("layers");
        ////terrainLayer = AddTag(layerProp, "Terrain", TagType.Layer);
        //tagManger.ApplyModifiedProperties();

        this.gameObject.tag = "Terrain";
        this.gameObject.layer = terrainLayer;
         ss = FindObjectOfType<spawner1>();
        resetTerrain();
        ss.SpawnEverything += setRanges;
        ss.SpawnEverything += RandomGeneration;
        ss.SpawnEverything += SplatMaps;
        //ss.SpawnEverything += PlantVegetation;
        ss.SpawnEverything += CreateDetails;
    }
    //World world; 
    PhysicsWorld physicsWorld;
    EntityManager manager;
    StepPhysicsWorld stepPhysicsWorld;
    private void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        stepPhysicsWorld = world.GetOrCreateSystem<StepPhysicsWorld>();
        physicsWorld = world.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;
        raycastHits = new NativeList<Unity.Physics.RaycastHit>(Allocator.Persistent);
        manager = world.EntityManager;
    }
    private void OnDestroy()
    {
        if (raycastHits.IsCreated) raycastHits.Dispose();
    }
    void setRanges()
    {

        ScalesRange = new Vector2(0.0015f, 0.003f);
        OffsetRange = new Vector2(0, 10000);
        OctavesRange = new Vector2(2, 5);
        HeightScaleRange = new Vector2(0.26f, .3f);
        PresistanceRange = new Vector2(2f, 2.5f);
    }
    float[,] getHeightMap()
    {
        if (!restartData)
            return terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        else
            return new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
    }
    void RandomGeneration()
    {
        
        float scale = UnityEngine.Random.Range(ScalesRange.x, ScalesRange.y);
        perlinXScale = scale;
        perlinYScale = scale;
        perlinOffsetX =(int) UnityEngine.Random.Range(OffsetRange.x, OffsetRange.y);
        perlinOffsetY = (int)UnityEngine.Random.Range(OffsetRange.x, OffsetRange.y);
        perlinOctaves = 3;
        perlinPersistance = UnityEngine.Random.Range(PresistanceRange.x, PresistanceRange.y);
        perlingHeightScale = UnityEngine.Random.Range(HeightScaleRange.x, HeightScaleRange.y);
        Perlin();
        smooth();
        smooth();

        for (int i = 0; i < TreeList.Count; i++)
        {
            spawnRandom(TreeList[i].tree, TreeList[i].treeNumber, TreeList[i].Adjustment, TreeList[i].stepnessRange);
        }
        FindObjectOfType<Forest>().spawnForest();
        ss.spawnPlayer();

    }
    void spawnRandom(GameObject stuff,int treeNumber, float Adjustment,Vector2 Stepnessrange)
    {
        float[,] HeightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        for (int i = 0; i < treeNumber; i++)
        {
            int x = (int)UnityEngine.Random.Range(5f, terrainData.heightmapResolution - 5f);
            int z = (int)UnityEngine.Random.Range(5f, terrainData.heightmapResolution - 5f);
            float y = HeightMap[x, z];
            int XHV = (int)(x / (float)terrainData.heightmapResolution * terrainData.size.x);
            int ZHV = (int)(z / (float)terrainData.heightmapResolution * terrainData.size.z);
            Vector3 position = new Vector3(ZHV, y * terrainData.size.y+1000f, XHV);
            if (Physics.BoxCast(position, stuff.gameObject.transform.localScale,-Vector3.up, Quaternion.identity,Mathf.Infinity, layer))
                i--;
            else
            {
                UnityEngine.RaycastHit hit;
                if (Physics.Raycast(position, -Vector3.up,out hit ,Mathf.Infinity, terrainLayere))
                {
                    float stepness = terrainData.GetSteepness(hit.point.x / terrainData.size.x, hit.point.z / terrainData.size.z);
                    if (((hit.point.y / terrainData.size.y) > WaterHeight) && (stepness >= Stepnessrange.x && stepness <= Stepnessrange.y))
                    {
                        Quaternion q = Quaternion.FromToRotation(Vector3.up, hit.normal);
                        q *= Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0);
                        Vector3 positionToSpawn = hit.point + new Vector3(0, Adjustment, 0);
                        //stuffe.up = hit.normal;
                        // q = Quaternion.rot
                        GameObject b = Instantiate(stuff, positionToSpawn, q);
                        //stuffe.RotateAround(stuffe.position, stuffe.up, UnityEngine.Random.Range(0, 360f));
                        //if (q.Equals(stuffe.rotation)) Debug.Log(true);
                        b.transform.parent = transform;
                    }
                    else
                        i--;
                }     
            }       
        }
    }
    NativeList<Unity.Physics.RaycastHit> raycastHits;
    struct raycastHitJob : IJob
    {
        public PhysicsWorld physicsWorld;
        public NativeList<Unity.Physics.RaycastHit> raycastHits;
        public RaycastInput raycastInput;
        public void Execute()
        {
            physicsWorld.CastRay(raycastInput, ref raycastHits);

        }
    }
    
    bool checkIfHits(Vector3 start, Vector3 end)
    {
        Debug.Log("a");
        //Task thread1 = Task.Factory.StartNew(() => stepPhysicsWorld.Simulation.FinalJobHandle.Complete());
        //Task.WaitAll(thread1);
        
        if (raycastHits.IsCreated)
        raycastHits.Clear();
        var sa = start;
        var en = end;
        var raycastInput = new RaycastInput
        {
            Start = sa,
            End = en,
            Filter = new CollisionFilter
            {
                BelongsTo = 0,
                CollidesWith = 0,
            }
        };
        Debug.Log("a");
        stepPhysicsWorld.Simulation.FinalJobHandle.Complete();
        new raycastHitJob
        {
            physicsWorld = physicsWorld,
            raycastHits = raycastHits,
            raycastInput = raycastInput,
        }.Run();
        Debug.Log(raycastHits.Length);

       //rayCastJob.Complete();
        if (raycastHits.IsCreated)
            return raycastHits.Length > 0 ? true : false;

        return false;
    }
    public (Quaternion? rotation,Vector3? position) spawnRandom(float Adjustment)
    {
        float[,] HeightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        (Quaternion?, Vector3?) posAndRot = new(null, null);
        int i = 0;
        while(posAndRot == (null,null) && i++<1000)
        {
            //stepPhysicsWorld.Simulation.FinalJobHandle.Complete();
            int x = (int)UnityEngine.Random.Range(5f, terrainData.heightmapResolution - 5f);
            int z = (int)UnityEngine.Random.Range(5f, terrainData.heightmapResolution - 5f);
            float y = HeightMap[x, z];
            int XHV = (int)(x / (float)terrainData.heightmapResolution * terrainData.size.x);
            int ZHV = (int)(z / (float)terrainData.heightmapResolution * terrainData.size.z);
            Vector3 position = new Vector3(ZHV, y * terrainData.size.y + 1000f, XHV);
            //Debug.Log(position);
            #region changment

            if (!checkIfHits(position, -Vector3.up))//Physics.BoxCast(position, stuff.gameObject.transform.localScale, -Vector3.up, Quaternion.identity, Mathf.Infinity, layer)
            {
                UnityEngine.RaycastHit hit;
                if (Physics.Raycast(position, -Vector3.up, out hit, Mathf.Infinity, terrainLayere))
                {
                    //float stepness = terrainData.GetSteepness(hit.point.x / terrainData.size.x, hit.point.z / terrainData.size.z);
                    if (((hit.point.y / terrainData.size.y) > WaterHeight))
                    {
                        Quaternion q = Quaternion.FromToRotation(Vector3.up, hit.normal);
                        q *= Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0);
                        Vector3 positionToSpawn = hit.point + new Vector3(0, Adjustment, 0);
                        posAndRot = (q, positionToSpawn);
                        //stuffe.up = hit.normal;
                        // q = Quaternion.rot
                        //GameObject b = Instantiate(stuff, positionToSpawn, q);
                        //stuffe.RotateAround(stuffe.position, stuffe.up, UnityEngine.Random.Range(0, 360f));
                        //if (q.Equals(stuffe.rotation)) Debug.Log(true);
                        //b.transform.parent = transform;
                    }
                    else i--;
                }
            }
            #endregion
        }
        return posAndRot;
    }
    

    public void AddWater()
    {
        GameObject water = GameObject.Find("water");
        if(!water)
        {
            water = Instantiate(WaterGo,transform.position,transform.rotation);
            water.name = "water";
        }

        water.transform.position = transform.position + new Vector3(terrainData.size.x / 2, WaterHeight * terrainData.size.y, terrainData.size.z / 2);
        water.transform.localScale = new Vector3(terrainData.size.x/2, 1, terrainData.size.z/2);
    }
    public void Erode()
    {
        switch (erosionType)
        {
            case ErosionType.Rain:
                {
                    Rain();
                    break;
                }
            case ErosionType.Tridal:
                {
                    Tridal();
                    break;
                }
            case ErosionType.Thermal:
                {
                    Thermal();
                    break;
                }
            case ErosionType.River:
                {
                    River();
                    break;
                }
            case ErosionType.Wind:
                {
                    Wind();
                    break;
                }
        }
        for (int i = 0; i < ecrosionSmooth; i++)
        {
            smooth();
        }
    }
    public void Rain()
    {
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        for (int i = 0; i < droplets; i++)
        {
            int x = UnityEngine.Random.Range(0, terrainData.heightmapResolution);
            int z = UnityEngine.Random.Range(0, terrainData.heightmapResolution);
            float y = heightMap[x, z]+200f;
            GameObject b = Instantiate(rain, new Vector3(x, y, z), Quaternion.identity);
            heightMap[x, z] -= solubility;
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void Tridal()
    {

    }
    public void Thermal()
    {

    }
    public void River()
    {

    }
    public void Wind()
    {

    }

    public void CreateDetails()
    {
        DetailPrototype[] NewDetailPrototypes;
        NewDetailPrototypes = new DetailPrototype[details.Count];
        int index = 0;
        foreach (var d in details)
        {
            NewDetailPrototypes[index] = new DetailPrototype();
            NewDetailPrototypes[index].prototype = d.prototype;
            NewDetailPrototypes[index].prototypeTexture = d.prototypeTexture;
            NewDetailPrototypes[index].healthyColor = d.HealthyColor;
            NewDetailPrototypes[index].holeEdgePadding = d.edgePadding;
            NewDetailPrototypes[index].noiseSpread = d.noiseSpread;
            NewDetailPrototypes[index].dryColor = d.DryColor;
            
            if (NewDetailPrototypes[index].prototype)
            {
                NewDetailPrototypes[index].usePrototypeMesh = true;
                NewDetailPrototypes[index].renderMode = DetailRenderMode.VertexLit;
            }
            else
            {
                NewDetailPrototypes[index].usePrototypeMesh = false;
                NewDetailPrototypes[index].renderMode = DetailRenderMode.GrassBillboard;
            }
            index++;
        }
        float[,] HeightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        terrainData.detailPrototypes = NewDetailPrototypes;
        for (int i = 0; i < terrainData.detailPrototypes.Length; i++)
        {
            int[,] detailMap = new int[terrainData.detailHeight, terrainData.detailWidth];
            for (int y = 0; y < terrainData.detailHeight - detailsSpacing; y+=detailsSpacing)
            {
                for (int x = 0; x < terrainData.detailWidth - detailsSpacing; x += detailsSpacing)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > details[i].density) continue; 
                    int NewX = UnityEngine.Random.Range(0, details[i].randomness)+x;
                    int NewY = UnityEngine.Random.Range(0, details[i].randomness) +y;
                    float noise = utile.Map(Mathf.PerlinNoise(NewX * details[i].feather, NewY * details[i].feather), 0, 1, .5f, 1);
                    int xHM = (int)(NewX / (float)terrainData.detailWidth * terrainData.heightmapResolution);
                    int yHM = (int)(NewY / (float)terrainData.detailHeight * terrainData.heightmapResolution);
                    float height = HeightMap[yHM,xHM];
                    float steepness = terrainData.GetSteepness(xHM/(float) terrainData.heightmapResolution, yHM/(float) terrainData.heightmapResolution);
                    float maxHeight = (details[i].maxHeight + details[i].overlape)*noise;
                    float minHeight = (details[i].minHeight - details[i].overlape)*noise;
                    if (((height <= maxHeight ) && (height >= minHeight- details[i].overlape)) && (steepness>= details[i].minSlope && steepness<=details[i].maxSlope))
                        detailMap[NewY, NewX] = 1;
                    else
                    {
                        detailMap[NewY, NewX] = 0;
                    }
                }
            }
            terrainData.SetDetailLayer(0, 0, i, detailMap);
        }
    
    }
    public void AddDetails()
    {
        details.Add(new Details());
    }
    public void RemoveDetails()
    {
        List<Details> newListe = new List<Details>();
        for (int i = 0; i < details.Count; i++)
        {
            if (!details[i].remove)
                newListe.Add(details[i]);
        }
        if (newListe.Count <= 0)
            newListe.Add(details[0]);
        details = newListe;
    }
    public void AddTrees()
    {
        stuff.Add(new trees());
    }
    public void RemoveTrees()
    {
        List<trees> newListe = new List<trees>();
        for (int i = 0; i < stuff.Count; i++)
        {
            if (!stuff[i].remove)
                newListe.Add(stuff[i]);
        }
        if(newListe.Count<=0)
            newListe.Add(stuff[0]);
        stuff = newListe;

    }

    public void PlantVegetation()
    {
        TreePrototype[] newTreePrototypes;
        newTreePrototypes = new TreePrototype[stuff.Count];
        int tindex = 0;
        foreach (var t in stuff)
        {
            newTreePrototypes[tindex] = new TreePrototype();
            newTreePrototypes[tindex].prefab = t.prefab;
            tindex++;
        }
        terrainData.treePrototypes = newTreePrototypes;

        List<TreeInstance> allVegetation = new List<TreeInstance>();
        for (int z = 0; z < terrainData.size.z; z += treesSpacing)
        {
            for (int x = 0; x < terrainData.size.x; x += treesSpacing)
            {
                for (int tp = 0; tp < terrainData.treePrototypes.Length; tp++)
                {

                    float thisHeight = terrainData.GetHeight(x, z) / terrainData.size.y;
                    float thisHeightStart = stuff[tp].minHeight;
                    float thisHeightEnd = stuff[tp].maxHeight;

                    float steepness = terrainData.GetSteepness(x / (float)terrainData.size.x,
                                                               z / (float)terrainData.size.z);
                    if ((thisHeight >= thisHeightStart && thisHeight <= thisHeightEnd) &&( steepness>= stuff[tp].minSlope && steepness<=stuff[tp].maxSlope))
                    {
                        TreeInstance instance = new TreeInstance();
                        instance.position = new Vector3((x + UnityEngine.Random.Range(-treesSpacing, treesSpacing)) / terrainData.size.x,
                                                        terrainData.GetHeight(x, z) / terrainData.size.y,
                                                        (z + UnityEngine.Random.Range(-treesSpacing, treesSpacing)) / terrainData.size.z);

                        Vector3 treeWorldPos = new Vector3(instance.position.x * terrainData.size.x,
                            instance.position.y * terrainData.size.y,
                            instance.position.z * terrainData.size.z)
                                                         + this.transform.position;

                        UnityEngine.RaycastHit hit;
                        int layerMask = 1 << terrainLayer;

                        if (Physics.Raycast(treeWorldPos + new Vector3(0, 1000, 0), -Vector3.up, out hit, Mathf.Infinity) ||
                            Physics.Raycast(treeWorldPos - new Vector3(0, 1000, 0), Vector3.up, out hit, Mathf.Infinity))
                        {

                            if(hit.collider.gameObject.layer == groundLayer)
                            {
                                if ((hit.point.y / terrainData.size.y) > WaterHeight)
                                {
                                    if (UnityEngine.Random.Range(0f, 1f) > stuff[tp].density) continue;
                                    float treeHeight = (hit.point.y - this.transform.position.y) / terrainData.size.y;
                                    instance.position = new Vector3(instance.position.x,
                                                                     treeHeight,
                                                                     instance.position.z);
                                    
                                    instance.prototypeIndex = tp;
                                    instance.color = stuff[tp].color;
                                    instance.lightmapColor = stuff[tp].lightColor;
                                    float r = UnityEngine.Random.Range(stuff[tp].minHeightScale, stuff[tp].maxHeightScale);
                                    instance.heightScale = r;
                                    instance.widthScale = r;
                                    allVegetation.Add(instance);
                                }
                            }
                            if (allVegetation.Count >= MaxTrees) goto TREESDONE;
                        }


                    }
                }
            }
        }
    TREESDONE:
        terrainData.treeInstances = allVegetation.ToArray();

    }

    List<Vector2> GenerateNeighbours(Vector2 pos, int width, int height)
    {
        List<Vector2> neighbours = new List<Vector2>();
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (!(x == 0 && y == 0))
                {
                    Vector2 nPos = new Vector2(Mathf.Clamp(pos.x + x, 0, width - 1),
                                                Mathf.Clamp(pos.y + y, 0, height - 1));
                    if (!neighbours.Contains(nPos))
                        neighbours.Add(nPos);
                }
            }
        }
        return neighbours;
    }
    public void AddSplat()
    {
        splatHeights.Add(new SplatHeights());
    }
    public void RemoveSplat()
    {
        List<SplatHeights> temp = new List<SplatHeights>();
        for (int i = 0; i < splatHeights.Count; i++)
        {
            if (!splatHeights[i].remove)
                temp.Add(splatHeights[i]);
        }
        if (temp.Count <= 0)
            temp.Add(splatHeights[0]);
        splatHeights = temp;
    }
    public void SplatMaps()
    {
        TerrainLayer[] newSplatPrototypes;
        newSplatPrototypes = new TerrainLayer[splatHeights.Count];
        int index = 0;
        foreach (var sh in splatHeights)
        {
            newSplatPrototypes[index] = new TerrainLayer();
            newSplatPrototypes[index].diffuseTexture = sh.texture;
            newSplatPrototypes[index].tileOffset = sh.titleOffset;
            newSplatPrototypes[index].tileSize = sh.titleSize;
            newSplatPrototypes[index].diffuseTexture.Apply();
            index++;
        }
        terrainData.terrainLayers = newSplatPrototypes;
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        float[,,] splatMapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
        for (int x = 0; x < terrainData.alphamapHeight; x++)
        {
            for (int y = 0; y < terrainData.alphamapWidth; y++)
            {
                float[] splat = new float[terrainData.alphamapLayers];
                for (int i = 0; i < splatHeights.Count; i++)
                {
                    float noise = Mathf.PerlinNoise(x * .01f, y * .01f) * noiseStrenght;
                    float HeightStart = splatHeights[i].minHeight - noise;
                    float HeightStop = splatHeights[i].maxHeight + noise;
                    float steepness = terrainData.GetSteepness(y / (float)terrainData.alphamapResolution, x / (float)terrainData.alphamapResolution);

                    if(heightMap[x,y]>=HeightStart&&heightMap[x,y]<=HeightStop &&
                        (steepness>=splatHeights[i].minSlope && steepness<=splatHeights[i].maxSlope))
                    {
                        splat[i] = 1;
                    }
                }
                NormalizeVector(splat);
                for (int i = 0; i < splatHeights.Count; i++)
                {
                    splatMapData[x, y, i] = splat[i];
                }
            }
        }
        terrainData.SetAlphamaps(0, 0, splatMapData);
    }
    void NormalizeVector(float[] v)
    {
        float totale = 0;
        for (int i = 0; i < v.Length; i++)
        {
            totale += v[i];
        }
        for (int i = 0; i < v.Length; i++)
        {
            v[i] /= totale;
        }
    }
    public void smooth()
    {
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        int width = terrainData.heightmapResolution;
        for (int i = 0; i < smoothAmmount; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    float avgHeight = heightMap[x, y];
                    List<Vector2> neighbours = GenerateNeighbours(new Vector2(x, y),
                                                                  terrainData.heightmapResolution,
                                                                  terrainData.heightmapResolution);
                    foreach (Vector2 n in neighbours)
                    {
                        avgHeight += heightMap[(int)n.x, (int)n.y];
                    }

                    heightMap[x, y] = avgHeight / ((float)neighbours.Count + 1);
                }
            }
        }
        
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void Perlin()
    {
        float[,] heightMap = getHeightMap();
        for (int x = 60; x < terrainData.heightmapResolution-60; x++)
        {
            for (int y = 60; y < terrainData.heightmapResolution-60; y++)
            {
                heightMap[x, y] +=
                    utile.fBM((x + perlinOffsetX) * perlinXScale,
                    (y + perlinOffsetY) * perlinYScale,
                    perlinOctaves,
                    perlinPersistance) * perlingHeightScale;
            }
        }
        for (int x = 59; x >= 0; x--)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                float a = heightMap[x+1, y];
                heightMap[x, y] += (a * 0.975f); ;
            }
        }
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 59; y >= 0; y--)
            {
                float a = heightMap[x , y+1];
               
                heightMap[x, y] += (a * 0.975f); ;
            }
        }
        for (int x = terrainData.heightmapResolution - 60; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                float a = heightMap[x - 1, y];
                heightMap[x, y] += (a * 0.975f); ;
            }
        }
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = terrainData.heightmapResolution-60; y < terrainData.heightmapResolution; y++)
            {
                float a = heightMap[x , y-1];
                heightMap[x, y] += (a * 0.975f); ;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void MidPointDiplacement()
    {
        float[,] heightMap = getHeightMap();
        int width = terrainData.heightmapResolution - 1;
        int squareSize = width;
        float height = (float)squareSize / 2f * this.height;
        float heightDumpener = (float)Mathf.Pow(2, -1 * roughness);

        int cornorX, cornorY;
        int midX, midY;
        int pmidXL, pmidXR, pmidYU, pmidYD;

        //heightMap[0, 0] = UnityEngine.Random.Range(0f, .2f);
        //heightMap[0,squareSize-1] = UnityEngine.Random.Range(0f, .2f);
        //heightMap[squareSize - 1,0 ] = UnityEngine.Random.Range(0f, .2f);
        //heightMap[squareSize - 1, squareSize - 1] = UnityEngine.Random.Range(0f, .2f);
        while (squareSize > 0)
        {
            for (int x = 0; x < width; x += squareSize)
            {
                for (int y = 0; y < width; y += squareSize)
                {
                    cornorX = x + squareSize;
                    cornorY = y + squareSize;

                    midX = (int)(x + squareSize / 2f);
                    midY = (int)(y + squareSize / 2f);
                    heightMap[midX, midY] = (float)((heightMap[x, y] + heightMap[cornorX, y] + heightMap[x, cornorY] + heightMap[cornorX, cornorY]) / 4f + UnityEngine.Random.Range(-height, height));
                }
            }
            for (int x = 0; x < width; x += squareSize)
            {
                for (int y = 0; y < width; y += squareSize)
                {
                    cornorX = x + squareSize;
                    cornorY = y + squareSize;

                    midX = (int)(x + squareSize / 2f);
                    midY = (int)(y + squareSize / 2f);
                    pmidXR = (int)(midX + squareSize);
                    pmidYU = (int)(midY + squareSize);
                    pmidXL = (int)(midX - squareSize);
                    pmidYD = (int)(midY - squareSize);

                    if (pmidXR >= width - 1 || pmidYU >= width - 1 || pmidXL <= 0 || pmidYD <= 0)
                        continue;

                    heightMap[midX, y] = (float)((heightMap[x, y] + heightMap[cornorX, y] + heightMap[midX, pmidYD] + heightMap[midX, midY]) / 4f + UnityEngine.Random.Range(-height, height));
                    heightMap[x, midY] = (float)((heightMap[x, y] + heightMap[x, cornorY] + heightMap[pmidXL, midY] + heightMap[midX, midY]) / 4f + UnityEngine.Random.Range(-height, height));
                    heightMap[cornorX, midY] = (float)((heightMap[cornorX, cornorY] + heightMap[cornorX, y] + heightMap[pmidXR, midY] + heightMap[midX, midY]) / 4f + UnityEngine.Random.Range(-height, height));
                    heightMap[midX, cornorY] = (float)((heightMap[x, cornorY] + heightMap[cornorX, cornorY] + heightMap[midX, pmidYU] + heightMap[midX, midY]) / 4f + UnityEngine.Random.Range(-height, height));
                }
            }
            squareSize = (int)(squareSize / 2f);
            height *= heightDumpener;
        }
        terrainData.SetHeights(0, 0, heightMap);

    }
    public void Voronnoi()
    {
        float[,] heightMap = getHeightMap();
        for (int i = 0; i < count; i++)
        {
            float peak = UnityEngine.Random.Range(MinHeight, maxHeight);
            Vector2 peakPos = new Vector2(UnityEngine.Random.Range(0, terrainData.heightmapResolution), UnityEngine.Random.Range(0, terrainData.heightmapResolution));
            if (heightMap[(int)peakPos.x, (int)peakPos.y] <= peak)
                heightMap[(int)peakPos.x, (int)peakPos.y] = peak;
            else
                continue;
            float maxDisance = Vector2.Distance(Vector2.zero, new Vector2(terrainData.heightmapResolution, terrainData.heightmapResolution));

            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                for (int y = 0; y < terrainData.heightmapResolution; y++)
                {
                    if (x != peakPos.x || y != peakPos.y)
                    {
                        float distance = Vector2.Distance(peakPos, new Vector2(x, y)) / maxDisance;
                        float h = peak - distance * DropOff - Mathf.Pow(distance, fallOff);
                        if (h > heightMap[x, y])
                            heightMap[x, y] = h;
                    }
                }
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void MultiplePerlinTerrain()
    {
        float[,] heightMap = getHeightMap();
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                foreach (PerlinParameters p in perlinParameters)
                {
                    heightMap[x, y] +=
                     utile.fBM((x + p.mperlinOffsetX) * p.mperlinXScale,
                               (y + p.mperlinOffsetY) * p.mperlinYScale,
                               p.mperlinOctaves,
                               p.mperlinPersistance) * p.mperlingHeightScale;
                }
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void AddNewPerlin()
    {
        perlinParameters.Add(new PerlinParameters());
    }
    public void RemovePerlin()
    {
        List<PerlinParameters> keptPerlinParameters = new List<PerlinParameters>();
        for (int i = 0; i < perlinParameters.Count; i++)
        {
            if (!perlinParameters[i].remove)
            {
                keptPerlinParameters.Add(perlinParameters[i]);
            }
        }
        if (keptPerlinParameters.Count == 0)
        {
            keptPerlinParameters.Add(perlinParameters[0]);
        }
        perlinParameters = keptPerlinParameters;
    }
    public void resetTerrain()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                heightMap[x, y] = 0;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void HeightMap()
    {
        Htexture = new Texture2D(513, 513, TextureFormat.ARGB32, false);
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                float colV = heightMap[x, y]*255f/ terrainHeight;
                Htexture.SetPixel(x, y, new Color(colV, colV, colV));
            }
        }
        Htexture.Apply();
    }
    public void LoadTexture()
    {
        float[,] heightMap = getHeightMap();
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                heightMap[x, y] += heightMapImage.GetPixel((int)(x * heightMapScale.x), (int)(y * heightMapScale.z)).grayscale * heightMapScale.y;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void RandomTerrain()
    {
        float[,] heightMap = getHeightMap();
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                heightMap[x, y] += UnityEngine.Random.Range(randomHeightRange.x, randomHeightRange.y);
                //UnityEngine.Random.Range(randomHeightRange.x, randomHeightRange.y);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    void OnEnable()
    {
        Debug.Log("Initialising Terrain Data");
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
    }
    public enum TagType { Tag=0,Layer=1}
    [SerializeField] int terrainLayer = -1;

     
    //int AddTag(SerializedProperty tagsProp, string a, TagType tag)
    //{
    //    bool found = false;
    //    for (int i = 0; i < tagsProp.arraySize; i++)
    //    {
    //        SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
    //        if (t.stringValue.Equals(a)) { found = true; return i; }
    //    }
    //    if (!found && tag == TagType.Tag)
    //    {
    //        tagsProp.InsertArrayElementAtIndex(0);
    //        SerializedProperty newtagProp = tagsProp.GetArrayElementAtIndex(0);
    //        newtagProp.stringValue = a;
    //    }
    //    else if(!found && tag == TagType.Layer)
    //    {
    //        for (int i = 7; i < tagsProp.arraySize; i++)
    //        {
    //            SerializedProperty newLayer = tagsProp.GetArrayElementAtIndex(i);
    //            if (newLayer.stringValue == "")
    //            {
    //                newLayer.stringValue = a;
    //                return i;
    //            }
    //        }
    //    }

    //    return -1;
    //}

}
