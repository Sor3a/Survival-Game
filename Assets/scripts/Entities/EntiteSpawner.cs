using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;


public class EntiteSpawner : MonoBehaviour
{
    [SerializeField] GameObject tree,player;
    [SerializeField] List<EntityTreeSpaw> spawnEntitysList;
    BlobAssetStore blob;
    CustomTerrain TerrainSpawn;


    void Awake()
    {
        TerrainSpawn = GetComponent<CustomTerrain>();
        var craftComponent = tree.GetComponentInChildren<craft>();
        float timeToCraft = craftComponent.timeToCraft;
        item Item = craftComponent.Item;
        Vector3 PlaceOfDrop = tree.transform.TransformPoint(craftComponent.placeOFDrope.position);
        Vector3 placeOfShowPanel = craftComponent.placeOfShowPanel.position;

        blob = new BlobAssetStore();
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(tree, settings);


        //for here
        foreach (var item in spawnEntitysList)
        {
            
            for (int i = 0; i < item.numberOfEntites; i++)
            {
                (Quaternion? rotation, Vector3? position) prefabRotationAndPos = new(null, null);

                    
                prefabRotationAndPos = TerrainSpawn.spawnRandom( item.fixY);
  
                if (prefabRotationAndPos != (null, null))
                {
                    var instance = manager.Instantiate(prefab);
                    manager.AddComponentData(instance, new TreeData
                    {
                        timeToCraft = timeToCraft,
                        initialTime = timeToCraft,
                        ItemID = Item.itemID,
                        PlaceOfDrop = PlaceOfDrop,
                        placeOfShowPanel = placeOfShowPanel,
                    });
                    manager.SetComponentData(instance, new Translation
                    {
                        Value = (float3)prefabRotationAndPos.position
                    });
                    manager.SetComponentData(instance, new Rotation
                    {
                        Value = (quaternion)prefabRotationAndPos.rotation
                    });
                }

            }

        }


    }


    private void OnDestroy()
    {
        blob.Dispose();
    }
}

[System.Serializable]
public class EntityTreeSpaw
{
    public GameObject prefab;
    public float fixY;
    public int numberOfEntites;
}