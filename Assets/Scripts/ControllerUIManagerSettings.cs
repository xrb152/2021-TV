using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ControllerUIManagerSettings : MonoBehaviour
{
    [Header("UI Container")]
    public GameObject UIObject;

    [Header("Controller UI")]
    public Image ButtonsClickable;
    public Image ObjectUI;

    [Header("Sprites Manager")]
    public Sprite ActiveSP;
    public Sprite InactiveSP;

    [Header("Manager Colors")]
    public Color ActiveColor;
    public Color InactiveColor;

    [Header("Boolean Manager")]
    internal bool CheckWorking = true;

    void Start()
    {
        ObjectUI = UIObject.GetComponent<Image>();
        if (PlayerPrefs.GetString(this.gameObject.name) == "1")
        {
            ObjectUI.color = ActiveColor;
            ButtonsClickable.sprite = ActiveSP;
            UIObject.transform.localPosition = new Vector3(18.5f, 0, 0);
        }
        else if (PlayerPrefs.GetString(this.gameObject.name) == "0")
        {
            ObjectUI.color = InactiveColor;
            ButtonsClickable.sprite = InactiveSP;
            UIObject.transform.localPosition = new Vector3(-18.5f, 0, 0);
        }
    }
    void Update()
    {
        CheckingState();
    }
    void CheckingState()
    {
        if (PlayerPrefs.GetString(this.gameObject.name) == "1")
        {
            ObjectUI.color = ActiveColor;
            ButtonsClickable.sprite = ActiveSP;
            UIObject.transform.localPosition = new Vector3(18.5f, 0, 0);
        }
        else if (PlayerPrefs.GetString(this.gameObject.name) == "0")
        {
            ObjectUI.color = InactiveColor;
            ButtonsClickable.sprite = InactiveSP;
            UIObject.transform.localPosition = new Vector3(-18.5f, 0, 0);
        }
    }
    public void SetVolume()
    {
        CheckWorking = !CheckWorking;
        if (CheckWorking) { PlayerPrefs.SetString(this.gameObject.name, "1"); } else if (CheckWorking == false) { PlayerPrefs.SetString(this.gameObject.name, "0"); }
    }
}
