using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFirstTime : MonoBehaviour
{
    [Header("Object UI")]
    public GameObject IntroLaunche;

    private void Start()
    {
        if(PlayerPrefs.GetString("FirstLaunchHome") == "")
        {
            IntroLaunche.SetActive(true);
        }
    }
}
