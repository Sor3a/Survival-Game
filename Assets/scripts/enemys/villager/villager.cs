using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class villager : MonoBehaviour,attackable
{
    CharacterController controller;
    Animator animator;
    [SerializeField] int health, damage1, damage2;
    int initialHealth;
    [SerializeField] Transform pointOfBack, pointOfSlash;
    [SerializeField] float Range, distanceToAttack2, speedToAttackSlash, speed;
    Transform player;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject slashEffect, splashSend, door;
    playerStats stats;
    public Transform placeOfShowPanel;
    [SerializeField] LayerMask dodgeLayer;
    public bool canAttack = true;
    [SerializeField] AudioSource walkingSound, sendAttackSound;
    soundManager s;
    public void getHealth(int dmg)
    {
        if (!canAttack) canAttack = true;
        health -= dmg;
        if (health <= 0)
        {
            Destroy(gameObject);
            Destroy(door);
        }

    }
    public float pourcentage()
    {
        return (float)health / (float)initialHealth;
    }

    private void OnEnable()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        initialHealth = health;
        player = FindObjectOfType<playerStats>().transform;
        stats = player.GetComponent<playerStats>();
        s = FindObjectOfType<soundManager>();
        walkingSound.outputAudioMixerGroup = s.soundsMixer;
        sendAttackSound.outputAudioMixerGroup = s.soundsMixer;
    }
    void AnimationControle(bool run, bool a1, bool a2, bool idle)
    {
        animator.SetBool("running", run);
        animator.SetBool("attack1", a1);
        animator.SetBool("attack2", a2);
        animator.SetBool("idle", idle);

    }
    void slash()
    {

        GameObject b = Instantiate(slashEffect, pointOfSlash.position, Quaternion.identity);
        b.transform.forward = (player.position - transform.position).normalized;
        b.GetComponent<ParticleSystem>().Play();
        Destroy(b, 3f);
    }
    void SendAttack(int dmg)
    {
        GameObject b = Instantiate(splashSend, pointOfSlash.position, Quaternion.identity);
        moveFire move = b.GetComponent<moveFire>();
        Vector3 dir = (player.position - pointOfSlash.position).normalized;
        move.setStuff(dmg, dir, speedToAttackSlash);
        Destroy(b, 3f);
    }
    void attack2()
    {
        slash();
        Ray r = new Ray(transform.position, (player.position - transform.position).normalized);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, distanceToAttack2, playerLayer))
        {
            stats.getHealth(damage2);
            player.GetComponent<playerMVT>().Attack(.4f, 10f, player.position - transform.position);
        }
        else
            SendAttack(damage2 - 20);

    }
    void attack1()
    {
        slash();
        Ray r = new Ray(transform.position, (player.position - transform.position).normalized);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, distanceToAttack2, playerLayer))
        {
            stats.getHealth(damage1);
            player.GetComponent<playerMVT>().Attack(.5f, 10f, player.position - transform.position);
        }
        else
            SendAttack(damage2 - 20);
    }
    void chooseAttack()
    {
        //direction = (player.position - transform.position).normalized;
        transform.forward = direction;
        if (Vector3.Distance(player.position, transform.position) <= distanceToAttack2)
        {
            int a = Random.Range(0, 2);
            if (a > 0)
                AnimationControle(false, true, false, false);
            else
                AnimationControle(false, false, true, false);
        }
        else
        {
            AnimationControle(true, false, false, false);
        }
    }
    float rotY;
    void move(Vector3 gravity, Vector3 goal)
    {
        if (!walkingSound.isPlaying)
            walkingSound.Play();
        Ray r = new Ray(transform.position, direction);
        if (Physics.Raycast(r, 5f, dodgeLayer))
        {
            ++angle;
            // transform.forward = direction;
        }
        else
        {

            angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);

        }
        transform.rotation = Quaternion.Euler(0, angle, 0);
        controller.Move(transform.forward * Time.deltaTime * speed + gravity);

    }
    Vector3 direction;
    float angle;
    void playStart()
    {
        sendAttackSound.Play();
    }
    void Update()
    {
        Vector3 gravity = transform.up * -8f * Time.deltaTime;
        if (Vector3.Distance(player.position, pointOfBack.position) <= Range && canAttack)
        {
            direction = (player.position - new Vector3(transform.position.x, player.position.y, transform.position.z)).normalized;
            if (Vector3.Distance(player.position, transform.position) <= distanceToAttack2)
            {
                if (walkingSound.isPlaying)
                    walkingSound.Stop();
                //sendAFirst.Play();
                if (animator.GetBool("attack1") == false && animator.GetBool("attack2") == false)
                    chooseAttack();

            }
            else
            {

                if (animator.GetBool("attack1") == false && animator.GetBool("attack2") == false)
                {

                    AnimationControle(true, false, false, false);
                    move(gravity, player.position);
                }
                else
                    transform.forward = direction;

            }
        }
        else
        {
            direction = (pointOfBack.position - transform.position).normalized;
            if (Vector3.Distance(transform.position, pointOfBack.position) > 4f)
            {

                AnimationControle(true, false, false, false);
                move(gravity, pointOfBack.position);
            }
            else
            {
                if (walkingSound.isPlaying)
                    walkingSound.Stop();
                if (health < initialHealth)
                    health += 1;
                if (!animator.GetBool("idle"))
                    AnimationControle(false, false, false, true);

            }
        }
        controller.Move(gravity);

    }
}
