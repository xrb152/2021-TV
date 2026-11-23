using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UiWin : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject CloudUI;

    [Header("Transformer")]
    public Transform PointRotation;
    public Transform circleCenter;

    [Header("floating Controller")]
    internal float SpeedRotation = 200;
    internal float[] values = new float[] { 45f, 90f, 135f, 180f, 225f, 270f, 315f, 360f }; // Array of values for each division

    [Header("Integer Controller")]
    private int currentIndex = 0; // Current index in the values array
    private int Coins = 24;
    private int Gems = 1;

    [Header("UI Shower")]
    public TextMeshProUGUI RewaredUICoins;
    public TextMeshProUGUI RewaredUIGems;

    [Header("Button Rewared")]
    public TextMeshProUGUI CoinsUI;
    public TextMeshProUGUI GemsUI;

    [Header("Boolean Manager")]
    internal bool LoadingCircle = true;
    private void Start()
    {
        RewaredUICoins.text = Coins + "";
        RewaredUIGems.text = Gems + "";
    }
    private void Update()
    {
        if (LoadingCircle)
        {
            float horizontalInput = 1;

            // Calculate the rotation amount based on the input and rotation speed
            float rotationAmount = -horizontalInput * SpeedRotation * Time.deltaTime;

            // Rotate the arrow around the circle center
            PointRotation.RotateAround(circleCenter.position, Vector3.forward, rotationAmount);

            // Check if the arrow has crossed a division boundary
            if (rotationAmount < 0 && PointRotation.rotation.eulerAngles.z <= values[currentIndex])
            {
                // Increment the current index
                currentIndex = (currentIndex + 1) % values.Length;
            }
            else if (rotationAmount > 0 && PointRotation.rotation.eulerAngles.z >= values[currentIndex])
            {
                // Decrement the current index
                currentIndex = (currentIndex - 1 + values.Length) % values.Length;
            }
            CoinsUI.text = Coins * currentIndex  + "";
            GemsUI.text = Gems * currentIndex  + "";
        }
    }
    public void ClaimReward()
    {
        LoadingCircle = false;
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + Coins * currentIndex);
        PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + Gems * currentIndex);
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
    }
    public void Nothanks()
    {
        LoadingCircle = false;
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + Coins);
        PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + Gems);
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        StartCoroutine(LoadingNextScene());
    }
    IEnumerator LoadingNextScene()
    {
        CloudUI.SetActive(true);
        CloudUI.gameObject.GetComponent<Animator>().Play("CloudPos 0");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
