using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Controller Tutorial")]
    public GameObject TutorialOne;
    public GameObject TutorialTwo;
    public GameObject TutorialThree;

    [Header("Controller Drawing")]
    public GameObject DrawingTutorialOne;
    public GameObject DrawingTutorialTwo;
    public GameObject DrawingTutorialThree;


    [Header("GameObjects Secondair")]
    public GameObject[] ListActivateed;

    [Header("Boolean Manager")]
    internal bool TutorialOneBool = false;
    internal bool TutorialOneDrawingBool = false;
    internal bool TutorialTwoBol = false;
    internal bool TutorialTwoDrawingBool = false;
    internal bool TutorialThreeBool = false;
    internal bool TutorialThreeDrawingBool = false;

    [Header("Helper Bool")]
    internal bool OneBool = false;
    internal bool TwoBool = false;
    internal bool ThreeBool = false;
    private void Update()
    {
        if(PlayerPrefs.GetString("FirstTutorialShow") == "")
        {
            CheckMouseUpAgain();
            if (TutorialOneBool == false)
            {
                Time.timeScale = 0.3f;
                TutorialOne.SetActive(true);
            }
            else
            {
                TutorialOne.SetActive(false);
                if (Input.GetMouseButtonDown(0))
                {
                    OneBool = true;
                }
                if(OneBool && Input.GetMouseButtonUp(0))
                {
                    TutorialOneDrawingBool = true;
                }
                if(TutorialOneDrawingBool == false)
                {
                    DrawingTutorialOne.SetActive(true);
                }
                else
                {
                    DrawingTutorialOne.SetActive(false);
                    if(TutorialTwoBol == false)
                    {
                        TutorialTwo.SetActive(true);
                    }
                    else
                    {
                        TutorialTwo.SetActive(false);
                        if (Input.GetMouseButtonDown(0))
                        {
                            TwoBool = true;
                        }
                        if(TwoBool && Input.GetMouseButtonUp(0))
                        {
                            TutorialTwoDrawingBool = true;
                        }
                        if(TutorialTwoDrawingBool == false)
                        {
                            DrawingTutorialTwo.SetActive(true);
                        }
                        else
                        {
                            DrawingTutorialTwo.SetActive(false);
                            if(TutorialThreeBool == false)
                            {
                                TutorialThree.SetActive(true);
                            }
                            else
                            {
                                TutorialThree.SetActive(false);
                                if (Input.GetMouseButtonDown(0))
                                {
                                    ThreeBool = true;
                                }
                                if(ThreeBool && Input.GetMouseButtonUp(0))
                                {
                                    TutorialThreeDrawingBool = true;
                                }
                                if(TutorialThreeDrawingBool == false)
                                {
                                    DrawingTutorialThree.SetActive(true);
                                }
                                else
                                {
                                    DrawingTutorialThree.SetActive(false);
                                    Time.timeScale = 1f;
                                    PlayerPrefs.SetString("FirstTutorialShow", "Done");
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            foreach(GameObject Obj in ListActivateed)
            {
                Obj.SetActive(true);
            }
        }
    }
    void CheckMouseUpAgain()
    {

    }
    public void ClickOne()
    {
        TutorialOneBool = true;
    }
    public void ClickTwo()
    {
        TutorialTwoBol = true;
    }
    public void ClickThree()
    {
        TutorialThreeBool = true;
    }
}
