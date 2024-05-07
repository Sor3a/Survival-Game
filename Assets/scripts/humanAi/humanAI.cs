using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanAI : MonoBehaviour,attackable
{
    [SerializeField] float speed,maxToGo;
    [SerializeField] LayerMask LayerToAvoid;
    [SerializeField] Transform centralPoint;
    [SerializeField] fixVillage village;
    [SerializeField] AudioSource walknigSound;
    Animator animator;
    CharacterController controller;
    float rotY;
    private void OnEnable()
    {
        walknigSound.outputAudioMixerGroup = FindObjectOfType<soundManager>().soundsMixer;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        StartCoroutine(changeMvt(Random.Range(30f, 40f)));
    }
    void returneNrml()
    {
        animator.SetInteger("idle", 0);
    }
    IEnumerator changeMvt(float waitedTime)
    {
        yield return new WaitForSeconds(waitedTime);
        walknigSound.Stop();
        animator.SetInteger("idle", Random.Range(1, 3));
        StartCoroutine(changeMvt(Random.Range(30f, 40f)));
    }
    public void getHealth(int dmg)
    {
        village.protect();
        Destroy(gameObject);
    }
    void move()
    {
        if(animator.GetInteger("idle")==0)
        {
            walkSound();
            Ray r = new Ray(transform.position, transform.forward);
            Ray r2 = new Ray(transform.position, transform.right);
            if (Vector3.Distance(transform.position, centralPoint.position) > maxToGo)
                rotY++;
            if (Physics.Raycast(r, 5f, LayerToAvoid))
            {
                if (Physics.Raycast(r2, 3f, LayerToAvoid))
                    rotY -= 3f;
                else
                    rotY += 3f;
            }
            controller.Move((transform.forward * speed + transform.up * -8f) * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, rotY, 0);
        }
    }
    void walkSound()
    {
        if (!walknigSound.isPlaying)
            walknigSound.Play();
    }
    private void FixedUpdate()
    {
            move();
    }


}
