using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createStuff : MonoBehaviour
{
    createSystemHolder holder1;
    createSystemHolder holder2;
    createSystemHolder holder3;
    inventoryHolder inv;
    [SerializeField] List<item> possibleCreatedItems;
    [SerializeField] List<GameObject> buttons;
    [SerializeField] GameObject panel, buttonPrefab, showHowToCreatePanel;
    int[,] a = new int[,] { { 24, 2, 28, 2, 13, 1 }, { 23, 2, 17, 2, 14, 1 }, { 25, 2, 18, 2, 15, 1 }, { 21, 1, 20, 2, 1, 1 },//potions
    { 20, 3, 33, 3, 0, 1 },{ 0, 1, 32, 4, 4, 1 },{ 4, 1, 31, 3, 5, 1 },//swords
    { 20, 3, 33, 2, 7, 1 },{ 7, 1, 32, 3, 8, 1 },{ 8, 1, 31,3, 9, 1 },//bow
    { 20, 2, 33, 3, 10, 1 },{ 10, 1, 32, 4, 11, 1 },{ 11, 1, 31, 3, 12, 1 },//pix
    { 20, 6, 33, 6, 2, 1 },{ 2, 1, 32, 4, 3, 1 },{ 3, 1, 31, 4, 6, 1 },//armor
    {27,3,16,2,33,1 },{29,5,30,2,32,1},{19,3,22,2,31,1},//meatls
    {16,2,20,5,34,15},//arrow
    {22,5,19,2,35,1},//end
    };//first item - number1-secondItem - number2-creation-number

    private void Awake()
    {
        inv = FindObjectOfType<inventoryHolder>();
        holder1 = transform.GetChild(0).GetComponent<createSystemHolder>();
        holder2 = transform.GetChild(1).GetComponent<createSystemHolder>();
        holder3 = transform.GetChild(2).GetComponent<createSystemHolder>();
        manager = FindObjectOfType<soundManager>();
        createPanel();

    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    int findComv(item item)
    {
        int j = 0;
        for (int i = 0; i < possibleCreatedItems.Count; i++)
        {
            if (item.itemID == a[i, 4])
            {
                j = i;
                return j;
            }
        }
        return j;
    }//find how to create item in array
    void createPanel()
    {
        // Debug.Log(findComv(possibleCreatedItems[13]));

        panel.SetActive(true);
        for (int i = 0; i < possibleCreatedItems.Count; i++)
        {
            int k = findComv(possibleCreatedItems[i]);
            //GameObject button = Instantiate(buttonPrefab);
            //button.transform.SetParent(panel.transform);
            GameObject button = buttons[i];
            button.AddComponent<addToCreatePanel>();
            addToCreatePanel b = button.GetComponent<addToCreatePanel>();
            b.setItem(possibleCreatedItems[i], holder3, showHowToCreatePanel);
            b.setStuff(findItemWithId(a[k, 0]), a[k, 1], findItemWithId(a[k, 2]), a[k, 3], findItemWithId(a[k, 4]));
            button.layer = 6;
        }
        panel.SetActive(false);

    }
    public item findItemWithId(int ID)
    {
        foreach (item item in inv.items)
        {
            if (item.itemID == ID)
                return item;
        }
        return null;
    }
    public void check()
    {
        if (holder3.Item != null)
        {
            int b = findComv(holder3.Item);
            checker(a[b, 0], a[b, 1], a[b, 2], a[b, 3], a[b, 4], a[b, 5]);

        }
    }
    soundManager manager;
    void checker(int ID1, int number1, int ID2, int number2, int ID, int n)
    {
        if (holder1.Item != null && holder2.Item != null)
        {
            if (holder1.Item.itemID == ID1 && holder1.numberOfItem - number1 >= 0 && holder2.Item.itemID == ID2 && holder2.numberOfItem - number2 >= 0)
            {
                playSound.playAudoi(9, manager);
                for (int i = 0; i < n; i++)
                {
                    holder3.AddItem(findItemWithId(ID));
                    holder1.getitems(number1);
                    holder2.getitems(number2);
                }
            }
        }
    }
}
