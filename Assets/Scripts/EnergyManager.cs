using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 能量（体力）管理器。统一管理「看激励视频回满能量」的入口：
///   1. EnergyManager 节点下的「Dron」（世界物体：BoxCollider2D + Animator，tag 为 Dron）
///   2. HUD 上的「jiliBtn」按钮（原本 Button 上只有一条 SetActive 持久调用，弹窗被自己的 Awake 立刻关掉，等于点不出广告）
///
/// 两处点击后统一走：AdConfirm 确认弹窗 → 看完激励视频 → LevelManager.FillEnergy() 把能量加满。
///
/// 说明：
/// 1. Dron 不是 UI 按钮，用 Physics2D.OverlapPoint 做点击检测，不改场景、不加 UI 射线遮挡。
/// 2. jiliBtn 的持久调用（SetActive AdConfirm 实例）仍保留：UnityEvent 先执行持久调用，再执行这里 AddListener 的回调。
///    场景里那个 AdConfirm 实例是 inactive 的，Awake 要等激活才跑，跑完会把自己再隐藏；
///    所以这里补一次 AdConfirm.Show(...)，它会把实例重新打开并设置奖励回调，弹窗与广告才真正生效。
/// 3. Dron 的点击放在「抬起」触发：避免同一次点击的抬起误落到刚弹出的确认按钮上。
/// </summary>
public class EnergyManager : MonoBehaviour
{
    /// <summary>Dron 的标签，改了场景 tag 时同步改这里。</summary>
    private const string DronTag = "Dron";

    public static EnergyManager Instance { get; private set; }

    /// <summary>
    /// 按下（或按住）时指针是否落在 Dron 上。
    /// 现场计算而不是在 Update 里缓存，避免脚本执行顺序不同导致 LevelManager 读到上一帧的旧值。
    /// </summary>
    public static bool IsPointerOverEnergy
    {
        get
        {
            EnergyManager ins = Instance;
            if (ins == null) return false;
            if (!Input.GetMouseButton(0)) return false;
            return ins.IsPointerOnDron(Input.mousePosition);
        }
    }

    [Header("Dron 物体（留空则按 tag 自动查找）")]
    public Transform dron;

    [Header("额外奖励入口按钮名（点击同样走激励视频，留空则不绑定）")]
    public string[] extraRewardButtons = { "jiliBtn" };

    [Header("参数")]
    [Tooltip("能量已满时点击 Dron 是否仍允许看广告")]
    public bool allowWhenFull = false;

    private LevelManager levelManager;
    private Camera mainCamera;
    private readonly Collider2D[] hitBuffer = new Collider2D[8];
    private readonly List<Button> boundButtons = new List<Button>();
    private bool pressedOnDron;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
        mainCamera = Camera.main;

        if (dron == null)
        {
            ResolveDron();
        }

