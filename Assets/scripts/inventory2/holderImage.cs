using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class holderImage : MonoBehaviour
{
    holderWithNumbers selectedHolder;
    public List<holderWithNumbers> holders = new List<holderWithNumbers>();
    List<Image> images = new List<Image>();
    List<int> holdersNumber = new List<int>();
    UseableItem player;
    int index=0;
    inventory inv;
    GameObject panel;
    Color initalColor,initialColor2;
    [SerializeField] Color pickColor;
    soundManager manager;


    private void Awake()
    {
        manager = FindObjectOfType<soundManager>();
        inv = FindObjectOfType<inventory>();
        panel = inv.gameObject;
        CreateNewList();
        player = FindObjectOfType<UseableItem>();
        selectedHolder = holders[0];
        changeIndex(0);
    }

    void CreateNewList()
    {
        foreach (Transform item in transform)
        {
            holderWithNumbers ho = item.gameObject.AddComponent<holderWithNumbers>();
            ho.Tnumber = ho.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            holders.Add(ho);
            images.Add(item.GetChild(1).GetComponent<Image>());
        }
        for (int i = 0; i < holders.Count; i++)
        {
            holdersNumber.Add(0);
        }
        initalColor = holders[0].transform.GetComponent<Image>().color;
        initialColor2 = images[0].color;
    }

    int indexT(item Item)
    {
        int k=-1;
        for (int i = holders.Count-1; i >=0; i--)
        {
            if (holders[i].Item == Item)
                return i;
            if (!holders[i].Item)
                k = i;
        }
        return k;
    }
    public void setItem(item Item)
    {
        int id = indexT(Item);
        if (id == -1)
        {
            //Debug.Log("full");
            return;
        }  
        else
        {
            if (!holders[id].Item)
            {
                images[id].sprite = Item.icon;
                images[id].color = Color.white;
                holders[id].Item = Item;
                if(holders[id].b)
                {
                    Button bb = holders[id].b;
                    bb.onClick.RemoveAllListeners();
                    bb.onClick.AddListener(() =>
                    {
                        if(panel.activeSelf)
                        {
                            inv.PickItem(Item,1);
                            refresh(holders[id]);
                            

                        }
                    });
                }
            }
            holders[id].numberOfItem++;
            holders[id].Tnumber.text = holders[id].numberOfItem.ToString();
        }
    }

    void useItem(item item)
    {
        if (item is potions p)
        {
            player.usePotion(p);
            if (item.itemID == 36)
                playSound.playAudoi(11, manager);
            else
                playSound.playAudoi(12, manager);
        }
    }
    int sizeOfHolders()
    {
        int x=-1;
        for (int i = 0; i < holders.Count; i++)
        {
            if (holders[i].Item)
                x++;
        }
        return x;
    }
    int indexOfHolder(holderWithNumbers selectedHolder)
    {
        int x = 0;
        for (int i = 0; i < holders.Count; i++)
        {
            if (selectedHolder.Item == holders[i].Item)
                return x;
            x++;
        }
        return -1;
    }
    void refresh(holderWithNumbers selectedHolder)
    {
       // Debug.Log(selectedHolder.Item.name);
        if (selectedHolder.numberOfItem > 0)
        {
            selectedHolder.numberOfItem--;
            selectedHolder.Tnumber.text = selectedHolder.numberOfItem.ToString();
        }
            
        if (selectedHolder.numberOfItem <= 0)
        {
            int a = sizeOfHolders() ;
            int k = indexOfHolder(selectedHolder);
            if (k!=-1)
            {
                //Debug.Log("k =" +k);
                for (int i = k; i < a; i++)
                {
                    images[i].sprite = images[i + 1].sprite;
                    holders[i].Item = holders[i + 1].Item;
                    holderWithNumbers s = holders[i];
                    holders[i].numberOfItem = holders[i + 1].numberOfItem;
                    holders[i].Tnumber.text = holders[i + 1].Tnumber.text;
                    //Debug.Log(holders[i].Item.name);
                    holders[i].b.onClick.RemoveAllListeners();
                    holders[i].b.onClick.AddListener(() =>
                    {
                        if (panel.activeSelf)
                        {
                            inv.PickItem(s.Item,1);
                            refresh(s);

                        }
                    });
                    //holders[i].Item = 
                }
                holders[a].DestroyItem();
                images[a].sprite = null;
                images[a].color = initialColor2;
                holders[a].numberOfItem = 0;
                holders[a].b.onClick.RemoveAllListeners();
                holders[a].Tnumber.text = "";
                //holders = temp;
                if (holders[index])
                    this.selectedHolder = holders[index];
                else
                    this.selectedHolder = holders[--index];
            }
        }
            
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(selectedHolder.Item)
            {
                useItem(selectedHolder.Item);
                refresh(selectedHolder);
            }
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            
            if (index + 1< holders.Count)
            {
                int a = index + 1;
                changeIndex(a);
            }
            else
            {
                changeIndex(0);
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (index - 1>=0)
            {
                int a = index - 1;
                changeIndex(a);
            }

            else
            {
                int a = holders.Count - 1;
                changeIndex(a);
            }
        }

    }
    void changeIndex(int a)
    {
        holders[index].image.color = initalColor;
        index = a;
        selectedHolder = holders[index];
        holders[index].image.color = pickColor;
    }

}
