using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExitgAME : MonoBehaviour
{
    public void exit()
    {
        Application.Quit();
    }
    public void donation()
    {
        Application.OpenURL("https://www.buymeacoffee.com/Sor3a");
    }
}
