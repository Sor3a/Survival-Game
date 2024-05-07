using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setItemsInPlayer : MonoBehaviour
{
    swordHolder sh;
    swordPholder sph;
    bowHolder bh;
    bowPholder bph;
    pickaxeHolder ph;
    pickaxePholder pph;
    setStatsInText stats;
    private void Awake()
    {
        ph = FindObjectOfType<pickaxeHolder>();
        pph = FindObjectOfType<pickaxePholder>();
        sph = FindObjectOfType<swordPholder>();
        sh = FindObjectOfType<swordHolder>();
        bh = FindObjectOfType<bowHolder>();
        bph = FindObjectOfType<bowPholder>();
        stats = GetComponent<setStatsInText>();

    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void setHolders()
    {
        if (sh.Item != null)
            sph.setItem(sh.Item);
        else
            sph.DestroyItem();

        if (bh.Item != null)
            bph.setItem(bh.Item);
        else
            bph.DestroyItem();

        if (ph.Item != null)
            pph.setItem(ph.Item);
        else
            pph.DestroyItem();

        stats.fix();
    }

}
