using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Manager Controller")]
    public GameObject CloudUI;

    public void SetActivateCloud()
    {
        CloudUI.SetActive(true);
        CloudUI.GetComponent<Animator>().Play("CloudAnimation");
    }
}
