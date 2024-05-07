using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixVillage : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] float adjustment;
    [SerializeField] villager protector,protector2;
    public void protect()
    {
        protector.canAttack = true;
        protector2.canAttack = true;
    }
    IEnumerator creator()
    {
        yield return new WaitForSeconds(.5f);
        foreach (Transform item in transform)
        {
            Ray r = new Ray(item.position + new Vector3(0, 1000, 0), -Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, Mathf.Infinity, terrainLayer))
            {
                //float forw = item.rot;
                item.position = hit.point + new Vector3(0,adjustment,0);
                item.up = hit.normal;
                item.RotateAround(item.position,item.up,180f);
            }
        }
    }
    IEnumerator fix()
    {
        yield return new WaitForEndOfFrame();
        foreach (Transform item in transform)
        {
            Ray r = new Ray(item.position + new Vector3(0, 1000, 0), -Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, Mathf.Infinity, terrainLayer))
            {
                //float forw = item.rot;
                item.position = hit.point + new Vector3(0, adjustment, 0);
                item.up = hit.normal;
                item.RotateAround(item.position, item.up, 180f);
            }
        }
    }
    private void OnEnable()
    {
        protector.canAttack = false;
        protector2.canAttack = false;
        StartCoroutine(fix());
    }
}
