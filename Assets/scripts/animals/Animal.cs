using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour,attackable
{
    [SerializeField] Animator animator;
    [SerializeField] CharacterController controller;
    public Transform placeOfShowPanel;
    [SerializeField] float speed;
    float initialSpeed;
    bool stopEating,dead;
    [SerializeField] int health = 50;
    float timeToRotate = 20f, timeToRotateR;
    float timeToEat = 30f, timeToEatR;
    [SerializeField] craft c;
    Transform midpoint;
    int initialH;
    [SerializeField] float runSpeed=8f;
    [SerializeField] AudioSource walking,eating;
    private void OnEnable()
    {
        soundManager s = FindObjectOfType<soundManager>();
        if(walking)
        walking.outputAudioMixerGroup = s.soundsMixer;
        if(eating)
        eating.outputAudioMixerGroup = s.soundsMixer;
        initialH = health;
        midpoint = FindObjectOfType<CustomTerrain>().transform.GetChild(0);
        initialSpeed = speed;
        timeToRotateR = timeToRotate;
        timeToEatR = timeToEat;
        animator.SetBool("isWalking", true);
    }
    void eatingS()
    {
        if (!eating.isPlaying)
            eating.Play();
        if (walking.isPlaying)
            walking.Stop();
    }
    void walkingS()
    {
        if (!walking.isPlaying)
            walking.Play();
    }
    public void goMid()
    {
        //Debug.Log("ff");
        Vector3 direction = (new Vector3(midpoint.position.x,transform.position.y,midpoint.position.z)  - transform.position).normalized;
        Vector3 gravity = transform.up * -8f;
        controller.Move((direction * speed + gravity) * Time.deltaTime);
        transform.forward = direction;
        timeToRotateR = timeToRotate;
        y = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
       // transform.rotation = Quaternion.Euler(0, y, 0);
    }
    float y = 0f,rY=1.5f,h=1.5f;
    private void Update()
    {
        if (transform.position.y > 37)
        {
            if (!stopEating)
            {
                if (timeToRotateR <= 0)
                {

                    y += .3f;
                    if (rY <= 0)
                    {
                        timeToRotateR = timeToRotate;
                        rY = h;
                    }
                    else
                        rY -= Time.deltaTime;

                }
                if (timeToRotateR > 0)
                    timeToRotateR -= Time.deltaTime;
            }
            if (!stopEating)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, y, transform.rotation.z);
                if (animator.GetBool("isWalking"))
                {
                    controller.Move((transform.forward * speed + -transform.up * 30f) * Time.deltaTime);
                    //Debug.Log("fdg");

                }

                if (!animator.GetBool("isWalking") && !animator.GetBool("isEating") && !dead)
                    animator.SetBool("isWalking", true);
            }

            if (timeToEatR <= 0)
            {
                stopEating = true;
                if (animator.GetBool("isWalking"))
                    animator.SetBool("isWalking", false);
                animator.SetBool("isEating", true);
                timeToEatR = timeToEat;
            }
            else
                timeToEatR -= Time.deltaTime;
        }
        else
            goMid();
    }
    public float pourcentage()
    {
        return (float)health /(float) initialH;
    }
    IEnumerator returneNrmlSpeed()
    {
        yield return new WaitForSeconds(5f);
        speed = initialSpeed;
        animator.SetBool("isRunning", false);
    }
    public void getHealth(int dmg)
    {
       // Debug.Log(dmg);
        health -= dmg;
        speed = initialSpeed + runSpeed;
        animator.SetBool("isRunning", true);
        StartCoroutine(returneNrmlSpeed());
        if (health<=0)
        {
            animator.SetBool("isEating", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDead", true);
        }
        if (animator.GetBool("isEating"))
            stopEatingE();
    }
    void FixDead()
    {
        animator.SetBool("isDead", false);
        dead = true;
        animator.SetBool("isWalking", false);
    }
    void Dead()
    {
        c.dropeItem();
        Destroy(gameObject);
    }
    void stopEatingE()
    {
        animator.SetBool("isEating", false);
        stopEating = false;
        timeToEatR = timeToEat;
    }


}
