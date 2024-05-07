using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour, attackable
{
    public Color enemyColor;
    [HideInInspector] public Transform player;
    [SerializeField] float radious, speed, changeRotationAfter, dsitanceToStop;
    public float followSpeed, speedToAttack;
    public LayerMask playerLayer;
    float timeToChangeR = 0, rotationY = 0f, nextPose;
    bool setNextPose = false;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public bool didFindPlayer = false;
    public int heath;
    [HideInInspector] public Animator animator;
    [HideInInspector] public float speedToAttackR=0;
    [HideInInspector] public playerStats stats;
    [SerializeField] Transform objectToCHange;
    public Transform placeOfShowPanel;
    int initialHealth;
    [HideInInspector] public Transform p;
    ParticleSystem particle;
    Transform midpoint;
    [SerializeField] int a = 1;
    public LayerMask blockLayers,nrmlLayer;
    public AudioSource walkingSound,attackSound;
    [HideInInspector] public soundManager manager;
    


    private void Awake()
    {
        manager = FindObjectOfType<soundManager>();
        p = FindObjectOfType<playerMVT>().transform;
        particle = p.GetComponent<UseableItem>().p;
        midpoint = FindObjectOfType<CustomTerrain>().transform.GetChild(0);
       
        stats = p.GetComponent<playerStats>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = transform.GetChild(0).GetComponent<Animator>();
        initialHealth = heath;
        craft c = GetComponent<craft>();
        if(c)
        {
            enemyColor = c.Item.model.GetComponent<MeshRenderer>().sharedMaterials[0].color;
        }
        if (walkingSound)
            walkingSound.outputAudioMixerGroup = manager.soundsMixer;
        if (attackSound)
            attackSound.outputAudioMixerGroup = manager.soundsMixer;
        runningAnimation();
    }
    public float pourcentage()
    {
        return (float)heath / (float)initialHealth;
    }
    
    public void getHealth(int dmg)
    {
        if (player == null)
        {
            
            player = p;
            stats = player.GetComponent<playerStats>();
        }
       // Attack((transform.position - player.position).normalized);
        heath -= dmg;
        //Debug.Log("attacked");
        if(heath<=0)
        {
           // Debug.Log(gameObject.name + " is dead ");
            craft itemToCraft = GetComponent<craft>();

            //i did not institae particles just create one parti and call it when so is dead ( maybe bug when 2 ppl dead ) fk it :)
            particle.transform.position = transform.position;
            particle.startColor = enemyColor;
            particle.Play();
            if (itemToCraft == null && transform.parent!=null)
                itemToCraft = transform.parent.GetComponent<craft>();
            if (itemToCraft)
                itemToCraft.dropeItem();
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
    public void changeAnimation()
    {
        if(distance > dsitanceToStop)
            runningAnimation();
    }
    float distance;
    float angle=0f;
    public void goMid()
    {
        Vector3 direction = (midpoint.position - transform.position).normalized;
        Vector3 gravity = transform.up * -8f;
        controller.Move((direction * speed + gravity) * Time.deltaTime);
        transform.forward = direction;
        timeToChangeR = changeRotationAfter;
        rotationY = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        if (setNextPose)
            nextPose = rotationY + 30;
        player = null;
        a = 1;
    }
    public void followPlayer()
    {
       // Vector3 enemypp = new Vector3(transform.position.x, player.position.y, transform.position.z);
        Vector3 direction = new Vector3( p.position.x,transform.position.y,p.position.z) - transform.position;
        
        //transform.forward =(playerPP - transform.position).normalized;
        distance = Vector3.Distance(player.position, transform.position);
        if (distance > dsitanceToStop)
        {
            Ray r = new Ray(transform.position, direction);
            Ray r1 = new Ray(transform.position, transform.right);
            
            if (!Physics.Raycast(r, 5f, blockLayers))
            {
                runningAnimation();
                didFindPlayer = false;
                Vector3 gravity = transform.up * -8f * Time.deltaTime;
                if (Physics.Raycast(r, 5f, nrmlLayer))
                {
                   // Debug.Log("f");
                    if (Physics.Raycast(r1, 5f, nrmlLayer))
                        angle--;
                    else
                        angle++;
                   
                }   
                else
                    angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
                // transform.forward = 
                transform.rotation = Quaternion.Euler(0, angle, 0);
                controller.Move(transform.forward * followSpeed * Time.deltaTime + gravity);
            }
            else
                player = null;
            
        }
        else
        {
            angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            if (speedToAttackR <= 0)
                attackAnimation();
            else
                setAnimationIdle();

            //Debug.Log(Vector3.Distance(playerPos, transform.position));
            didFindPlayer = true;
        }
        if(speedToAttackR>0)
            speedToAttackR -= Time.deltaTime;
    }
    public void setAnimationIdle()
    {

        animator.SetBool("attacking", false);
        animator.SetBool("running", false);
        if (walkingSound && !isCustom)
            walkingSound.Stop();
    }
    public void findPlayer()
    {

        if (p.position.y>37)
        {
            if (Vector3.Distance(p.position, transform.position) <= radious)
            {
                Ray r = new Ray(transform.position,p.position-transform.position);
                if(!Physics.Raycast(r,20f, blockLayers))
                {
                    player = p; 
                }
            }
        }
    }
    public void randomMvt()
    {
        Vector3 gravity =  transform.up * -8f * Time.deltaTime;
        runningAnimation();
        controller.Move(gravity + transform.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, rotationY, 0);

        //transform.Translate(speed * Time.deltaTime * transform.forward,Space.World);

        if (timeToChangeR <= 0)
        {
            if (!setNextPose)
            {
                a = Random.Range(0, 2) > 0 ? 1 : -1;
                nextPose = rotationY + 30*a;
                setNextPose = true;
            }
            rotationY+=.1f*a;
            if (Mathf.Abs(rotationY - nextPose)<0.1f)
                timeToChangeR = changeRotationAfter;
        }
        else
        {
            setNextPose = false;
            timeToChangeR -= Time.deltaTime;
        }

    }
    IEnumerator setAttackFalse()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("attacking", false);
    }
    void attackSoun()
    {
        if (attackSound && !attackSound.isPlaying)
            attackSound.Play();
    }
    public void attackAnimation()
    {
        if (walkingSound && !isCustom)
            walkingSound.Stop();

        animator.SetBool("attacking", true);
        animator.SetBool("running", false);
        StopAllCoroutines();
        StartCoroutine(setAttackFalse());

    }
    void walkSoun()
    {
        walkingSound.Play();
    }
    [SerializeField] bool isCustom = false;
    void runningAnimation()
    {
        if (!animator.GetBool("running"))
        {
            animator.SetBool("attacking", false);
            animator.SetBool("running", true);
            if (walkingSound && !walkingSound.isPlaying && !isCustom)
            {
                
                walkingSound.Play();
            }
                
        }
    }
    void Update()
    {
        if (transform.position.y > 37)
        {
            float distance = Vector3.Distance(p.position, transform.position);
            if (player == null)
            {
                randomMvt();
                
                findPlayer();
            }
            else
            {
                if (distance < 50f)
                {
                    followPlayer();
                }     
                else
                    player = null;
            }
            if (distance >= 600f)
            {
                if (transform.parent != null)
                    Destroy(transform.parent.gameObject);
                Destroy(gameObject);
            }
        }
        else
        {
            goMid();
            
        }

        //if (startMotion)
        //{
        //    transform.forward = (player.position - transform.position).normalized;
        //    controller.Move(motion * Time.deltaTime * forceAttack + transform.up * RjumpSpeed * Time.deltaTime);
        //    if (motionTimeEnd)
        //    {
        //        startMotion = false;
        //        EnemyCanMove = true;
        //    }
        //}

    }


}
