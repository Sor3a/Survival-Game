using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemIHold : MonoBehaviour
{
    public item item = null;
    inventory  inv;
    Image image;
    Color initialColor;
    private void Awake()
    {

        inv = FindObjectOfType<inventory>();
        image = transform.GetChild(2).GetComponent<Image>();
        initialColor = image.color;
        gameObject.SetActive(false);
    }

    public void setitem(item Item)
    {
        if(Item)
        {
            gameObject.SetActive(true);
            item = Item;
            image.sprite = Item.icon;
            image.color = Color.white;
            Button b = GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => inv.UseItem(Item));
            Button b1 = transform.GetChild(1).GetComponent<Button>();
            b1.onClick.RemoveAllListeners();
            b1.onClick.AddListener(() => inv.DropeItem(item));
        }
    }
    public void DestroyItem()
    {
        item = null;
        image.sprite = null;
        image.color = initialColor;
        gameObject.SetActive(false);
    }

}
