using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryHolder : MonoBehaviour
{
    public List<item> items;
    public List<int> numberOfItems;
    [SerializeField] Transform content;

    private void Awake()
    {
        numberOfItems = new List<int>();
        for (int i = 0; i < items.Count; i++)
        {
            numberOfItems.Add(0);
        }
        //foreach (Transform item in transform)
        //{
        //    item.gameObject.AddComponent<itemIHold>();
            
        //}
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).gameObject.AddComponent<itemIHold>();
        }
    }
    public int numberOfItem(int itemId)
    {
        return numberOfItems[itemId];
    }

    public item FindItemWithId(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemID == id)
                return items[i];
        }
        return null;
    }
}
