using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{

    [SerializeField] GameObject panel;
   public void setNextScene()
    {
        panel.SetActive(true);
        next();
    }
    void next()
    {
        SceneManager.LoadScene(1);
    }

}
