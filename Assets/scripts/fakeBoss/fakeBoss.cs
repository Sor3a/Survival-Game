using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeBoss : MonoBehaviour
{
    Animator animator;
    CharacterController controller;
    [SerializeField] GameObject rocks,effect;
    [SerializeField] float rocksNumber,gravity;
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] GameObject Boss;
    [SerializeField] Vector3 pos;
    Transform player;
    GameObject blackScreen;
    bool detect = false;
    Transform mainTerrain;
    private void OnEnable()
    {
        player = FindObjectOfType<playerMVT>().transform;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        blackScreen = player.GetChild(1).GetChild(11).gameObject;
        mainTerrain = FindObjectOfType<CustomTerrain>().transform;
       StartCoroutine( CreateRocks(0));
    }
    IEnumerator vsbOSS(playerMVT pmvt)
    {
        yield return new WaitForSeconds(2f);
        Instantiate(Boss, player.position + new Vector3(100f, 0, 0), Quaternion.identity);
        pmvt.playerCanMove = true;
        blackScreen.SetActive(false);

    }
    void goToBoss()
    {
        blackScreen.SetActive(true);
        playerMVT pmvt = player.GetComponent<playerMVT>();
        pmvt.playerCanMove = false;
        FindObjectOfType<spawner1>().ended = true;
        StartCoroutine(vsbOSS(pmvt));
        player.position = pos;


    }
    IEnumerator CreateRocks(int i)
    {
        yield return new WaitForSeconds(0.1f);
        for (int j = 0; j < 4;j++)
        {
            int x = Random.Range(-50, 50);
            int z = Random.Range(-50, 50);
            Vector3 pos = transform.position + new Vector3(x, 1000, z);
            Ray r = new Ray(pos, -Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, Mathf.Infinity, terrainLayer))
            {
                GameObject b = Instantiate(rocks, hit.point, Quaternion.identity);
                moveFire move = b.GetComponent<moveFire>();
                move.isRock = true;
                move.setStuff(0, (transform.position - hit.point).normalized, 50f);
                Destroy(b, 10f);
            }
       
        }
        if (i < rocksNumber)
            StartCoroutine(CreateRocks(++i));
        else
            nextAnimation();

    }
    IEnumerator end()
    {
        yield return new WaitForSeconds(4f);
        animator.SetBool("end", true);
    }
    void nextAnimation()
    {
        animator.SetBool("Down", true);
        detect = true;
        StartCoroutine(end());
        Destroy(effect);
        StartCoroutine(fix());
    }
    IEnumerator fix()
    {
        yield return new WaitForSeconds(3f);
        animator.SetBool("Down", false);
    }
    private void Update()
    {
        if(detect)
            controller.Move(-gravity * Vector3.up * Time.deltaTime);
        if (Physics.Raycast(transform.position,-Vector3.up,5f, terrainLayer) && detect)
        {
            animator.SetBool("Down", false);
        }
    }
}
