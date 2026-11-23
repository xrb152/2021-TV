using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    [Header("UI Controller")]
    public Image FillingBar;
    public Slider SliderBar;

    [Header("Animator Controller")]
    public Animator ControllerAnimation;

    [Header("Floating Manager")]
    public float TimeToNextLoad = 0f;

    [Header("Scripts Controller")]
    public ScriptableObject[] ListObjects;

    [Header("Boolean Manager")]
    internal bool CheckLoaded = true;

    private void Start()
    {
        ManagerAds();
        if (SliderBar != null) SliderBar.gameObject.SetActive(true);

        // 防止 TimeToNextLoad 为 0 或负数导致除以 0（fillAmount 变 Infinity，进度条永远卡住）
        if (TimeToNextLoad <= 0f)
        {
            Debug.LogWarning("[Loading] TimeToNextLoad 未配置或<=0，使用默认值 5 秒");
            TimeToNextLoad = 5f;
        }

        if (FillingBar != null) FillingBar.fillAmount = 0f;
        if (SliderBar != null) SliderBar.value = 0f;
    }
    private void Update()
    {
        if (FillingBar == null || SliderBar == null) return;

        // 关键修复：原判断是 `fillAmount != 1`，但 fillAmount 累加时浮点精度会让它
        // 从 0.99 直接跳到 1.00x（略大于 1），`!= 1` 永远成立，导致进度填满后
        // 永不进入跳转分支 —— 这就是 Loading 场景黑屏卡住的根因。
        // 改用 >= 1，保证填满后必定触发跳转。
        if (FillingBar.fillAmount < 1f)
        {
            FillingBar.fillAmount = Mathf.Min(1f, FillingBar.fillAmount + Time.deltaTime / TimeToNextLoad);
            SliderBar.value = FillingBar.fillAmount;
        }
        else if (CheckLoaded)
        {
            if (ControllerAnimation != null) ControllerAnimation.Play("CloudAnimation");
            StartCoroutine(LoadingNextScene());
            CheckLoaded = false;
        }
    }
    void ManagerAds()
    {
        // ListObjects 为空是正常配置（当前项目未用），无需打印 "Null" 污染 Console。
        if (ListObjects == null || ListObjects.Length == 0)
        {
            Debug.Log("[Loading] ListObjects 为空，跳过附加对象初始化");
        }
    }
    IEnumerator LoadingNextScene()
    {
        yield return new WaitForSeconds(1f);

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // 已是最后一个场景，避免 LoadScene 索引越界抛异常
            Debug.LogWarning("[Loading] 已是 Build Settings 中最后一个场景，不再跳转");
        }
    }
}
