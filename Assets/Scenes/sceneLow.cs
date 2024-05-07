using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sceneLow : MonoBehaviour
{
    Image a;
    [SerializeField] float slower;
    private void Awake()
    {
        a = GetComponent<Image>();
    }
    float b=1;
    void Update()
    {
        if (b > 0)
            a.color = new Color(0, 0, 0, b-= slower);
        else
            Destroy(gameObject);
    }
}
