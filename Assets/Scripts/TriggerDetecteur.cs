using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetecteur : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy"))
        {
            LevelManager Manager = FindAnyObjectByType<LevelManager>();Manager.EnemyEnterd = true;
        }
    }
}
