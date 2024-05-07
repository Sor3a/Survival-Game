using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dog : MonoBehaviour
{
    Animator animator;
    CharacterController controller;
    [SerializeField] float speedWalking, timeToRotate, RotationParTime,timeTObark;
    float rotationY=0f, RtimeToRotate, RotationYPrev=0f,RtimeTobark;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        RtimeToRotate = timeToRotate;
        RtimeTobark = timeTObark;
        animator.SetBool("isWalking", true);
    }
    void move()
    {
        Vector3 gravity = transform.up * -8f;
        controller.Move((transform.forward * speedWalking + gravity) * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, rotationY, transform.rotation.z);
        if (RtimeToRotate > 0)
            RtimeToRotate -= Time.deltaTime;
        else
        {
            if (rotationY < RotationYPrev + RotationParTime)
                rotationY += 0.5f;
            else
            {
                RotationYPrev = rotationY;
                RtimeToRotate = timeToRotate;
            }
        }
    }
    void returneWalking()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isBarking", false);
    }
    private void FixedUpdate()
    {
        if (RtimeTobark>0)
        {
            if (animator.GetBool("isWalking"))
            {
                RtimeTobark -= Time.deltaTime;
                move();
            }   
        }
        else
        {
            RtimeTobark = timeTObark;
            animator.SetBool("isWalking", false);
            animator.SetBool("isBarking", true);
        }
            
    }
}
