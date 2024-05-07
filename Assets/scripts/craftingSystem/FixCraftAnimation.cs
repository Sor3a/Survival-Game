using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCraftAnimation : MonoBehaviour
{
    pickaxeCraft pCraft;
    soundManager manager;
    private void Awake()
    {
        manager = FindObjectOfType<soundManager>();
        pCraft = FindObjectOfType<pickaxeCraft>();
    }
    public void craftItem()
    {
        pCraft.Craft();
        
    }
    void playSoun()
    {
        playSound.playAudoi(1, manager);
    }
}
