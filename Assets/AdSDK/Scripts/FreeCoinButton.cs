using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HUD 上的「免费金币」按钮：点击弹出广告确认框，看完抖音激励视频发放金币。
///
/// 原实现依赖 HyperWe.CurrencyHolder / njector / AutoInjectTarget 这套依赖注入框架，
/// 但本项目并未包含这些程序集，导致 Assembly-CSharp 编译失败。
/// 这里改为自包含的本地金币存取（PlayerPrefs），保留按钮 → AdConfirm 弹窗 → 发奖励的完整链路。
/// 如果以后接入了项目的金币系统，只需把 GrantReward() 里的 AddCoins 换成对应接口即可。
/// </summary>
[RequireComponent(typeof(Button))]
public class FreeCoinButton : MonoBehaviour
{
    private const string CoinPrefsKey = "PlayerCoins";

    [SerializeField]
    private int rewardAmount = 200;

    private Button button;

    /// <summary>当前金币数。</summary>
    public static int GetCoins()
    {
        return PlayerPrefs.GetInt(CoinPrefsKey, 0);
    }

    /// <summary>增减金币，返回变化后的金币数。</summary>
    public static int AddCoins(int amount)
    {
        int total = Mathf.Max(0, GetCoins() + amount);
        PlayerPrefs.SetInt(CoinPrefsKey, total);
        PlayerPrefs.Save();
        return total;
    }

    private void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnClick);
        }
    }

    public void OnClick()
    {
        Debug.Log("[广告] 点击免费金币按钮，弹出激励视频确认框");
        AdConfirm.Show(GrantReward);
    }

    private void GrantReward()
    {
        int total = AddCoins(rewardAmount);
        Debug.Log($"[广告] 激励视频完成，发放金币 {rewardAmount}，当前金币 {total}");
    }
}
