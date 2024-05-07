using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowAttack : MonoBehaviour
{
    float MaxCharger = 1f;
    Animator animator;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform ShootingPos;
    [SerializeField] float timeArrow;
    public float bowSpeed;
    Transform aiming;
    float timeArrowR = 0;
    [SerializeField] Material[] goldMat, silverMat, DimondMat;
    MeshRenderer Mesh;
    [HideInInspector] public int bowDmg;
    playerMVT mvt;
    panelsControle pControle;
    inventoryHolder invHolder;
    [SerializeField] int arrowId;
    createStuff createStuff;
    inventory inv;
    [SerializeField] ParticleSystem p, p2;
    soundManager manager;
    private void Awake()
    {
        manager = FindObjectOfType<soundManager>();
        inv = FindObjectOfType<inventory>();
        createStuff = FindObjectOfType<createStuff>();
        invHolder = FindObjectOfType<inventoryHolder>();
        pControle = FindObjectOfType<panelsControle>();
        mvt = FindObjectOfType<playerMVT>();
        Mesh = GetComponent<MeshRenderer>();
        aiming = Camera.main.transform;
        animator = transform.GetComponent<Animator>();
    }
    void createArrow()
    {

        animator.SetBool("attack", false);
        timeArrowR = timeArrow;
        if (invHolder.numberOfItem(arrowId) > 0)
        {
            playSound.playAudoi(2, manager);
            inv.UseArrow(createStuff.findItemWithId(arrowId));
            GameObject a = Instantiate(arrow, ShootingPos.position, Quaternion.identity);
            Transform arrowTransform = a.transform;
            arrowTransform.forward = aiming.forward;
            arrowTransform.GetChild(0).GetComponent<moveArrow>().setStuff(bowSpeed * MaxCharger, p, p2);
            Destroy(a, 5f);
        }
    }
    public void setMaterial(item item)
    {
        if (item.itemID == 7)
            Mesh.materials = silverMat;
        else if (item.itemID == 8)
            Mesh.materials = goldMat;
        else if (item.itemID == 9)
            Mesh.materials = DimondMat;
    }
    void Update()
    {
        if (!animator.GetBool("hold"))
            animator.SetBool("running", mvt.ismoving);
        if (pControle.canAnimate())
        {
            if (Input.GetMouseButtonDown(0) && timeArrowR <= 0)
            {
                animator.SetBool("hold", true);
                animator.SetBool("running", false);
            }
            else if (Input.GetMouseButtonUp(0) && animator.GetBool("hold"))
            {
                animator.SetBool("hold", false);
                animator.SetBool("attack", true);
            }
            if (Input.GetMouseButton(0))
            {
                if (MaxCharger < 3)
                    MaxCharger += Time.deltaTime;
            }
        }
        if (timeArrowR > 0)
            timeArrowR -= Time.deltaTime;
    }
}
