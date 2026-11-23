using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSpawning : MonoBehaviour
{
    [Header("Manager")]
    public Sprite[] ListBarrel;
    public Sprite[] ListMeele;
    public Sprite[] ListRange;

    [Header("Manager Controller")]
    private UIManager ManagerUI;

    private void Awake()
    {
        ManagerUI = GetComponent<UIManager>();
        /////
        ///
        ManagerUI.IconLeft.sprite = ListBarrel[PlayerPrefs.GetInt("Level")];
        ManagerUI.IconCenter.sprite = ListMeele[PlayerPrefs.GetInt("Level")];
        ManagerUI.IconRight.sprite = ListRange[PlayerPrefs.GetInt("Level")];
    }
}
