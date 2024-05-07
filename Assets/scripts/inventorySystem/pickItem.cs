using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickItem : MonoBehaviour
{
    [HideInInspector] public item Item;
    int x=0;
    Transform child;
    soundManager manager;
    private void OnEnable()
    {
        manager = FindObjectOfType<soundManager>();
    }

    public void SetItem(item item)
    {
        Item = item;
        //transform.GetComponent<Image>().sprite = item.icon;
        child = transform.GetChild(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject p = other.gameObject;
        if (p.layer == 7)
        {
            playSound.playAudoi(7, manager);
            ParticleSystem destroyP = p.GetComponent<UseableItem>().p2();
            destroyP.transform.position = transform.position;
            destroyP.Play();
            other.transform.GetChild(1).GetChild(3).GetComponent<inventory>().PickItem(Item,1);
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        child.rotation = Quaternion.Euler(-70f, 0f, x++);
    }
}
