using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct TreeData : IComponentData
{
    public float timeToCraft;
    public float initialTime;
    public int ItemID;
    public Vector3 PlaceOfDrop;
    public Vector3 placeOfShowPanel;

    
    public float retrnePourcentage()
    {
        return initialTime / timeToCraft;
    }
}
