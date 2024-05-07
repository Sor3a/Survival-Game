using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class craft : MonoBehaviour
{
    public float timeToCraft;
    float initailTimeTOcraft;
    public item Item;
    public GameObject Enemy;
    
    [SerializeField] GameObject itemToGet;
    public Transform placeOFDrope;
    public Transform placeOfShowPanel;
    public string Name;

    private void OnEnable()
    {
        initailTimeTOcraft = timeToCraft;
    }
    public float pourcentage()
    {
        return timeToCraft / initailTimeTOcraft;
    }
    public void dropeItem()
    {
        if(Item)
        {
            GameObject b;
            if (placeOFDrope != null)
                b = Instantiate(itemToGet, placeOFDrope.position, Quaternion.identity);
            else
                b = Instantiate(itemToGet, transform.position, Quaternion.identity);
            GameObject itemModel = Instantiate(Item.model, b.transform);
            itemModel.transform.localPosition = new Vector3(0f, -.5f, 0f);
            b.GetComponent<pickItem>().SetItem(Item);
        }
        else if(Enemy)
        {
            GameObject b = Instantiate(Enemy, placeOFDrope.position, Quaternion.identity);
        }
    }

}
