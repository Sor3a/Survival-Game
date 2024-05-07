using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weather : MonoBehaviour
{
    [SerializeField] int numberOfStuff;
    DayAndNight timing;
    [SerializeField] GameObject rock,thunder;
    Transform player;
    [SerializeField] float maxDistance;
    private void Awake()
    {
        timing = FindObjectOfType<DayAndNight>();
        player = FindObjectOfType<playerStats>().transform;
    }
    private void Start()
    {
        StartCoroutine(startWeather(Random.Range(600f,800f)));
    }
    IEnumerator rocks(int a)
    {
        yield return new WaitForSeconds(2f);
        if(timing.day<10)
        {
            for (int i = 0; i < Random.Range(2, 4); i++)
            {
                float x = player.position.x+ Random.Range(-maxDistance, maxDistance);
                float z = player.position.z+ Random.Range(-maxDistance, maxDistance);
                float y = player.position.y + 200f;
                GameObject b = Instantiate(rock, new Vector3(x, y, z), Quaternion.identity);
                moveFire s = b.GetComponent<moveFire>();
                s.setStuff(20, (new Vector3(transform.position.x + Random.Range(-10, 10), 0f, transform.position.z + Random.Range(-10, 10)) - transform.position).normalized, 100f);
                s.isRock = true;   
                Destroy(b,5f);
            }
            if (a < numberOfStuff)
                StartCoroutine(rocks(++a));
        }
    }
    IEnumerator thunders(int a)
    {
        yield return new WaitForSeconds(2f);
        if (timing.day < 10)
        {
            for (int i = 0; i < Random.Range(2, 4); i++)
            {
                float x = player.position.x + Random.Range(-maxDistance, maxDistance);
                float z = player.position.z + Random.Range(-maxDistance, maxDistance);
                float y = player.position.y + 50f;
                GameObject b = Instantiate(thunder, new Vector3(x, y, z), Quaternion.identity);
                b.GetComponent<ParticleSystem>().Play();
                b.GetComponent<AudioSource>().Play();
                Destroy(b, 4f);
            }
            if (a < numberOfStuff)
                StartCoroutine(thunders(++a));
        }

    }
    IEnumerator startWeather(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("ff");
        StartCoroutine(rocks(0));
        StartCoroutine(thunders(0));
        StartCoroutine(startWeather(Random.Range(1000f, 1500f)));
    }
    void Update()
    {

    }
}
