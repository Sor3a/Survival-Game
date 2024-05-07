using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerStats : MonoBehaviour
{
    public int damage, dmgBefor;
    public int armor;
    public int health;
    public float eating, force;
    int dmgAadded=0;
    [HideInInspector] public int initialDmg;
    bool isDmgReal = true;
    [SerializeField] Scrollbar healthS, eatingS, forceS;
    float range=0f;


    private void Awake()
    {
        loadingW w = FindObjectOfType<loadingW>();
        if(w)
        LoadingPanel = w.gameObject;
    }
    private void Start()
    {
        initialDmg = damage;
        dmgBefor = damage;
        healthS.value = 0;
        forceS.value = 0;
        eatingS.value = 0;
        changeHealthSize();
    }
    private void Update()
    {
        if (eating > 0.1f)
            eating -= Time.deltaTime*0.5f;
        setScrolls();
        if(range!=0)
        {
            if (eating < range)
            {
                eating += Time.deltaTime * 20f;
                eatingS.size = (eating / 100f) <= 0 ? 0.01f : (eating / 100f);
            }
            else
                range = 0;
        }
        //if (Input.GetKeyDown(KeyCode.X))
        //    getHealth(100);
    }
    public void AddEating(float eatingAdded,int hp)
    {
        range = eatingAdded + eating;
        if (range > 100) range = 100;
        Addhealth(hp);
    }
    void changeHealthSize()
    {
        healthS.size = ((float)health / 100f) <= 0 ? 0.01f : ((float)health / 100f);
    }

    public void GetForce(float f)
    {
        
        force -= Time.deltaTime * f;
        forceS.size = (force / 100f) <= 0 ? 0.01f : (force / 100f);


    }
    public void returneForce()
    {
        force += Time.deltaTime * 20f;
        forceS.size = (force / 100f) > 100f ? 1f : (force / 100f);
        eating -= Time.deltaTime*2;
        eatingS.size = (eating / 100f) <= 0 ? 0.01f : (eating / 100f);
    }
    public void Addhealth(int healthAdded)
    {
        health += healthAdded;
        if (health > 100)
            health = 100;
        changeHealthSize();
    }
    [SerializeField] bool testing;
    GameObject LoadingPanel;
    public void getHealth(int dmg)
    {
        health -= dmg;
        health += armor*dmg/100;
        if (health <= 0)
        { 
            if(!testing)
            {
                if(LoadingPanel)
                LoadingPanel.SetActive(true);
                SceneManager.LoadScene(1);
            } 
        }
        changeHealthSize();
    }
    IEnumerator returnDmg(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        damage -= dmgAadded;
        dmgAadded = 0;
        isDmgReal = true;
    }
    public void FixDmg()
    {
        damage = initialDmg + dmgAadded;
    }
    public void AddDmg(int dmg,float time)
    {
        if (dmg != dmgAadded)
        {
            damage = damage - dmgAadded + dmg;
            isDmgReal = false;
        }
        dmgAadded = dmg;
        StopCoroutine(returnDmg(time));
        StartCoroutine(returnDmg(time));     
    }
    void setScrolls()
    {
        if(eating>0.1f)
        eatingS.size = (eating / 100f) <= 0 ? 0.01f : (eating / 100f);
    }

}
