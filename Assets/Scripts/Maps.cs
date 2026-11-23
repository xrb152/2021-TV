using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour
{
    [Header("Controller Warnings")]
    internal GameObject WarningTop;
    internal GameObject WarningLeft;
    internal GameObject WarningRight;

    private void Awake()
    {
        WarningTop = transform.Find("Warning").transform.GetChild(0).transform.GetChild(0).gameObject;
        WarningLeft = transform.Find("Warning").transform.GetChild(1).transform.GetChild(0).gameObject;
        WarningRight = transform.Find("Warning").transform.GetChild(2).transform.GetChild(0).gameObject;
    }
}
