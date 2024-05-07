using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "item", menuName = "items", order = 1)]
public class item : ScriptableObject
{
    public int itemID;
    public Sprite icon;
    public bool useable;
    public GameObject model;
    public string text;
}
