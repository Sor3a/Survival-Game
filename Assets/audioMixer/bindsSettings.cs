using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bindsSettings : MonoBehaviour
{
    mixerManager m;
    private void Awake()
    {
        m = FindObjectOfType<mixerManager>();
    }
    private void OnEnable()
    {
        int i = 1;

        if(PlayerPrefs.GetString("1").Length>0)
        {
            Debug.Log("aa");
            foreach (Transform item in transform)
            {
                Transform s = item.GetChild(1);
                TMP_InputField t = s.GetComponent<TMP_InputField>();
                if(t)
                {
                    t.text = PlayerPrefs.GetString(i.ToString());
                    i++;
                }

            }
        }

    }
    private void OnDisable()
    {
        int i = 1;
        foreach (Transform item in transform)
        {
            Transform s = item.GetChild(1).GetChild(0);
            TMP_InputField b = item.GetChild(1).GetComponent<TMP_InputField>();
            string text = b.text;
            if (s.childCount == 3)
            {
                PlayerPrefs.SetString(i.ToString(), text.Length < 1 ? s.GetChild(1).GetComponent<TextMeshProUGUI>().text : text);
            }
            else
            {
                PlayerPrefs.SetString(i.ToString(), text.Length < 1 ? s.GetChild(0).GetComponent<TextMeshProUGUI>().text : text);
            }
            i++;
        }
        if (FindObjectOfType<playerMVT>())
            m.loadBinds();
    }
 
}
