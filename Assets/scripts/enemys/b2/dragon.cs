using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragon : enemy
{
    [SerializeField] GameObject ballOfFire;
    [SerializeField] int dmg;
    [SerializeField] float fireSpeed, fireSpeed1;
    [SerializeField] Transform shootingPos;
    [SerializeField] LayerMask ground;
    float startY=0f;
    private void Start()
    {
        Ray r = new Ray(transform.position, Vector3.up * -1f);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, Mathf.Infinity, ground))
        {
            startY = transform.position.y - hit.point.y;
        }

    }
    void createFire()
    {
        attackSound.Play();
        speedToAttackR = speedToAttack;
        GameObject b = Instantiate(ballOfFire, shootingPos.position, Quaternion.identity);
        float a = Random.Range(fireSpeed, fireSpeed1);
        b.GetComponent<moveFire>().setStuff(dmg, (player.position - shootingPos.position).normalized, a);
        Destroy(b, 6f);
    }
    //private void LateUpdate()
    //{
    //    Ray r = new Ray(transform.position, Vector3.up * -1f);
    //    RaycastHit hit;
    //    if (Physics.Raycast(r, out hit, Mathf.Infinity, ground))
    //    {
    //        if(transform.position.y > )
    //        transform.position = new Vector3(transform.position.x, startY + hit.point.y, transform.position.y);
    //    }
    //}
}
