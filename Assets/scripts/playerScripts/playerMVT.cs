using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMVT : MonoBehaviour
{
    public float speed;
    CharacterController controller;
    float x, y, mouseX, mouseY, IntialSpeed, RjumpSpeed;
    [SerializeField] float jumpSpeed, sensibility, forceAttack,shiftSpeed;
    [SerializeField] Transform face;
    [SerializeField] LayerMask ground;
    float lastSpeedAmoutAdded = 0, lastJumpAmoutAdded = 0, intiatlJumpSpeed;
    [HideInInspector] public bool ismoving;
    public bool playerCanMove = false;
    float FixInitalSpeed;
    bool startMotion, motionTimeEnd = false;
    [SerializeField] float motionTime;
    Vector3 motion;
    [SerializeField] Transform endPlace,jump;
    [SerializeField] GameObject runningEffect,walkingEffect;
    playerStats stats;
    [SerializeField] ParticleSystem jumpEffect;
    showPanePlayer panlePl;
    soundManager manager;
    RayCastWithEntites rayCastPhysics;

    private void Awake()
    {
        rayCastPhysics = FindObjectOfType<RayCastWithEntites>();
        manager = FindObjectOfType<soundManager>();
        panlePl = GetComponent<showPanePlayer>();
        stats = GetComponent<playerStats>();
        controller = GetComponent<CharacterController>();
        rayCastPhysics = FindObjectOfType<RayCastWithEntites>();
        playerCanMove = false;
    }
    private void Start()
    {
        Application.runInBackground = true;
        RjumpSpeed = 0;
        IntialSpeed = speed;
        FixInitalSpeed = IntialSpeed;
        intiatlJumpSpeed = jumpSpeed;
    }
    IEnumerator retrueSpeedNormal(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        IntialSpeed = FixInitalSpeed;
    }
    public void slowSpeed(float slowTime, float slowAmout)
    {
        IntialSpeed -= slowAmout;
        StartCoroutine(retrueSpeedNormal(slowTime));
    }
    IEnumerator stopSpeed(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        lastSpeedAmoutAdded = 0;
        this.IntialSpeed = FixInitalSpeed;
    }
    IEnumerator stopJump(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        lastJumpAmoutAdded = 0;
        this.jumpSpeed = intiatlJumpSpeed;
    }
    public void addSpeed(float speed, float time)
    {
        if (lastSpeedAmoutAdded != speed)
            this.IntialSpeed = this.IntialSpeed - lastSpeedAmoutAdded + speed;
        lastSpeedAmoutAdded = speed;
        StopCoroutine(stopSpeed(time));
        StartCoroutine(stopSpeed(time));
    }
    public void addJump(float jumpSpeed, float time)
    {
        if (lastJumpAmoutAdded != jumpSpeed)
            this.jumpSpeed = this.jumpSpeed - lastJumpAmoutAdded + jumpSpeed;
        lastJumpAmoutAdded = jumpSpeed;
        StopCoroutine(stopJump(time));
        StartCoroutine(stopJump(time));
    }
    [HideInInspector] public KeyCode Ypositive = KeyCode.Z,Ynegative = KeyCode.S;
    [HideInInspector] public KeyCode Xpositive = KeyCode.D, Xnegative = KeyCode.Q;

    public void setKeys(KeyCode z,KeyCode s ,KeyCode d,KeyCode q)
    {
        Ypositive = z;
        Ynegative = s;
        Xpositive = d;
        Xnegative = q;
    }
    [HideInInspector] public bool CanMove = true;

    Vector3 direction()
    {
        return (y * transform.forward + x * transform.right).normalized;
    }
    void getAxis()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");

        y = Input.GetKey(Ypositive) ? Input.GetKey(Ynegative) ? 0 : 1 : Input.GetKey(Ynegative) ? -1 : 0; //Input.GetAxis("Horizontal")
        x = Input.GetKey(Xpositive) ? Input.GetKey(Xnegative) ? 0 : 1 : Input.GetKey(Xnegative) ? -1 : 0; //Input.GetAxis("Vertical")
        
        ismoving = (x != 0 || y != 0) && rayCastPhysics.DetectCollision(direction()) ? true : false;
    }
    bool isGrounded()
    {
        Ray r = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        bool isGrounded = Physics.Raycast(r, 2f);
        return isGrounded;
    }
    bool didJump = false;
    IEnumerator didJumpT()
    {
        yield return new WaitForSeconds(.4f);
        didJump = true;
    }
    int  moveState;
    void sound()
    {
        if (ismoving)
        {

            if (!shiftOn)
            {
                if (moveState != 1)
                {
                    playSound.stopSound(manager, 6);
                    playSound.playAudoi(5, manager);
                    moveState = 1;
                }

            }
            else
            {
                if (moveState != 2)
                {
                    playSound.stopSound(manager, 5);
                    playSound.playAudoi(6, manager);
                    moveState = 2;
                }
            }

        }
        else
        {
            playSound.stopSound(manager, 5);
            playSound.stopSound(manager, 6);
            moveState = 0;
        }

    }
    void Update()
    {
        getAxis();
        sound();
        bool g = isGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && g && stats.force > 0.01f && !forceIsReturning)
        {
            RjumpSpeed = jumpSpeed;
            speed = IntialSpeed;
           
            StartCoroutine(didJumpT());
        }
        if (!g)
        {
            if (didJump)
                stats.GetForce(8f);
            RjumpSpeed -= Time.deltaTime * 20f;
            //if (speed == IntialSpeed)
            //    speed /= 2f;
        }
        else
        {
            if (didJump)
            {
                didJump = false;
                jumpEffect.transform.position = jump.position;
                jumpEffect.Play();
            }     
            if (speed != IntialSpeed)
                speed = IntialSpeed;
        }
        //if (Input.GetKeyDown(KeyCode.K))
        //    Attack(1f, 50f, -transform.forward);


        //speeding fuctionality
        if (runningEffect.activeSelf == false && ismoving  )
            walkingEffect.SetActive(true);
        else
            walkingEffect.SetActive(false);


        if (stats.force > 0.1f && !forceIsReturning)//
        {
            //Debug.Log("dfgh1");
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //Debug.Log("dfgh");
                runningEffect.SetActive(true);
                if (!shiftOn)
                {

                    IntialSpeed += shiftSpeed;
                    shiftOn = true;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
                stats.GetForce(5f);
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                runningEffect.SetActive(false);
                IntialSpeed = FixInitalSpeed + lastSpeedAmoutAdded;
                shiftOn = false;
            }


        }
        else
        {
            runningEffect.SetActive(false);
            forceIsReturning = true;
            if (stats.force < 100f && stats.eating > 0.1f)
                stats.returneForce();
            else
                forceIsReturning = false;
            shiftOn = false;
            if (IntialSpeed != FixInitalSpeed + lastSpeedAmoutAdded)
            {
                IntialSpeed = FixInitalSpeed + lastSpeedAmoutAdded;

            }
        }
    }
    public void setSensibility(float sens)
    {
        sensibility = sens;
    }
    IEnumerator StopMotion()
    {
        yield return new WaitForSeconds(motionTime);
        motionTimeEnd = true;
    }
    public void Attack(float motionTime, float motionForce, Vector3 motion)
    {
        this.motionTime = motionTime;
        forceAttack = motionForce;
        this.motion = motion;
        AddForce();
    }
    void AddForce()
    {
        RjumpSpeed = -10f;
        motionTimeEnd = false;
        startMotion = true;
        playerCanMove = false;
        StartCoroutine(StopMotion());
    }
    bool shiftOn = false;
    bool forceIsReturning = false;
    private void FixedUpdate()
    {

        if (playerCanMove)
        {
            if (!ismoving) { x = 0; y = 0; }
            
                controller.Move((transform.forward * y + transform.right * x) * speed * Time.deltaTime + transform.up * RjumpSpeed * Time.deltaTime);
        }
        if(panlePl.lastPanelOpen)
        {
            if (panlePl.lastPanelOpen.activeSelf == false)
            {
                transform.rotation = Quaternion.Euler(0, mouseX * sensibility, 0);
                mouseY = Mathf.Clamp(mouseY, -60f / sensibility, 60f / sensibility);
                face.localRotation = Quaternion.Euler(mouseY * sensibility, 0, 0);
            }
        }



        if (startMotion)
        {   
            controller.Move(motion * Time.deltaTime * forceAttack + transform.up * RjumpSpeed * Time.deltaTime);
            if (motionTimeEnd && isGrounded())
            {
                startMotion = false;
                playerCanMove = true;
            }
        }
    }
    IEnumerator returnePlayer()
    {
        yield return new WaitForSeconds(.1f);
        playerCanMove = true;
    }
    public void MoveToEnd()
    {
        //Debug.Log("i work");
        playerCanMove = false;
        transform.position = endPlace.position;
        StartCoroutine(returnePlayer());
    }
}
