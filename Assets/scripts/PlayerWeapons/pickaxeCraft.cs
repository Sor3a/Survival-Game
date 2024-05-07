using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickaxeCraft : MonoBehaviour
{
    Transform player;
    [SerializeField] float distanceToCraft;
    [SerializeField] LayerMask craftedLayer;
    Animator animator;
    Transform cam;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material[] nrmlMat, goldMat, DiMat;
    playerMVT mvt;
    panelsControle pContole;
    [SerializeField] ParticleSystem[] craftingEffect;
    [SerializeField] ParticleSystem[] craftingEffectIron;
    [SerializeField] ParticleSystem[] craftingEffectNotTree;
    [SerializeField] ParticleSystem smokeEffect;
    soundManager manager;
    private void Awake()
    {
        manager = FindObjectOfType<soundManager>();
        pContole = FindObjectOfType<panelsControle>();
        mvt = FindObjectOfType<playerMVT>();
        cam = Camera.main.transform;
        animator =transform.GetChild(0).GetComponent<Animator>();
        player = FindObjectOfType<playerStats>().transform;
        gameObject.SetActive(false);
    }

    public void setMesh(item Item)
    {
        if (Item.itemID == 10)
            mesh.materials = nrmlMat;
        else if (Item.itemID == 11)
            mesh.materials = goldMat;
        else if (Item.itemID == 12)
            mesh.materials = DiMat;
    }
    void startCrafting()
    {

        if (!animator.GetBool("crafting"))
            animator.SetBool("crafting", true);
        
    }

    void PlayPartical(ParticleSystem p, Vector3 pos)
    {
        p.transform.position = pos;
        p.Play();
    }
    public void Craft()
    {
        Ray r = new Ray(player.position, cam.forward);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, distanceToCraft, craftedLayer))
        {
           // Debug.Log("we start Crafting");
            GameObject facedGameObject = hit.collider.gameObject;
            craft c = facedGameObject.GetComponent<craft>();
            playWithScale(facedGameObject.transform);
            playSound.playAudoi(0, manager);
            //play the effect
            if (c)
            {
                
                if (c.Item)
                {
                    if (c.Item.itemID == 20 && facedGameObject.GetComponent<chests>() == null)
                    {
                        PlayPartical(craftingEffect[Random.Range(0, craftingEffect.Length)], hit.point);
                    }
                    else if (c.Item.itemID == 33 && facedGameObject.GetComponent<chests>() == null)
                    {
                        PlayPartical(craftingEffectIron[Random.Range(0, craftingEffectIron.Length)], hit.point);
                    }
                    else
                    {
                        PlayPartical(craftingEffectNotTree[Random.Range(0, craftingEffectNotTree.Length)], hit.point);
                    }
                }
                else
                {
                    PlayPartical(craftingEffectNotTree[Random.Range(0, craftingEffectNotTree.Length)], hit.point);
                }


                if (FindObjectOfType<pickaxePholder>().Item is pickaxe p)
                    c.timeToCraft -= p.DestroySpeed * Random.Range(0.9f, 1.2f);


                if (c.timeToCraft <= 0)
                {
                    chests chest = facedGameObject.GetComponent<chests>();
                    if (chest != null)
                    {
                        PlayPartical(smokeEffect, facedGameObject.transform.position);
                    }
                    c.dropeItem();
                    Destroy(facedGameObject);
                }
            }  
        }
    }
    bool scaleChanged;
    Transform item;
    Vector3 initialScale;
    void playWithScale(Transform item)
    {
        this.item = item;
        initialScale = item.localScale;
        item.localScale = new Vector3(item.localScale.x - 0.08f, item.localScale.y, item.localScale.z - 0.08f);
        scaleChanged = true;
    }
    private void FixedUpdate()
    {
        if (scaleChanged)
        {
            if (item)
            {
                if (item.localScale != initialScale)
                    item.localScale = new Vector3(item.localScale.x + 0.008f, item.localScale.y, item.localScale.z + 0.008f);
                else
                    scaleChanged = false;
            }

        }
    }
    private void Update()
    {
        
        animator.SetBool("running", mvt.ismoving);
        if(pContole.canAnimate())
        {
            if (Input.GetMouseButton(0))
                startCrafting();
            else
            {
                //if (rightNowCrafted != null)
                //    rightNowCrafted = null;
                if (animator.GetBool("crafting"))
                    animator.SetBool("crafting", false);
            }
        }
        else
        {
            if (animator.GetBool("crafting"))
                animator.SetBool("crafting", false);
        }
    }
}
