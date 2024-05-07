using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class setStatsInText : MonoBehaviour
{
   // [SerializeField] armorHolder armor;
    [SerializeField] swordHolder sword;
    [SerializeField] bowHolder bow;
    [SerializeField] pickaxeHolder axe;
    [SerializeField] TextMeshProUGUI damage, bowdmg, craftingForce, armore;
    playerStats stats;
    private void Awake()
    {
        stats = FindObjectOfType<playerStats>();
    }

    public void fix()
    {
        if (!sword.Item)
            damage.text = "Damage : " + "13";
        else
        {
            if (sword.Item is sword s)
                damage.text = "Damage : " + (s.damageAdd + stats.initialDmg).ToString();
        }
        if (!axe.Item)
            craftingForce.text = "Crafting Force : " + "6";
        else
        {
            if (axe.Item is pickaxe s)
                craftingForce.text = "Crafting Force : " + (s.DestroySpeed * 20).ToString();
        }
        armore.text = "Armor : " + stats.armor;

        if (!bow.Item)
            bowdmg.text = "Bow Damage: " + "0";
        else
        {
            if (bow.Item is bow s)
                bowdmg.text = "Bow Damage : " + (s.bowDmg + stats.initialDmg).ToString();
        }
    }
    private void OnEnable()
    {
        fix();
    }
}
