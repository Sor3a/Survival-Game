using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class human : MonoBehaviour,attackable
{
    [SerializeField] float speedWalking, timeToRotate, RotationParTime, speed, DistanceToForget;
    float RtimeToRotate;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Transform target,player,midpoint;
    float rotationY = 0f, RotationYPrev = 0f;
    public int Health;
    int initalHealh;
    public float distanceToAttack;
    public Transform placeOfShowPanel;
    public AudioSource walkingSOund,slash;
    public float pourcentage()
    {
        return (float)Health / (float)initalHealh;
    }
     void playSlash(){ slash.Play(); }
    public void goMid()
    {
        Vector3 direction = (new Vector3(midpoint.position.x, transform.position.y, midpoint.position.z) - transform.position).normalized;
        Vector3 gravity = transform.up * -8f;
        controller.Move((direction * speed + gravity) * Time.deltaTime);
        transform.forward = direction;
        rotationY = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        RotationYPrev = rotationY;
        RtimeToRotate = timeToRotate;
        target = null;
        a = 1;
        playMvtSound();
    }
    public void controleAnimation(bool walk, bool run, bool a1, bool a2)
    {
        animator.SetBool("walking", walk);
        animator.SetBool("running", run);
        animator.SetBool("attack1", a1);
        animator.SetBool("attack2", a2);
    }
    public void playMvtSound()
    {
        if (!walkingSOund.isPlaying)
            walkingSOund.Play();
    }
    public void followTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance > DistanceToForget)
        {
            target = null;
            return;
        }
        if (distance > distanceToAttack )
        {
            if(!animator.GetBool("attack1") && !animator.GetBool("attack2"))
            {
                controleAnimation(false, true, false, false);
                Vector3 gravity = transform.up * -8f;

                playMvtSound();
                controller.Move((direction * speed + gravity) * Time.deltaTime);
            }

        }
    }
    public void getHealth(int dmg)
    {
        if (!target)
            target = FindObjectOfType<playerMVT>().transform;
        Health -= dmg;
        if (Health <= 0)
        {
            Destroy(gameObject);
            GetComponent<craft>().dropeItem();
        }
    }
    public soundManager s;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        RtimeToRotate = timeToRotate;
        initalHealh = Health;
        player = FindObjectOfType<playerMVT>().transform;
        midpoint = FindObjectOfType<CustomTerrain>().transform.GetChild(0);
        s = FindObjectOfType<soundManager>();
        slash.outputAudioMixerGroup = s.soundsMixer;
        walkingSOund.outputAudioMixerGroup = s.soundsMixer;
    }
    int a=1;
    public void RandomWalking()
    {
        playMvtSound();
        if (!animator.GetBool("walking"))
            animator.SetBool("walking", true);
        Vector3 gravity = transform.up * -8f;
        controller.Move((transform.forward * speedWalking + gravity) * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, rotationY, transform.rotation.z);
        if (RtimeToRotate > 0)
            RtimeToRotate -= Time.deltaTime;
        else
        {
            if (Mathf.Abs( rotationY - (RotationYPrev + RotationParTime*a))>0.1f)
                rotationY += 0.1f*a;
            else
            {
                a = Random.Range(0, 2) > 0 ? 1 : -1;
                RotationYPrev = rotationY;
                RtimeToRotate = timeToRotate;
            }
        }

    }
}
