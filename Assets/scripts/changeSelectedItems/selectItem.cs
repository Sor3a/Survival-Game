using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectItem : MonoBehaviour
{
    [SerializeField] GameObject sword,bow,pickaxe,stick;
    swordPholder holderSword;
    bowPholder holderBow;
    playerStats Pstates;
    pickaxePholder holderPickaxe;
    weaponAttack wp;
    bowAttack ba;
    pickaxeCraft pC;
    private void Awake()
    {
        Pstates = FindObjectOfType<playerStats>();
        holderPickaxe = transform.GetChild(2).GetComponent<pickaxePholder>();
        holderSword = transform.GetChild(0).GetComponent<swordPholder>();
        holderBow = transform.GetChild(1).GetComponent<bowPholder>();
        wp = sword.GetComponent<weaponAttack>();
        ba = bow.transform.GetChild(0).GetComponent<bowAttack>();
        pC = pickaxe.GetComponent<pickaxeCraft>();
    }
    void swordHolderSetDmg()
    {
        Pstates.FixDmg();
        if (holderSword.Item is sword s)
        {
            Pstates.damage += s.damageAdd;
        }
    }

    void bowHolderSetDmg()
    {
        Pstates.FixDmg();
        if (holderBow.Item is bow s)
        {
            Pstates.damage += s.bowDmg;
        }
    }

    void desactiveItems()
    {
        if (holderSword.Item == null && sword.activeSelf == true)
            sword.SetActive(false);
        if (holderBow.Item == null && bow.activeSelf == true)
            bow.SetActive(false);
        if (holderPickaxe.Item == null && pickaxe.activeSelf == true)
            pickaxe.SetActive(false);
    }
    public void FixItem(int itemID)
    {
        if (itemID == 0)
        {
            wp.setMaterial(holderSword.Item);
            if (sword.activeSelf == true)
            {
                swordHolderSetDmg();
            }
        }
        else if (itemID == 1)
        {
            ba.setMaterial(holderBow.Item);
            if (bow.activeSelf == true)
            {
                bowHolderSetDmg();
            }
        }
        else if (itemID == 2)
        {
            pC.setMesh(holderPickaxe.Item);
        }
            

    }
    void setActiveItem(GameObject weapon,int itemID)
    {

        if (weapon.activeSelf == false)
        {
            sword.SetActive(false);
            bow.SetActive(false);
            this.pickaxe.SetActive(false);
            stick.SetActive(false);
            weapon.SetActive(true);
            if(itemID == 0)
            {
                swordHolderSetDmg();
            }
            else if (itemID == 1)
                bowHolderSetDmg();
        }
        else
        {
            weapon.SetActive(false);
            Pstates.FixDmg();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && holderSword.Item!=null)
            setActiveItem(sword, 0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && holderBow.Item != null)
            setActiveItem(bow, 1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && holderPickaxe.Item != null)
            setActiveItem(pickaxe, 2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            setActiveItem(stick, 7);
        desactiveItems();

    }
}
