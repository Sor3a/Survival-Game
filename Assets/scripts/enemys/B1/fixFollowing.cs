using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixFollowing : MonoBehaviour
{
    Transform child;
    private void Awake()
    {
        child = transform.GetChild(0);
    }
    void Update()
    {
        transform.position = new Vector3( child.position.x, transform.position.y,child.position.z);
    }
}
