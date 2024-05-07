using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crafRock : MonoBehaviour
{
    [HideInInspector] public List<GameObject> bla = new List<GameObject>();
    bool fine = false;
    [SerializeField] float timerToDestroy;
    DayAndNight s;
    craft c;

    private void OnEnable()
    {
        s = FindObjectOfType<DayAndNight>();
        c = GetComponent<craft>();
    }

    [System.Serializable]
    public class items
    {
       public item first;
       public item second;
    }
    public List<items> itemsToGive = new List<items>();
    private void Update()
    {
        if(!fine)
        {
            for (int i = 0; i < bla.Count; i++)
            {
                if (!bla[i])
                    bla.RemoveAt(i);
            }
            if (bla.Count == 0)
                fine = true;

            if (timerToDestroy > 0)
                timerToDestroy -= Time.deltaTime;
            else
            {
                for (int i = 0; i < bla.Count; i++)
                {
                    if (bla[i])
                        Destroy(bla[i]);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            c.Item = Random.Range(0, 2) > 0 ? itemsToGive[s.day].first : itemsToGive[s.day].second;
            c.dropeItem();
            Destroy(gameObject);
        }
        

    }
}
