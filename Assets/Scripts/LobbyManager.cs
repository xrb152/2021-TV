using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    [Header("Controller Manager")]
    public UiLobbyManager ManagerLobby;
    public UiMainLobby MainLobby;

    [Header("Ui Controller")]
    public TextMeshProUGUI UILevel;

    [Header("Text UI")]
    public TextMeshProUGUI Coins;
    public TextMeshProUGUI Gems;

    [Header("UiM")]
    public GameObject[] UIM;

    private void Start()
    {
        UILevel.text = "等级 " + (PlayerPrefs.GetInt("Level") + 1);
    }
    private void Update()
    {
        Coins.text = PlayerPrefs.GetInt("Coins") + "";
        Gems.text = PlayerPrefs.GetInt("Gems") + "";
        if(PlayerPrefs.GetInt("Level") > 2)
        {
            foreach(GameObject MM in UIM)
            {
                MM.SetActive(true);
            }
        }
    }
    public void StartGame()
    {
        UIController ControllerUI = FindAnyObjectByType<UIController>();
        PlayerPrefs.SetString("FirstLaunchHome", "Done");
        StartCoroutine(LoadingGame());
        ControllerUI.SetActivateCloud();
    }
    ///////////
    IEnumerator LoadingGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
