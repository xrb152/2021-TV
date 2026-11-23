using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PoolEnermy : MonoBehaviour
{
    [Header("Manager Controller")]
    public CharManager ManagerPosition;

    [Header("Prefabes")]
    public GameObject[] listEnemys;

    [Header("Internal integer")]
    internal int Currentmode = 0;
    internal int TotalEnemy = 13;

    [Header("Boolean Manager")]
    internal bool EnemyUp = false;
    internal bool EnemyLeft = false;
    internal bool EnemyRight = false;

    private void Start()
    {
        Maps ObjectActive = FindAnyObjectByType<Maps>();
        if (ObjectActive.gameObject.activeSelf) { ObjectActive.WarningTop.SetActive(true); }
        StartCoroutine(LoadingTimer());
    }
    IEnumerator LoadingTimer()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawningEnemy());
    }
    IEnumerator SpawningEnemy()
    {
        if(TotalEnemy > 0)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
            if (EnemyUp && EnemyLeft == false && EnemyRight == false)
            {
                (Instantiate(listEnemys[Currentmode], ManagerPosition.EnemyPosition[Random.Range(0, ManagerPosition.EnemyPosition.Length)].position, listEnemys[Currentmode].transform.rotation) as GameObject).transform.SetParent(transform);
                TotalEnemy -= 1;
            }else if (EnemyLeft && EnemyUp)
            {
                int RandomizeNumber = Random.Range(0,3);
                if(RandomizeNumber == 1)
                {
                    (Instantiate(listEnemys[Currentmode], ManagerPosition.EnemyPosition[Random.Range(0, ManagerPosition.EnemyPosition.Length)].position, listEnemys[Currentmode].transform.rotation) as GameObject).transform.SetParent(transform);
                    TotalEnemy -= 1;
                }
                if (RandomizeNumber == 2)
                {
                    (Instantiate(listEnemys[Currentmode], ManagerPosition.EnemyPositionLeft[Random.Range(0, ManagerPosition.EnemyPositionLeft.Length)].position, listEnemys[Currentmode].transform.rotation) as GameObject).transform.SetParent(transform);
                    TotalEnemy -= 1;
                }
            }else if (EnemyRight && EnemyUp)
            {
                int RandomizeNumber = Random.Range(0, 3);
                if (RandomizeNumber == 1)
                {
                    (Instantiate(listEnemys[Currentmode], ManagerPosition.EnemyPosition[Random.Range(0, ManagerPosition.EnemyPosition.Length)].position, listEnemys[Currentmode].transform.rotation) as GameObject).transform.SetParent(transform);
                    TotalEnemy -= 1;
                }
                if (RandomizeNumber == 2)
                {
                    (Instantiate(listEnemys[Currentmode], ManagerPosition.EnemyPositionRight[Random.Range(0, ManagerPosition.EnemyPositionRight.Length)].position, listEnemys[Currentmode].transform.rotation) as GameObject).transform.SetParent(transform);
                    TotalEnemy -= 1;
                }
            }
            else if(EnemyRight && EnemyLeft && EnemyUp)
            {
                int randomizePosition = Random.Range(0, 3);
                if(randomizePosition == 0)
                {
                    (Instantiate(listEnemys[Currentmode], ManagerPosition.EnemyPositionRight[Random.Range(0, ManagerPosition.EnemyPositionRight.Length)].position, listEnemys[Currentmode].transform.rotation) as GameObject).transform.SetParent(transform);
                    TotalEnemy -= 1;
                }
                if(randomizePosition == 1)
                {
                    (Instantiate(listEnemys[Currentmode], ManagerPosition.EnemyPosition[Random.Range(0, ManagerPosition.EnemyPosition.Length)].position, listEnemys[Currentmode].transform.rotation) as GameObject).transform.SetParent(transform);
                    TotalEnemy -= 1;
                }
                if(randomizePosition == 2)
                {
                    (Instantiate(listEnemys[Currentmode], ManagerPosition.EnemyPositionLeft[Random.Range(0, ManagerPosition.EnemyPositionLeft.Length)].position, listEnemys[Currentmode].transform.rotation) as GameObject).transform.SetParent(transform);
                    TotalEnemy -= 1;
                }
            }
            StartCoroutine(SpawningEnemy());
        }
        else
        {
            Maps ObjectActive = FindAnyObjectByType<Maps>();
            if (ObjectActive.gameObject.activeSelf) { ObjectActive.WarningTop.SetActive(false); }
        }
    }
}
