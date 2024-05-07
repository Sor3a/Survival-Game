using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chests : MonoBehaviour
{
    public List<item> items = new List<item>();
    craft c;
    private void OnEnable()
    {
        c = GetComponent<craft>();
        c.Item = returnRandom();
    }
    
    public item returnRandom()
    {
        return items[Random.Range(0, items.Count)];
    }
}
