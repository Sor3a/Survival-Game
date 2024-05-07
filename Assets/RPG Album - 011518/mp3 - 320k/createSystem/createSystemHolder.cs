using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class createSystemHolder : MonoBehaviour
{
    [HideInInspector] public int numberOfItem=0,neededNumber = 0;
    [HideInInspector] public item Item;
    [SerializeField] inventory inv;
    Image Image;
    Button b;
    TextMeshProUGUI text;
    [SerializeField] bool imageInChild = false;

    private void Awake()
    {
        if (!inv)
            inv = FindObjectOfType<inventory>();
        b = GetComponent<Button>();
        if(!imageInChild)
        Image = GetComponent<Image>();
        else
        {
            Image = transform.GetChild(1).GetComponent<Image>();
        }
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void AddItem(item item)
    {

        if (item != Item)
        {
            if (Item)
                useItem();
            Item = item;
            Image.sprite = item.icon;
            Image.color = Color.white;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => useItem());
            numberOfItem = 0;
        }
        numberOfItem++;
        text.text = numberOfItem.ToString();
    }
    public void SetitemHolder3(item item)
    {
        if(numberOfItem>0)
        {

                inv.PickItem(Item, numberOfItem);
            
        }
        if (Item != item)
        {
            Item = item;
            Image.sprite = item.icon;
            Image.color = Color.white;
            numberOfItem = 0;
            text.text = numberOfItem.ToString();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => useItem());
        }
    }
    void useItem()
    {
        if (!inv)
            inv = FindObjectOfType<inventory>();
        if (numberOfItem > 0)
        {
            inv.PickItem(Item, numberOfItem);    
            numberOfItem = 0;
            text.text = numberOfItem.ToString();
            if (numberOfItem <= 0)
                DestroyItem();
        }
    }
    public void getitems(int number)
    {
        numberOfItem = numberOfItem - number;
        text.text = numberOfItem.ToString();
        if (numberOfItem <= 0)
            DestroyItem();
    }
    void DestroyItem()
    {
        b.onClick.RemoveAllListeners();
        Item = null;
        Image.sprite = null;
        Image.color = new Color(0, 0, 0, 0);
        text.text = "";
    }
}
