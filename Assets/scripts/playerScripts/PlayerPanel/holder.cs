using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class holder : MonoBehaviour
{
     public item Item = null;
    inventory inv;
    [HideInInspector] public Image image;
     public Button b;
    setItemsInPlayer set;
    Image childImage;
    [SerializeField] bool setInOrginal;
    [SerializeField] int childNumber; // the child to the parent getchild(childNumber)
    Color initialC;
    private void Awake()
    {
        if(setInOrginal)
        {
            childImage = transform.GetChild(childNumber).GetComponent<Image>();
            initialC = childImage.color;
        }

        set = FindObjectOfType<setItemsInPlayer>();
        setComp();
    }
    void setComp()
    {
        inv = FindObjectOfType<inventory>();
        image = GetComponent<Image>();
        b = GetComponent<Button>();
        set = FindObjectOfType<setItemsInPlayer>();
        if (setInOrginal)
        {
            childImage = transform.GetChild(childNumber).GetComponent<Image>();
            initialC = childImage.color;
        }
    }
    public void setItem(item item)
    {
        if (inv == null)
            setComp();
        Item = item;
        if(!setInOrginal)
        image.sprite = item.icon;
        else
        {
            if(item)
            childImage.sprite = item.icon;
            childImage.color = Color.white;
        }
        if (b != null)
        {
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() =>
            {
                pickItem();
                if (item is armor Armore)
                    FindObjectOfType<playerStats>().armor = 0;
                set.setHolders();
                b.onClick.RemoveAllListeners();
            });
        }
    }
    public void pickItem()
    {
        inv.PickItem(Item,1);
        DestroyItem();
    }
    public void DestroyItem()
    {
        Item = null;
        if (!setInOrginal)
            GetComponent<Image>().sprite = null;
        else
        {
           // childImage.sprite = null;
            childImage.color = initialC;
        }


    }
}
