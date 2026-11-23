using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;

public class LevelsListed : MonoBehaviour
{
    [Header("Integer Controller")][Range(0, 15)]
    internal int CurrentLevel = 1;

    [Header("List Images")]
    public Image[] ListLevels;

    [Header("Sprites Controller")]
    public Sprite LevelActive;
    public Sprite LevelActiveHigh;
    public Sprite LevelInactive;

    private int appliedLevel = -1;

    void Start()
    {
        CurrentLevel = PlayerPrefs.GetInt("Level") + 1;
        ApplyState();
    }
    void Update()
    {
        // 只在进度变化时重刷，避免每帧设置 sprite 触发 UI 重建
        if (CurrentLevel != appliedLevel)
        {
            ApplyState();
        }
    }

    /// <summary>
    /// 按当前进度刷新关卡图标。进度可能超过图片数量（关卡数据比大厅图标多），
    /// 这里统一做钳制，避免索引越界每帧抛异常。
    /// </summary>
    private void ApplyState()
    {
        appliedLevel = CurrentLevel;

        if (ListLevels == null || ListLevels.Length == 0) return;

        int current = Mathf.Clamp(CurrentLevel, 0, ListLevels.Length);

        for (int i = 0; i < current; i++)
        {
            if (ListLevels[i] != null) ListLevels[i].sprite = LevelActive;
        }

        for (int i = current + 1; i < ListLevels.Length; i++)
        {
            if (ListLevels[i] != null) ListLevels[i].sprite = LevelInactive;
        }
    }
}