        BindExtraRewardButtons();
    }

    private void OnDestroy()
    {
        UnbindExtraRewardButtons();
        if (Instance == this) Instance = null;
    }

    private void Update()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        if (Input.GetMouseButtonDown(0))
        {
            pressedOnDron = IsPointerOnDron(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 必须「按下和抬起都在 Dron 上」才算点击，防止从别处拖过来误触
            if (pressedOnDron && IsPointerOnDron(Input.mousePosition))
            {
                OnDronClick();
            }

            pressedOnDron = false;
        }
    }

    /// <summary>点击 Dron：能量没满时弹出激励视频确认框。</summary>
    private void OnDronClick()
    {
        if (!allowWhenFull && IsEnergyFull())
        {
            Debug.Log("[能量] 能量已满，无需观看广告");
            return;
        }

        OnRewardEntryClick();
    }

    /// <summary>
    /// 奖励入口统一调用：弹出确认框，看完激励视频把能量加满。
    /// 供按钮（jiliBtn）与外部逻辑复用，不在这里判断能量是否已满——按钮保留原有「随时可打开」的行为。
    /// </summary>
    public void OnRewardEntryClick()
    {
        ComponentnUI ui = FindAnyObjectByType<ComponentnUI>();
        if (ui != null && ui.CheckSettings)
        {
            Debug.Log("[能量] 设置面板打开中，忽略奖励入口点击");
            return;
        }

        if (levelManager == null) levelManager = FindAnyObjectByType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogWarning("[能量] 场景中没有 LevelManager，无法发放能量奖励");
            return;
        }

        Debug.Log("[能量] 请求激励视频确认框");
        AdConfirm.Show(OnRewardGranted);
    }

    private void OnRewardGranted()
    {
        if (levelManager == null) levelManager = FindAnyObjectByType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogWarning("[能量] 激励视频完成，但找不到 LevelManager，无法加满能量");
            return;
        }

        levelManager.FillEnergy();
        Debug.Log("[能量] 激励视频完成，能量已加满");
    }

    private bool IsEnergyFull()
    {
        if (levelManager == null) return false;
        int max = levelManager.StartMoving > 0 ? levelManager.StartMoving : levelManager.TotalRound;
        return max > 0 && levelManager.TotalRound >= max;
    }

    /// <summary>按 tag 找到场景里的 Dron。</summary>
    private void ResolveDron()
    {
        GameObject go = GameObject.FindWithTag(DronTag);
        if (go != null)
        {
            dron = go.transform;
            return;
        }

        Debug.LogWarning($"[能量] 未找到 tag 为 {DronTag} 的 Dron，请在 EnergyManager 上手动指定");
    }

    /// <summary>
    /// 把额外入口按钮（默认 jiliBtn）接到同一套激励视频逻辑上。
    /// 只 AddListener，不动场景里原有的持久调用，原行为（打开 AdConfirm 面板）保持不变。
    /// </summary>
    private void BindExtraRewardButtons()
    {
        if (extraRewardButtons == null) return;

        foreach (string buttonName in extraRewardButtons)
        {
            if (string.IsNullOrEmpty(buttonName)) continue;

            GameObject go = GameObject.Find(buttonName);
            if (go == null)
            {
                Debug.LogWarning($"[能量] 未找到奖励入口按钮「{buttonName}」");
                continue;
            }

            Button btn = go.GetComponent<Button>();
            if (btn == null)
            {
                Debug.LogWarning($"[能量]「{buttonName}」上没有 Button 组件，跳过绑定");
                continue;
            }

            // 先移除再添加，避免热重载或重复初始化时触发两次
            btn.onClick.RemoveListener(OnRewardEntryClick);
            btn.onClick.AddListener(OnRewardEntryClick);

            if (!boundButtons.Contains(btn)) boundButtons.Add(btn);
            Debug.Log($"[能量] 已绑定奖励入口按钮：{buttonName}");
        }
    }

    private void UnbindExtraRewardButtons()
    {
        foreach (Button btn in boundButtons)
        {
            if (btn != null) btn.onClick.RemoveListener(OnRewardEntryClick);
        }

        boundButtons.Clear();
    }

    /// <summary>屏幕上这个点是否压在 Dron 的碰撞体上。</summary>
    private bool IsPointerOnDron(Vector2 screenPosition)
    {
        // 配置了 dron 就只判定它自己的碰撞体，避免场景里其它同名 tag 干扰
        if (dron != null)
        {
            return IsHitDron(dron, screenPosition);
        }

        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return false;

        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPosition);

        // 该点可能同时命中多个碰撞体（背景、地面等），逐个检查 tag
        int count = Physics2D.OverlapPointNonAlloc(worldPoint, hitBuffer);
        for (int i = 0; i < count; i++)
        {
            Collider2D hit = hitBuffer[i];
            if (hit != null && hit.CompareTag(DronTag)) return true;
        }

        return false;
    }

    private bool IsHitDron(Transform target, Vector2 screenPosition)
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return false;

        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPosition);
        int count = Physics2D.OverlapPointNonAlloc(worldPoint, hitBuffer);
        for (int i = 0; i < count; i++)
        {
            Collider2D hit = hitBuffer[i];
            if (hit != null && hit.transform == target) return true;
        }

        return false;
    }
}
