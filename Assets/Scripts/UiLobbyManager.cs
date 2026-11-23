using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLobbyManager : MonoBehaviour
{
    [Header("Levels")]
    public GameObject[] ListLevelOne;
    public GameObject[] ListLevelTwo;
    public GameObject[] ListLevelThree;

    [Header("List levels")]
    public GameObject[] ListLevels;

    private void Awake()
    {
        // 关卡进度可能超出大厅槽位数量（大厅只摆了有限的几个关卡位，而进度会一直累加），
        // 越界时落到最后一个槽位。原写法直接索引，一旦越界 Awake 抛异常，
        // 下面 Setting 初始化也不会执行。
        int levelIndex = Mathf.Max(0, PlayerPrefs.GetInt("Level"));
        if (ListLevels != null && ListLevels.Length > 0)
        {
            if (levelIndex >= ListLevels.Length)
            {
                Debug.LogWarning($"[大厅] 关卡进度 {levelIndex} 超出大厅槽位数量 {ListLevels.Length}，改为显示最后一个槽位");
                levelIndex = ListLevels.Length - 1;
            }

            if (ListLevels[levelIndex] != null)
            {
                ListLevels[levelIndex].SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("[大厅] UiLobbyManager.ListLevels 未配置，跳过关卡槽位激活");
        }

        if (PlayerPrefs.GetInt("Setting") == 00)
        {
            PlayerPrefs.SetString("Music", "1");
            PlayerPrefs.SetString("Sound", "1");
            PlayerPrefs.SetString("Haptic", "0");
            PlayerPrefs.SetInt("Setting", 11);
        }
    }
}
