using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Manager Controller")]
    public LevelManager ManagerController;
    public PoolAlly AllyManager;

    [Header("UI Shower")]
    public Sprite UIActive;
    public Sprite UIInactive;

    [Header("UI Objects")]
    public Image UILeft;
    public Image UICenter;
    public Image UIRight;

    [Header("Icons")]
    public Image IconLeft;
    public Image IconRight;
    public Image IconCenter;

    void Start()
    {
        ReactiveAll();
    }
    private void Update()
    {
        if(ManagerController.TotalRound < 6)
        {
            UILeft.transform.localScale = new Vector3(1f, 1f, 1f);
            UILeft.sprite = UIInactive;
        }
        if (ManagerController.TotalRound < 13)
        {
            UICenter.transform.localScale = new Vector3(1f, 1f, 1f);
            UICenter.sprite = UIInactive;
        }
        if (ManagerController.TotalRound < 30)
        {
            UIRight.transform.localScale = new Vector3(1f, 1f, 1f);
            UIRight.sprite = UIInactive;
        }
    }
    void ReactiveAll()
    {
        UILeft.transform.localScale = new Vector3(1f, 1f, 1f);
        UICenter.transform.localScale = new Vector3(1f, 1f, 1f);
        UIRight.transform.localScale = new Vector3(1f, 1f, 1f);
        ////
        UILeft.sprite = UIInactive;
        UICenter.sprite = UIInactive;
        UIRight.sprite = UIInactive;
    }
    public void ClickLeft()
    {
        ReactiveAll();
        if (ManagerController.TotalRound > 3)
        {
            UILeft.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            UILeft.sprite = UIActive;
            ManagerController.SpawnLocation = "PoolAlly";
            ManagerController.UIActivation = UILeft;
            ManagerController.UIInactive = UIInactive;
            ManagerController.CurrentFactor = 6;
            ManagerController.TotalMover = 6;
            ManagerController.SelectedMode = true;
            ManagerController.AutorizatedSpawn = true;

            /////
            GameObject Object = ManagerController.PooledObjects;
            foreach(GameObject ActiveBarrel in AllyManager.ListBarrel)
            {
                if(IconLeft.sprite.name == ActiveBarrel.name)
                {
                    ActiveBarrel.transform.rotation = Quaternion.Euler(0, 0, 0);
                    ManagerController.PooledObjects = ActiveBarrel;
                    Object = ActiveBarrel;
                }
            }
        }
    }
    public void ClickCenter()
    {
        ReactiveAll();
        if (ManagerController.TotalRound > 3)
        {
            UICenter.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            UICenter.sprite = UIActive;
            ManagerController.SpawnLocation = "PoolAlly";
            ManagerController.UIActivation = UICenter;
            ManagerController.UIInactive = UIInactive;
            ManagerController.CurrentFactor = 13;
            ManagerController.TotalMover = 13;
            ManagerController.SelectedMode = true;
            ManagerController.AutorizatedSpawn = true;

            /////
            GameObject Object = ManagerController.PooledObjects;
            foreach(GameObject ActiveMelee in AllyManager.ListAllyMelee)
            {
                if (IconCenter.sprite.name == ActiveMelee.name)
                {
                    ActiveMelee.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    ManagerController.PooledObjects = ActiveMelee;
                    Object = ActiveMelee;
                }
            }
        }
    }
    public void ClickRight()
    {
        ReactiveAll();
        if (ManagerController.TotalRound > 3)
        {
            UIRight.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            UIRight.sprite = UIActive;
            ManagerController.SpawnLocation = "PoolAlly";
            ManagerController.UIActivation = UIRight;
            ManagerController.UIInactive = UIInactive;
            ManagerController.CurrentFactor = 30;
            ManagerController.TotalMover = 30;
            ManagerController.SelectedMode = true;
            ManagerController.AutorizatedSpawn = true;

            /////
            GameObject Object = ManagerController.PooledObjects;
            foreach(GameObject ActiveRange in AllyManager.ListAllyRange)
            {
                if (IconRight.sprite.name == ActiveRange.name)
                {
                    ActiveRange.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    ManagerController.PooledObjects = ActiveRange;
                    Object = ActiveRange;
                }
            }
        }
    }
}
