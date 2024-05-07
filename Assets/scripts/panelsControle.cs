using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelsControle : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel, CreatePanel, PlayerPanel,itemsPanel,showHowToCreatePanel;
    //setItemsInPlayer set;
    KeyCode e, i, j;

    private void Start()
    {
        Cursor.visible = false;
    }
    void Dvis()
    {
        Cursor.visible = false;
    }
    void vis()
    {
        Cursor.visible = true;
    }
    public void setKeys(KeyCode ee,KeyCode ii, KeyCode jj)
    {
        e = ee;
        i = ii;
        j = jj;
    }
    public bool isPanelOpen()
    {
        return inventoryPanel.activeSelf || CreatePanel.activeSelf || PlayerPanel.activeSelf;
    }
    void gameobj(GameObject obj)
    {
        
        if (obj.activeSelf == true)
        {
            obj.SetActive(false);
            if (!isPanelOpen())
                Dvis();
        }
        else
        {
            obj.SetActive(true);
            vis();
        }
            
    }
    public bool canAnimate()
    {
        if (inventoryPanel.activeSelf || CreatePanel.activeSelf || PlayerPanel.activeSelf || itemsPanel.activeSelf || showHowToCreatePanel.activeSelf)
            return false;
        else
            return true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(e))
            gameobj(inventoryPanel);
        else if (Input.GetKeyDown(i))
        {
            if (PlayerPanel.activeSelf == true)
            {
                PlayerPanel.SetActive(false);
                if (!isPanelOpen())
                Dvis();
                //if (set == null)
                //    set = FindObjectOfType<setItemsInPlayer>();

                //set.setHolders();

            }
            else
            {
                vis();
                CreatePanel.SetActive(false);
                PlayerPanel.SetActive(true);
                itemsPanel.SetActive(false);
                showHowToCreatePanel.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(j))
        {
            if (CreatePanel.activeSelf == true)
            {
                CreatePanel.SetActive(false);
                showHowToCreatePanel.SetActive(false);
                itemsPanel.SetActive(false);
                if (!isPanelOpen())
                    Dvis();
            }
            else
            {
                vis();
                itemsPanel.SetActive(true);
                CreatePanel.SetActive(true);
                //if (set == null)
                //    set = FindObjectOfType<setItemsInPlayer>();
                //if (set != null)
                //    set.setHolders();
                PlayerPanel.SetActive(false);
            }

        }
    }
}
