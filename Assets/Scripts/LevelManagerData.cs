using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerData : MonoBehaviour
{
    [Header("Manager Controller")]
    private LevelManager Manager;
    private CharManager ManagerCharacter;
    private PoolEnermy EnemyPool;
    private UiLobbyManager LobbyController;

    private void Awake()
    {
        Manager = FindAnyObjectByType<LevelManager>();
        ManagerCharacter = FindAnyObjectByType<CharManager>();
        EnemyPool = FindAnyObjectByType<PoolEnermy>();
        LobbyController = FindAnyObjectByType<UiLobbyManager>();
        /// Next Addition
        CheckCurrentLevels();
    }
    void CheckCurrentLevels()
    {
        if(PlayerPrefs.GetInt("Level") + 1 == 1)
        {
            Manager.TotalRound = 130;
            EnemyPool.TotalEnemy = 8;
            EnemyPool.EnemyUp = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 2)
        {
            Manager.TotalRound = 240;
            EnemyPool.TotalEnemy = 13;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyRight = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 3)
        {
            Manager.TotalRound = 270;
            EnemyPool.TotalEnemy = 16;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyLeft = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 4)
        {
            Manager.TotalRound = 270;
            EnemyPool.TotalEnemy = 15;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyLeft = true;
            EnemyPool.EnemyRight = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 5)
        {
            Manager.TotalRound = 290;
            EnemyPool.TotalEnemy = 20;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyLeft = true;
            EnemyPool.EnemyRight = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 6)
        {
            Manager.TotalRound = 290;
            EnemyPool.TotalEnemy = 16;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyRight = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 7)
        {
            Manager.TotalRound = 290;
            EnemyPool.TotalEnemy = 20;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyLeft = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 8)
        {
            Manager.TotalRound = 300;
            EnemyPool.TotalEnemy = 23;
            EnemyPool.EnemyUp = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 9)
        {
            Manager.TotalRound = 320;
            EnemyPool.TotalEnemy = 25;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyLeft = true;
            EnemyPool.EnemyRight = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 10)
        {
            Manager.TotalRound = 330;
            EnemyPool.TotalEnemy = 30;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyRight = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 11)
        {
            Manager.TotalRound = 350;
            EnemyPool.TotalEnemy = 35;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyLeft = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 12)
        {
            Manager.TotalRound = 370;
            EnemyPool.TotalEnemy = 35;
            EnemyPool.EnemyUp = true;
            EnemyPool.EnemyLeft = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 13)
        {
            Manager.TotalRound = 400;
            EnemyPool.TotalEnemy = 40;
            EnemyPool.EnemyUp = true;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 14)
        {
            PlayerPrefs.SetInt("Level", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Manager.TotalRound = 420;
            EnemyPool.TotalEnemy = 45;
        }
        if (PlayerPrefs.GetInt("Level") + 1 == 15)
        {
            Manager.TotalRound = 600;
            EnemyPool.TotalEnemy = 50;
        }

        // 兜底：进度超出最后一关（关卡数据只配到 15 级），循环回第 1 关，
        // 否则会以默认配置一直跑下去，同时大厅关卡下标也会越界。
        int nextLevel = PlayerPrefs.GetInt("Level") + 1;
        if (nextLevel > 15)
        {
            Debug.LogWarning("[关卡] 进度 " + nextLevel + " 超出关卡数据范围，重置为第 1 关");
            PlayerPrefs.SetInt("Level", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
