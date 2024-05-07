using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class inventory : MonoBehaviour
{
    inventoryHolder holder;
    [SerializeField] GameObject playerPanel,CreatePanel,itemToGet;
    [HideInInspector] public GameObject ConvertStuff;
    [HideInInspector] public createSystemHolder holderShop;
    [SerializeField] Transform canvas;
    Transform player;
    holderImage holder3;
    [SerializeField] Transform content;
    private void Awake()
    {
        holder3 = FindObjectOfType<holderImage>();
        holder = GetComponent<inventoryHolder>();
        player = FindObjectOfType<playerMVT>().transform;
        manager = FindObjectOfType<soundManager>();

    }
    public void setHolderShop(createSystemHolder h,GameObject panel)
    {
        holderShop = h;
        ConvertStuff = panel;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PickItem(holder.items[1], 1);
            PickItem(holder.items[2], 1);
            PickItem(holder.items[3], 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PickItem(holder.items[4], 1);
            PickItem(holder.items[5], 1);
            PickItem(holder.items[6], 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            PickItem(holder.items[7], 1);
            PickItem(holder.items[8], 1);
            PickItem(holder.items[9], 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            PickItem(holder.items[10], 1);
            PickItem(holder.items[11], 1);
            PickItem(holder.items[12], 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            PickItem(holder.items[13], 1);
            PickItem(holder.items[14], 1);
            PickItem(holder.items[15], 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            PickItem(holder.items[16], 1);
            PickItem(holder.items[17], 1);
            PickItem(holder.items[34], 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            PickItem(holder.items[19], 1);
            PickItem(holder.items[20], 1);
            PickItem(holder.items[21], 1);
        }
    }
    soundManager manager;
    public void DropeItem(item Item)
    {
        playSound.playAudoi(8, manager);
        GameObject b = Instantiate(itemToGet, player.position + player.forward, Quaternion.identity);
        b.transform.forward = (player.position - b.transform.position).normalized;
        GameObject itemModel = Instantiate(Item.model, b.transform);
        itemModel.transform.localPosition = new Vector3(0f,-.2f,0f);
        b.GetComponent<pickItem>().SetItem(Item);
        holder.numberOfItems[Item.itemID]--;
        refreshStats(Item);
        if (holder.numberOfItems[Item.itemID] <= 0)
            DestroyItem(Item);
    }
    void SetItem(item Item)
    {
        //bool itemIsSet = false;
        //int i = 0;
        //while (!itemIsSet)
        //{

                itemIHold hold = content.GetChild(a++).GetComponent<itemIHold>();
                if (hold.item == null)
                {
                    hold.setitem(Item);
                    //itemIsSet = true;
                   
                }
        //    else
        //        itemIsSet = true;
        //}

    }
    itemIHold findItemHolder(item Item)
    {
        itemIHold holder;
        int i = -1;
        while (content.GetChild(++i).GetComponent<itemIHold>().item != Item && i+1< content.childCount)
        { }
        holder = content.GetChild(i).GetComponent<itemIHold>();
        return holder;
    }
    void refreshStats(item Item)
    {
        findItemHolder(Item).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = holder.numberOfItems[Item.itemID].ToString();
    }
    int a = 0;
    public void PickItem(item Item, int number)
    {
        if (a + 1 < content.childCount)
        {
            if (holder.numberOfItems[Item.itemID] <= 0)
                SetItem(Item);
            holder.numberOfItems[Item.itemID] += number;
            refreshStats(Item);
        }
        else
        {
            if (holder.numberOfItems[Item.itemID] > 0)
            {
                holder.numberOfItems[Item.itemID] += number;
                refreshStats(Item);
            }
        }
    }

    public void UseItem(item Item)
    {
        if (playerPanel.activeSelf == true)
        {
            if (Item is armor Armor)
            {
                armorHolder holde = FindObjectOfType<armorHolder>();
                if (holde.Item != Item)
                {
                    if (holde.Item != null)
                        PickItem(holde.Item,1);
                    holde.set(Item);
                    holder.numberOfItems[Item.itemID]--;
                }
            }
            else if(Item is sword Sword)
            {
                swordHolder holde = FindObjectOfType<swordHolder>();
                if (holde.Item != Item)
                {
                    if (holde.Item != null)
                        PickItem(holde.Item,1);
                    holde.set(Item);
                    holder.numberOfItems[Item.itemID]--;
                }
            }
            else if(Item is bow Bow)
            {
                bowHolder holde = FindObjectOfType<bowHolder>();
                if (holde.Item != Item)
                {
                    if (holde.Item != null)
                        PickItem(holde.Item,1);
                    holde.set(Item);
                    holder.numberOfItems[Item.itemID]--;
                }
            }
            else if (Item is pickaxe Pickaxe)
            {
                pickaxeHolder holde = FindObjectOfType<pickaxeHolder>();
                if (holde.Item != Item)
                {
                    if (holde.Item != null)
                        PickItem(holde.Item,1);
                    holde.set(Item);
                    holder.numberOfItems[Item.itemID]--;
                }
            }
        }//playerPanel ( health + armor)
        else if(CreatePanel.activeSelf == true && !Item.useable)
        {
            createSystemHolder holder1 = CreatePanel.transform.GetChild(0).GetComponent<createSystemHolder>();
            createSystemHolder holder2 = CreatePanel.transform.GetChild(1).GetComponent<createSystemHolder>();
            if (holder1.Item == null || holder1.Item == Item)
            {
                holder1.AddItem(Item);
                holder.numberOfItems[Item.itemID]--;
            }
            else if(holder2.Item == null || holder2.Item == Item)
            {
                holder2.AddItem(Item);
                holder.numberOfItems[Item.itemID]--;
            }
        }//createPanelAdd
        else if(ConvertStuff)
        {
            if(ConvertStuff.activeSelf == true)
            {
                holderShop.AddItem(Item);
                holder.numberOfItems[Item.itemID]--;
            }
        }
        if (Item.useable )
        {
            //if (Item.itemID == 35)
            //    EngGame();
            if(ConvertStuff)
            {
                if(ConvertStuff.activeSelf == false)
                {
                    holder.numberOfItems[Item.itemID]--;
                    holder3.setItem(Item);
                }
            }
            else
            {
                holder.numberOfItems[Item.itemID]--;
                holder3.setItem(Item);
            }

            //if(Item is potions p)
            //{
            //    player.GetComponent<UseableItem>().usePotion(p);
            //}
        }
        refreshStats(Item);
        if (holder.numberOfItems[Item.itemID] <= 0)
            DestroyItem(Item);
    }
    public void UseArrow(item Item)
    {
        holder.numberOfItems[Item.itemID]--;
        refreshStats(Item);
        if (holder.numberOfItems[Item.itemID] <= 0)
            DestroyItem(Item);
    }
    void EngGame()
    {
        FindObjectOfType<playerMVT>().MoveToEnd();
    }


    public void DestroyItem(item Item)
    {
        for (int i = 0; i < a+1; i++)
        {
            itemIHold hold = content.GetChild(i).GetComponent<itemIHold>();
            if (hold.item == Item)
            {
                if (content.GetChild(i + 1).GetComponent<itemIHold>().item == null)
                    hold.DestroyItem();
                else
                {
                    for (int j = i; j < a; j++)
                    {
                        itemIHold holde = content.GetChild(j).GetComponent<itemIHold>();
                        if (content.GetChild(j + 1).GetComponent<itemIHold>().item == null)
                        {
                            holde.DestroyItem();
                            j = 50;
                        }
                        else
                        {
                            holde.setitem(content.GetChild(j + 1).GetComponent<itemIHold>().item);
                            refreshStats(holde.item);
                        }
                    }
                }
                i = 50;
            }
        }
    }//destroy Item after finished
}
