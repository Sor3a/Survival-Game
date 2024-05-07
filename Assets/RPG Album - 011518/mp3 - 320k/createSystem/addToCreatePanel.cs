using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class addToCreatePanel : UIchecker
{
    Image image;
    Button b;
    private item item1, item2, resultItem;
    private int item1N, item2N;
    GameObject panel;
    private void Awake()
    {
        
        image =transform.GetChild(0).GetComponent<Image>();
        if (!image)
            image = GetComponent<Image>();
        b = GetComponent<Button>();

    }
    private void Start()
    {
        panel.SetActive(false);
    }

    public void setStuff(item i1, int iN1, item i2, int iN2, item createdItem)
    {
        item1 = i1;
        item2 = i2;
        item1N = iN1;
        item2N = iN2;
        resultItem = createdItem;
    }

    void setPanel(GameObject Image, item i1, int N1)
    {

        Image.GetComponent<Image>().sprite = i1.icon;
        Image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = N1.ToString();
    }
    void setText(GameObject textFiled)
    {
        textFiled.GetComponent<TextMeshProUGUI>().text = resultItem.text;
    }
    public void setItem(item item, createSystemHolder holder, GameObject panele)
    {
        panel = panele;
        image.sprite = item.icon;
        b.onClick.AddListener(() => { holder.SetitemHolder3(item); panel.SetActive(false); });
        panel.SetActive(false);
    }
    private void Update()
    {
        if (UIIsOn() == gameObject)
        {
            //if (panel.activeSelf == false)
            {
                panel.SetActive(true);
                setPanel(panel.transform.GetChild(0).gameObject, item1, item1N);
                setPanel(panel.transform.GetChild(1).gameObject, item2, item2N);
                setText(panel.transform.GetChild(2).gameObject);
            }

        }
        if (UIIsOn() == null)
            panel.SetActive(false);
    }
}
