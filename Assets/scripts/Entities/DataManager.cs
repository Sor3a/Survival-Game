using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class DataManager : MonoBehaviour
{
    public  GameObject itemToGet;
    EntityManager manager;
    inventoryHolder holder;
    private void Awake()
    {
        holder = FindObjectOfType<inventoryHolder>();
    }
    private void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;   
        
    }
    public void DropItem(Entity e)
    {
        if(manager.HasComponent<TreeData>(e))
        {
            var treeData = manager.GetComponentData<TreeData>(e);

            GameObject b;
            var pos = manager.GetComponentData<Translation>(e).Value;
            if (treeData.PlaceOfDrop != Vector3.zero)
                b = Instantiate(itemToGet, treeData.PlaceOfDrop + (Vector3)pos, Quaternion.identity);
            else
                b = Instantiate(itemToGet, pos, Quaternion.identity);
            var Item = holder.FindItemWithId(treeData.ItemID);

            if (Item)
            {
                GameObject itemModel = Instantiate(Item.model, b.transform);
                itemModel.transform.localPosition = new Vector3(0f, -.5f, 0f);
                b.GetComponent<pickItem>().SetItem(Item);
            }
        }
    }
}
