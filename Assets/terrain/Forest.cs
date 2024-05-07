using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    [System.Serializable]
    public class forst
    {
        public GameObject tree;
        public int number;
        public float Adjustment;
    }
    [SerializeField] forst[] stuff;
    [SerializeField] float range;
    [HideInInspector] public TerrainData terrainData;
    [SerializeField] LayerMask layer, terrainLayere;
    [SerializeField] float WaterHeight;
    [SerializeField] int forstNumber;
    private void Awake()
    {
        terrainData = GetComponent<Terrain>().terrainData;
    }

    GameObject spawnElement(float[,] HeightMap,GameObject element,float Adjustment,float range1X,float range2X, float range1Y, float range2Y)
    {
        GameObject b = null;
        int x = (int)Random.Range(range1X, range2X);
        int z = (int)Random.Range(range1Y, range2Y);
        float y = HeightMap[x, z];
        int XHV = (int)(x / (float)terrainData.heightmapResolution * terrainData.size.x);
        int ZHV = (int)(z / (float)terrainData.heightmapResolution * terrainData.size.z);
        Vector3 position = new Vector3(ZHV, y * terrainData.size.y + 1000f, XHV);
        if (Physics.BoxCast(position, element.transform.localScale, -Vector3.up, Quaternion.identity, Mathf.Infinity, layer))
            return null;
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(position, -Vector3.up, out hit, Mathf.Infinity, terrainLayere))
            {
                //float stepness = terrainData.GetSteepness(hit.point.x / terrainData.size.x, hit.point.z / terrainData.size.z);
                if (((hit.point.y / terrainData.size.y) > WaterHeight))
                {

                    b = Instantiate(element, hit.point + new Vector3(0, Adjustment, 0), Quaternion.identity);
                    Transform stuffe = b.transform;
                    stuffe.up = hit.normal;
                    stuffe.RotateAround(stuffe.position, stuffe.up, UnityEngine.Random.Range(0, 360f));

                    stuffe.parent = transform;
                }
                else
                    return null;

            }

        }
        return b;
    }
    float change(float number,int a) // same size on x and on y that's why
    {
        if(a==0)
        return (number *(float) terrainData.heightmapResolution /(float) terrainData.size.x);
        else
            return (number * (float)terrainData.heightmapResolution / (float)terrainData.size.z);
    }
    public void spawnForest()
    {
        if(terrainData == null)
            terrainData = GetComponent<Terrain>().terrainData;
        float initialRange = range;
        float[,] HeightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        for (int k = 0; k < forstNumber; k++)
        {
            range = initialRange;
            GameObject MainElement = null;
            while (MainElement == null)
            {
                
                MainElement = spawnElement(HeightMap, stuff[0].tree, stuff[0].Adjustment, 10f, terrainData.heightmapResolution - 10f, 10f, terrainData.heightmapResolution - 10f);
               // Debug.Log("gfd");
            }
            Vector3 pos = MainElement.transform.position;
            for (int j = 0; j < stuff.Length; j++)
            {
                forst a = stuff[j];
                for (int i = 0; i < a.number; i++)
                {
                    if (!(pos.x - range < 2 || pos.x + range > terrainData.size.x || pos.z - range < 2 || pos.z + range > terrainData.size.z))
                    {
                        GameObject e = spawnElement(HeightMap, a.tree, a.Adjustment, change(pos.x - range, 0), change(pos.x + range, 0), change(pos.z - range, 1), change(pos.z + range, 1));
                        if (e == null)
                            i--;
                    }
                    //else
                    //{
                    //    i--;
                    //}
                }
            }
        }  
    }
}
