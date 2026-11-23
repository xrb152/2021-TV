using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentnUI : MonoBehaviour
{
    [Header("Panels List")]
    public GameObject PanelSettings;

    [Header("插屏广告")]
    [Tooltip("打开设置面板时展示一次插屏广告")]
    public bool showInterstitialOnOpen = true;
    [Tooltip("本组件两次插屏的最小间隔（秒）。SDK 自身还有 30 秒间隔限制，这里只是避免同一次暂停内重复请求")]
    public float interstitialCooldown = 30f;

    [Header("Boolean Manager")]
    internal bool CheckSettings = false;

    private float lastInterstitialTime = -999f;

    public void OpenSettings()
    {
        CheckSettings = !CheckSettings;
        if (CheckSettings)
        {
            PanelSettings.SetActive(true);
            Time.timeScale = 0f;
            TryShowInterstitial();
        }
        else if (!CheckSettings)
        {
            PanelSettings.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// 打开设置面板时请求一次插屏广告。
    /// 用 unscaledTime 计时：此刻 Time.timeScale 已经是 0，普通 Time.time 不再走动。
    /// </summary>
    private void TryShowInterstitial()
    {
        if (!showInterstitialOnOpen) return;

        if (Time.unscaledTime - lastInterstitialTime < interstitialCooldown)
        {
            Debug.Log("[广告] 距离上次插屏不足冷却时间，本次跳过");
            return;
        }

        AdManager ad = AdManager.Instance;
        if (ad == null)
        {
            Debug.LogWarning("[广告] 场景中没有 AdManager，跳过设置面板插屏");
            return;
        }

        lastInterstitialTime = Time.unscaledTime;
        Debug.Log("[广告] 打开设置面板，请求插屏广告");
        ad.ShowInterstitialAd();
    }
}
