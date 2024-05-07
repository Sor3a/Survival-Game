using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Entities;
using Unity.Transforms;

public class showPanePlayer : MonoBehaviour
{
    Transform player, cam;
    [SerializeField] float distanceToSee;
    [SerializeField] LayerMask Craftinglayer,enemyLayer,animalLayer,mashRooms,spawnStuff;
    [SerializeField] GameObject panel,panel2;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Scrollbar bar;
     ParticleSystem[] pickEffects;
    inventory inv;
    RayCastWithEntites rayCastEntitiesSystem;
    EntityManager manager;
    private void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        rayCastEntitiesSystem = FindObjectOfType<RayCastWithEntites>();
        player = transform;
        cam = Camera.main.transform;
        inv = FindObjectOfType<inventory>();
        pickEffects = GetComponent<UseableItem>().PickItemsEffects;
    }
    private void Start()
    {
        lastPanelOpen = FindObjectOfType<exchange>().panel;
    }
    
    void showPanelF(Vector3 placeOfShow , float pourcentage,string name,Vector3 objectIsee,float fix)
    {
        panel.SetActive(true);
        if (placeOfShow == null)
            panel.transform.position = objectIsee + (player.position - objectIsee).normalized*fix;
        else
            panel.transform.position = placeOfShow + (player.position - objectIsee).normalized * fix;
        panel.transform.forward = player.forward;
        bar.value = 0;
        bar.size = pourcentage;
        text.text = name;
    }
    void showPanelF(Transform placeOfShow, float pourcentage, string name, Vector3 objectIsee, float fix)
    {
        panel.SetActive(true);
        if (placeOfShow == null)
            panel.transform.position = objectIsee + (player.position - objectIsee).normalized * fix;
        else
            panel.transform.position = placeOfShow.position + (player.position - objectIsee).normalized * fix;
        panel.transform.forward = player.forward;
        bar.value = 0;
        bar.size = pourcentage;
        text.text = name;
    }
    [HideInInspector] public GameObject lastPanelOpen;
    void showPanel()
    {
        Ray r = new Ray(player.position, cam.forward);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, distanceToSee, Craftinglayer))
        {
            Transform objectIsee = hit.collider.transform;
            craft c = objectIsee.GetComponent<craft>();
            if (c)
                showPanelF(c.placeOfShowPanel, c.pourcentage(), c.Name, objectIsee.position, 1.1f);
        }
        else if(Physics.Raycast(r, out hit, distanceToSee+30f, enemyLayer))
        {
            Transform objectIsee = hit.collider.transform;
            enemy c = objectIsee.GetComponent<enemy>();
            if(c)
            {
                showPanelF(c.placeOfShowPanel, c.pourcentage(), "Enemy", objectIsee.position,.5f);
            }
            else //villager
            {
                villager a = objectIsee.GetComponent<villager>();
                if(a)
                {
                    showPanelF(a.placeOfShowPanel, a.pourcentage(), "hogaboga", objectIsee.position, 0f);
                }
                else
                {
                    human h1 = objectIsee.GetComponent<human>();
                    if (h1)
                        showPanelF(h1.placeOfShowPanel, h1.pourcentage(), "human", objectIsee.position, 0f);
                }

            }

        }
        else if(Physics.Raycast(r, out hit, distanceToSee + 30f, animalLayer))
        {
            Transform objectIsee = hit.collider.transform;
            Animal c = objectIsee.GetComponent<Animal>();
            showPanelF(c.placeOfShowPanel, c.pourcentage(), "Animal", objectIsee.position,1f);
        }
        else 
            panel.SetActive(DetectRayCastsWithEntities());

        if (Physics.Raycast(r, out hit, distanceToSee, mashRooms))
        {
            GameObject facedObject = hit.collider.gameObject;
            secondShowPanel(facedObject);
            if (Input.GetKeyDown(KeyCode.F))
            {
                craft c = facedObject.GetComponent<craft>();
                if (c)
                {
                    inv.PickItem(c.Item,1);
                    int a = Random.Range(0, pickEffects.Length);
                    pickEffects[a].transform.position = hit.point;
                    pickEffects[a].Play();
                    Destroy(facedObject);
                }
                else
                {
                    exchange ex = facedObject.GetComponent<exchange>();
                    if (ex)
                    {
                        ex.showPanel();
                        lastPanelOpen = ex.panel;
                    }


                }
                panel2.SetActive(false);
            }
        }
        else if (Physics.Raycast(r, out hit, distanceToSee, spawnStuff))
        {
            GameObject facedObject = hit.collider.gameObject;
            secondShowPanel(facedObject);
            if (Input.GetKeyDown(KeyCode.F))
            {
                facedObject.GetComponent<spawnerContole>().spawn();
            }

        }
        else
        {
            panel2.SetActive(false);
            if(lastPanelOpen)
            lastPanelOpen.SetActive(false);
        }

        
    }
    bool DetectRayCastsWithEntities()
    {
        var raycastHits = rayCastEntitiesSystem.getRayCastHits();
        if (raycastHits != null)
        {

            for (int i = 0; i < raycastHits.Length; i++)
            {
                if (manager.HasComponent<TreeData>(raycastHits[i].Entity))
                {
                    var entity = raycastHits[i].Entity;
                    var TreeData = manager.GetComponentData<TreeData>(entity);
                    var EntityPos = (Vector3)manager.GetComponentData<Translation>(entity).Value;
                    showPanelF(
                        EntityPos +TreeData.placeOfShowPanel ,
                        TreeData.timeToCraft/TreeData.initialTime,
                        "Tree", 
                        EntityPos, 
                        1.1f);
                    return true;
                    
                }
            }
        }
        return false;

    }
    void secondShowPanel(GameObject facedObject)
    {
        panel2.transform.forward = player.forward;
        panel2.transform.position = facedObject.transform.position + (player.position - facedObject.transform.position).normalized;
        panel2.SetActive(true);
    }

    void Update()
    {
        showPanel();
    }

}
