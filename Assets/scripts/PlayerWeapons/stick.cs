using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;


public class stick : MonoBehaviour
{

    Transform player;
    Transform cam;
    [SerializeField] float distanceToCraft, timeToAttack, stickCrafting;
    [SerializeField] int stickDmg;
    [SerializeField] LayerMask craftedLayer, enemyLayer, animalLayer;
    float timeToAttackR = 0;
    Animator animator;
    playerMVT mvt;
    [SerializeField] ParticleSystem  AnimalEffect, smokeEffect,ironEffect;
    [SerializeField] ParticleSystem[] craftEffects;
    [SerializeField] ParticleSystem[] craftEffectsNotTree;
    [SerializeField] ParticleSystem[] EnemysEffect;
    playerStats stats;
    soundManager manager;
    [SerializeField] AudioSource[] potionCraft,TreesCraft;
    panelsControle pControle;
    RayCastWithEntites raycastEntitySystem;
    EntityManager entityManager;
    DataManager dataManager;
    private void Awake()
    {
        dataManager = FindObjectOfType<DataManager>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        raycastEntitySystem = FindObjectOfType<RayCastWithEntites>();
        pControle = FindObjectOfType<panelsControle>();
        manager = FindObjectOfType<soundManager>();
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        player = FindObjectOfType<playerMVT>().transform;
        mvt = player.GetComponent<playerMVT>();
        stats = FindObjectOfType<playerStats>();
    }
    private void Start()
    {
        timeToAttackR = timeToAttack;
    }
    void PlayPartical(ParticleSystem p, Vector3 pos)
    {
        p.transform.position = pos;
        p.Play();
    }
    bool scaleChanged;
    Transform item;
    Vector3 initialScale;
   

    public void Craft()
    {
        Ray r = new Ray(player.position, cam.forward);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, distanceToCraft, craftedLayer))
        {

            GameObject hited = hit.collider.gameObject;
            // Debug.Log(hited.name+" "+hited.layer);

            craft c = hited.GetComponent<craft>();
            chests chest = hited.GetComponent<chests>();
            playSound.playAudoi(0, manager);
            playWithScale(hited.transform);
            if (c)
            {
                
                if (c.timeToCraft < 5f)
                    c.timeToCraft -= stickCrafting * Random.Range(0.9f, 1.3f);
                else
                {
                    PlayPartical(ironEffect, hit.point);
                    return;
                }


                if (c.Item)
                {
                    if (c.Item.itemID == 20 && chest == null)
                        PlayPartical(craftEffects[Random.Range(0, craftEffects.Length)], hit.point);                    
                    else
                        PlayPartical(craftEffectsNotTree[Random.Range(0, craftEffectsNotTree.Length)], hit.point); 
                }
                else
                {
                    PlayPartical(craftEffectsNotTree[Random.Range(0, craftEffectsNotTree.Length)], hit.point);
                }


                if (c.timeToCraft <= 0)
                {
                    if (chest != null)
                    {
                        PlayPartical(smokeEffect, hited.transform.position);
                    }
                    c.dropeItem();
                    Destroy(hited);
                }
            } 
        }

        else if (Physics.Raycast(r, out hit, distanceToCraft, enemyLayer))
        {
            playSound.playAudoi(10, manager);
            GameObject hitedObject= hit.collider.gameObject;
            enemy Enemy = hitedObject.GetComponent<enemy>();
            attackable target = hitedObject.GetComponent<attackable>();
            ParticleSystem p = EnemysEffect[Random.Range(0, EnemysEffect.Length)];
            if (target != null) target.getHealth(stats.damage);
            if (Enemy)
            {
                Color e = Enemy.enemyColor;
                p.startColor = e;
               // Enemy.getHealth(stickDmg);
            }
            PlayPartical(p, hit.point);
            playSound.playAudoi(13, manager);
        }
        else if (Physics.Raycast(r, out hit, distanceToCraft, animalLayer))
        {
            PlayPartical(AnimalEffect, hit.point);
            hit.collider.gameObject.GetComponent<Animal>().getHealth(stickDmg);
            playSound.playAudoi(13, manager);
        }

        var raycastHits = raycastEntitySystem.getRayCastHits();
        if (raycastHits != null)
        {
            
            for (int i = 0; i < raycastHits.Length; i++)
            {
                if (entityManager.HasComponent<TreeData>(raycastHits[i].Entity))
                {
                    getTimeToCraftOfTree(raycastHits[i].Entity);
                    break;
                }
            }
        }
    }

    void getTimeToCraftOfTree(Entity e)
    {
        if(entityManager.HasComponent<TreeData>(e))
        {
            var treeDataComponent = entityManager.GetComponentData<TreeData>(e);
            var EntityPosition = entityManager.GetComponentData<Translation>(e).Value;
            //Debug.Log(treeDataComponent.timeToCraft);
            PlayPartical(craftEffects[Random.Range(0, craftEffects.Length)], EntityPosition);
            treeDataComponent.timeToCraft -= stickCrafting * Random.Range(0.9f, 1.3f);
            entityManager.SetComponentData(e, treeDataComponent);
            if (treeDataComponent.timeToCraft<=0)
            {
                dataManager.DropItem(e);
                entityManager.DestroyEntity(e);
            }
        }
    }

    int a = -1; //int that said which animation should i start ( bug animation won't change if player keep pressing and changing target ) idk :)

    void startCrafting()
    {
        
        if (a < 0)
        {
            Ray r = new Ray(player.position, cam.forward);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, distanceToCraft, animalLayer)|| Physics.Raycast(r, out hit, distanceToCraft, enemyLayer))
            {
                a = 2;
            }
            else
                a = 1;
        }
        if (a == 1 && !animator.GetBool("crafting"))
        {
            animator.SetBool("crafting", true);
            //playSound.playAudoi(1, manager);
        }    
        else if (a == 2 && !animator.GetBool("attacking"))
        {
            animator.SetBool("attacking", true);
            
        }
        
    }
    void playSoun()
    {
        playSound.playAudoi(1, manager);
    }
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
    void Update()
    {
       

        animator.SetBool("moving", mvt.ismoving);
        if (Input.GetMouseButton(0) && pControle.canAnimate())
            startCrafting();
        else
        {
            a = -1;
            animator.SetBool("crafting", false);
            animator.SetBool("attacking", false);
        }
        if (timeToAttackR > 0)
            timeToAttackR -= Time.deltaTime;
    }
}
