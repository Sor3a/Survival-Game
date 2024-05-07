using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class exchangeItems
{

    public item firstItem;
    public int numberOfItemsToGive;
    public item resultItem;
    public int numberOfItemsToGet;



}
public class exchange : MonoBehaviour
{
    [SerializeField] Image[] holder1;
    [SerializeField] TextMeshProUGUI[] holder1Text;
    [SerializeField] Button[] holder2;
    [SerializeField] TextMeshProUGUI[] holder2Text;
    public GameObject panel;
    [SerializeField] item i;
    [SerializeField] createSystemHolder Item1,ItemResult;
    inventory inv;

    List<exchangeItems> fixedExchange = new List<exchangeItems>();
    exchangeList exListe;
    List<exchangeItems> itemsT()
    {
        List<exchangeItems> temp = new List<exchangeItems>();
        while (temp.Count<3)
        {
            bool canAdd = true;
            exchangeItems item = exListe.items[Random.Range(0, exListe.items.Count)];
            for (int i = 0; i < temp.Count; i++)
            {
                if (item == temp[i])
                    canAdd = false;
            }
            if (canAdd)
                temp.Add(item);
        }
        return temp; 
    }

    private void Start()
    {
        panel.SetActive(false);
        StartCoroutine(changeItems());
    }
    private void OnEnable()
    {
        inv = FindObjectOfType<inventory>();
        exListe = FindObjectOfType<exchangeList>();
        fixedExchange = itemsT();
        manager = FindObjectOfType<soundManager>();
    }
    void ItemsToUI()
    {
            
        for (int i = 0; i < fixedExchange.Count; i++)
        {
            holder1[i].sprite = fixedExchange[i].firstItem.icon;
            
            holder1Text[i].text = fixedExchange[i].numberOfItemsToGive.ToString();
            holder2[i].transform.GetChild(1).GetComponent<Image>().sprite = fixedExchange[i].resultItem.icon;
            holder2Text[i].text = fixedExchange[i].numberOfItemsToGet.ToString();

            int copy = i;
            holder2[i].onClick.AddListener(() => { ItemResult.SetitemHolder3(fixedExchange[copy].resultItem); setChecker(fixedExchange[copy]); });
        }
    }
    void setChecker(exchangeItems ex)
    {
        this.ex = ex;
    }
    IEnumerator changeItems()
    {
        yield return new WaitForSeconds(400f);
        fixedExchange = itemsT();
        StartCoroutine(changeItems());
    }
    exchangeItems findItem(item Item)
    {
        for (int i = 0; i < fixedExchange.Count; i++)
        {
            if (fixedExchange[i].resultItem == Item)
                return fixedExchange[i];
        }
        return null;
    }
    exchangeItems ex;
    soundManager manager;
    public void check()
    {
        if(ItemResult.Item)
        {
            //= findItem(ItemResult.Item);
            if (Item1.numberOfItem >= ex.numberOfItemsToGive && Item1.Item== ex.firstItem)
            {
                playSound.playAudoi(9, manager);
                Item1.getitems(ex.numberOfItemsToGive);
                for (int i = 0; i < ex.numberOfItemsToGet; i++)
                    ItemResult.AddItem(ItemResult.Item);
            }
        }
    }

    public void showPanel()
    {
        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
            ItemsToUI();
            inv.setHolderShop(Item1, panel);
            Cursor.visible = true;
        }
        else
        {
            panel.SetActive(false);
            Cursor.visible = false;
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            panel.SetActive(false);
            Cursor.visible = false;

        }
    }
}
