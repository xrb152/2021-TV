using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pathfinding;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Manager Controller")]
    public AstarPath FinderManager;
    public PoolAlly AllyManager;

    [Header("Controller UI")]
    public TextMeshProUGUI ValueActive;
    public TextMeshProUGUI UILevel;

    [Header("UI Bars")]
    public Image BarFilling;

    [Header("Objects")]
    public GameObject CloudUI;

    [Header("Audio Source")]
    public AudioSource Whoosh;
    public AudioSource WinSound;
    public AudioSource LoseSound;

    [Header("Integer Controller")]
    internal int TotalRound = 130;
    internal int CurrentFactor = 0;
    internal int TotalMover = 0;
    internal int StartMoving = 0;

    [Header("Internal Objects")]
    private Camera mainCamera;

    [Header("Floating Controller")]
    internal float DistanceBtw = 0.7f;

    [Header("Boolean Manager")]
    internal bool CheckClicked = false;
    internal bool StartPooling = false;
    internal bool AutorizatedSpawn = true;
    internal bool SpawnAuto = false;
    internal bool AccebeltityToTouch = true;
    internal bool SelectedMode = false;
    internal bool EnemyItsIN = false;
    internal bool EnemyEnterd = false;
    internal bool FinishGame = true;

    [Header("Strings Controller")]
    internal string CurrentMode = "";
    internal string SpawnLocation = "";

    [Header("Internal UI")]
    internal Image UIActivation;

    [Header("UI Showing")]
    internal Sprite UIInactive;

    [Header("Objects Controller")]
    public GameObject UIShowing;
    private void Start()
    {
        StartMoving = TotalRound;
        CloudUI.gameObject.SetActive(true);StartCoroutine(LoadingInactive());
        UILevel.text = "等级 " + (PlayerPrefs.GetInt("Level") + 1);
        FinderManager.scanOnStartup = true;
        FinderManager.Scan();
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if ((Input.GetMouseButtonUp(0)))
        {
            CheckClicked = false;
            StartPooling = false;
            AutorizatedSpawn = true;
            SpawnAuto = false;
            Whoosh.Stop();
        }
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // 设置面板打开时不响应操作；按住 Dron（看广告回能量）时不响应，避免顺带丢出一颗子弹
        if (mousePosition.y > -5.883055f
            && FindAnyObjectByType<ComponentnUI>().CheckSettings == false
            && EnergyManager.IsPointerOverEnergy == false)
        {
            if (AccebeltityToTouch && SelectedMode)
            {
                ManagerMouseClickable(); ManagerUI();
                if (PooledObjects != null)
                {
                    if (TotalMover >= CurrentFactor && CurrentFactor > 0)
                    {
                        if (StartPooling && TotalRound >= 1) { StartPoolingObjects(); }
                        if (TotalRound < 0) { TotalRound = 0; }
                    }
                    else if (UIActivation != null)
                    {
                        UIActivation.sprite = UIInactive;
                        UIActivation.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }
        }
        if (EnemyItsIN) { Debug.Log("You Lose"); }
    }
    private void LateUpdate()
    {
        if (EnemyEnterd)
        {
            GameObject[] EnemyCalculated = GameObject.FindGameObjectsWithTag("Enermy");
            if(EnemyCalculated.Length == 0)
            {
                Finish(true);
            }
        }   
    }
    internal void Finish(bool Win)
    {
        if (Win && FinishGame)
        {
            WinSound.Play();
            if(PlayerPrefs.GetInt("Level") > 2)
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                UIShowing.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                StartCoroutine(FinishGameLoading());
                CloudUI.SetActive(true);
                CloudUI.gameObject.GetComponent<Animator>().Play("CloudPos 0");
            }
            FinishGame = false;
            ShowInterstitialAd("关卡胜利");
        }
        if(Win == false && FinishGame)
        {
            LoseSound.Play();
            StartCoroutine(FinishGameLoading());
            CloudUI.SetActive(true);
            CloudUI.gameObject.GetComponent<Animator>().Play("CloudPos 0");
            FinishGame = false;
            ShowInterstitialAd("关卡失败");
        }
    }

    /// <summary>
    /// 胜利 / 失败时展示插屏广告。
    /// 展示时机放在结算面板弹出之后，避免广告盖住结算动画；
    /// 具体能否展示由 AdManager 内部判断（模块启动 15 秒内、两次间隔 30 秒内会自动跳过）。
    /// </summary>
    private void ShowInterstitialAd(string reason)
    {
        AdManager ad = AdManager.Instance;
        if (ad == null)
        {
            Debug.LogWarning($"[广告] {reason}：场景中没有 AdManager，跳过插屏广告");
            return;
        }

        Debug.Log($"[广告] {reason}，请求插屏广告");
        ad.ShowInterstitialAd();
    }
    IEnumerator FinishGameLoading()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void ClickCheckNot(bool CheckA)
    {
        Debug.Log("Clicked");
        AccebeltityToTouch = !CheckA;
    }

    /// <summary>
    /// 把当前关卡能量条加满到本关上限（StartMoving）。
    /// 供看广告奖励等外部逻辑调用。
    /// </summary>
    public void FillEnergy()
    {
        // StartMoving 是本关开始时的初始能量，即能量上限；兜底用关卡配置值
        int max = StartMoving > 0 ? StartMoving : TotalRound;
        if (max <= 0) max = 130;
        TotalRound = max;

        // 若当前处于"子弹耗尽"的临界状态，恢复可继续放置
        if (TotalRound >= 1)
        {
            AutorizatedSpawn = true;
        }

        // 立即刷新能量条与数值显示
        if (BarFilling != null && ValueActive != null)
        {
            ManagerUI();
        }
        Debug.Log($"[奖励] 能量已加满: {TotalRound}/{max}");
    }
    void ManagerUI()
    {
        BarFilling.fillAmount = (float)((float)TotalRound / (float)StartMoving);
        ValueActive.text = TotalRound + "/" + StartMoving;
    }
    void ManagerMouseClickable()
    {
        if (Input.GetMouseButtonDown(0) && TotalRound > 0)  
        {
            CheckClicked = true;
            StartPooling = true;
        }
        else if(TotalRound <= 0) { CheckClicked = false; }
        if(Input.GetMouseButtonUp(0))
        {
            CheckClicked = false; StartPooling = false;
        }
        if (CheckClicked)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D colliderHit = Physics2D.OverlapPoint(mousePosition);
            if (colliderHit != null)
            {
                if (colliderHit.tag == "Ground")
                {
                    CurrentMode = "Ground";
                    StartPooling = true;
                }
                else if (colliderHit.tag == "MoveStraight")
                {
                    CurrentMode = "MoveStraight";
                    StartPooling = true;
                }
            }
        }
    }
    void StartPoolingObjects()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D colliderHit = Physics2D.OverlapPoint(mousePosition);
        if (colliderHit != null)
        {
            if (colliderHit.tag == CurrentMode)
            {
                if (AutorizatedSpawn == true)
                {
                    Whoosh.Play();
                    if (OldObject != null && Vector2.Distance(OldObject.position, mousePosition) > DistanceBtw)
                    {
                        GameObject newSprite = Instantiate(PooledObjects, mousePosition, PooledObjects.transform.rotation);
                        newSprite.transform.SetParent(GameObject.Find(SpawnLocation).transform);
                        OldObject = newSprite.transform;
                        TotalRound -= 1 * CurrentFactor;
                        CurrentFactor -= 1;
                        AutorizatedSpawn = false;
                    }
                    if (OldObject == null && SpawnAuto == false)
                    {
                        GameObject newSprite = Instantiate(PooledObjects, mousePosition, PooledObjects.transform.rotation);
                        newSprite.transform.SetParent(GameObject.Find(SpawnLocation).transform);
                        OldObject = newSprite.transform;
                        TotalRound -= 1 * CurrentFactor;
                        CurrentFactor -= 1;
                        AutorizatedSpawn = false;
                        SpawnAuto = true;
                    }
                }
            }
            else if (colliderHit.tag == CurrentMode)
            {
                if (AutorizatedSpawn == true)
                {
                    Whoosh.Play();
                    if (OldObject != null && Vector2.Distance(OldObject.position, mousePosition) > DistanceBtw)
                    {
                        GameObject newSprite = Instantiate(PooledObjects, mousePosition, PooledObjects.transform.rotation);
                        newSprite.transform.SetParent(GameObject.Find(SpawnLocation).transform);
                        OldObject = newSprite.transform;
                        TotalRound -= 1 * CurrentFactor;
                        CurrentFactor -= 1;
                        AutorizatedSpawn = false;
                    }
                    if (OldObject == null && SpawnAuto == false)
                    {
                        GameObject newSprite = Instantiate(PooledObjects, mousePosition, PooledObjects.transform.rotation);
                        newSprite.transform.SetParent(GameObject.Find(SpawnLocation).transform);
                        OldObject = newSprite.transform;
                        TotalRound -= 1 * CurrentFactor;
                        CurrentFactor -= 1;
                        AutorizatedSpawn = false;
                        SpawnAuto = true;
                    }
                }
            }
            else if (colliderHit != null && colliderHit.tag != "Ground")
            {
                AutorizatedSpawn = true;
            }
        }
    }
    IEnumerator LoadingInactive()
    {
        yield return new WaitForSeconds(0.4f);
        CloudUI.SetActive(false);
    }

    [Header("Objects Pooling")]
    internal GameObject PooledObjects;

    [Header("Transformer")]
    private Transform OldObject;
}
