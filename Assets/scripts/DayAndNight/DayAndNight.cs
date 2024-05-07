using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//[ExecuteAlways]
public class DayAndNight : MonoBehaviour
{
    public Gradient AmbientColor;
    //public Gradient DirectionalColor;
    public Gradient FogColor;
    [SerializeField] Light DirectionalLight;
    [SerializeField, Range(0, 24)] public float TimeOfDay;
    [SerializeField] float slower = 1;
    public int day = -1;
    float initialDate;
    bool DayB;
    [SerializeField] TextMeshProUGUI DayText, dT, hT;
    [SerializeField] GameObject endBoss;
    [SerializeField] Material dayMat, nightMat;
    Material putedTexutre;
    bool Paused = false;
    private void Start()
    {
        dT.text = "Day : " + 1;
        DayB = false;
        day = -1;
        initialDate = TimeOfDay;
        StartCoroutine(showDayAfter());
    }
    void showDayText()
    {
        if (showDay)
        {
            DayText.text = "DAY " + (day + 1).ToString();
            if (a < 4)
            {
                DayText.color = new Color(1, 1, 1, a += .01f);
            }
            else
            {
                showDay = false;
                DayText.color = new Color(1, 1, 1, 0);
                a = 0;
            }
        }
    }
    void changeMat()
    {
        if (TimeOfDay > 5 && TimeOfDay < 20)
        {
            if (putedTexutre != dayMat)
            {

                // RenderSettings.skybox.mainTexture = dayMat;
                putedTexutre = dayMat;
                RenderSettings.skybox = dayMat;
            }

        }
        else
        {
            if (putedTexutre != nightMat)
            {
                Debug.Log("i fking work");
                //RenderSettings.skybox.mainTexture = nightMat;

                RenderSettings.skybox = nightMat;
                putedTexutre = nightMat;
            }
        }
    }
    IEnumerator showDayAfter()
    {
        yield return new WaitForSeconds(5f);
        showDay = true;
    }
    bool showDay;
    float a = 0;
    [SerializeField] bool work = true;
    public void PauseTheGame(bool state)
    {
        Paused = state;
    }
    private void Update()
    {
        changeMat();
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("fd");
            day++;
        }
        if (Application.isPlaying)
        {
            if (work && !Paused)
                TimeOfDay += Time.deltaTime / slower;
            TimeOfDay %= 24;
            updateLighting(TimeOfDay / 24);
            int h = (int)TimeOfDay;
            float min = TimeOfDay - (float)h;
            min = (min * 0.6f) * 100;
            if (min < 10)
                hT.text = "Time : " + h + ":0" + (int)min;
            else
                hT.text = "Time : " + h + ":" + (int)min;
        }
        else
            updateLighting(TimeOfDay / 24);
        if (Mathf.Abs(initialDate - TimeOfDay) < 0.01f && !DayB)
        {
            if (day != -1)
                showDay = true;
            day++;
            dT.text = "Day : " + (day + 1).ToString();
            DayB = true;
            StartCoroutine(returneDay());
        }
        showDayText();
        if (day == 10 && TimeOfDay > 9 && !ended)
            End();

            

    }
    [HideInInspector] public bool ended = false;
    void End()
    {
        ended = true;
        GameObject b = Instantiate(endBoss, FindObjectOfType<playerMVT>().transform.position + new Vector3(0, 40f, 0), Quaternion.identity);
    }
    IEnumerator returneDay()
    {
        yield return new WaitForSeconds(2f);
        DayB = false;
    }
    void updateLighting(float timePercentage)
    {
        RenderSettings.ambientLight = AmbientColor.Evaluate(timePercentage);
        RenderSettings.fogColor = FogColor.Evaluate(timePercentage);
        if (DirectionalLight)
        {
            // DirectionalLight.color = DirectionalColor.Evaluate(timePercentage);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercentage * 360f) - 90f, 170f, 0));
        }
    }
    void OnValide()
    {
        if (DirectionalLight != null)
            return;
        if (RenderSettings.sun != null)
            DirectionalLight = RenderSettings.sun;
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (var light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                }

            }
        }
    }
}
