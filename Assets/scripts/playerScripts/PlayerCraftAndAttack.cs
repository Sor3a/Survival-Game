using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraftAndAttack : MonoBehaviour
{
    [SerializeField] GameObject water;
    playerStats stats;
    bool inWater;
    [SerializeField] GameObject settingsPanel;
    panelsControle manager;
    DayAndNight d;
    private void Awake()
    {
        d = FindObjectOfType<DayAndNight>();
        stats = GetComponent<playerStats>();
        manager = FindObjectOfType<panelsControle>();
    }
    IEnumerator getHealth()
    {
        yield return new WaitForSeconds(0.2f);
        stats.getHealth(1);
        if (inWater)
            StartCoroutine(getHealth());
    }
    private void Update()
    {
        if (transform.position.y < 34.4f)
        {
            if (water.activeSelf == false)
                water.SetActive(true);
            if (!inWater)
            {
                StartCoroutine(getHealth());
                inWater = true;
            }
        }
        else
        {
            if (water.activeSelf == true)
                water.SetActive(false);
            inWater = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
                returneTime();
            else
            {
                Time.timeScale = 0;
                d.PauseTheGame(true);
                settingsPanel.SetActive(true);
                Cursor.visible = true;
            }
                
        }
    }
    public void returneTime()
    {
        if(!manager.isPanelOpen())
        Cursor.visible = false;
        Time.timeScale = 1;
        d.PauseTheGame(false);
        settingsPanel.SetActive(false);
    }
}
