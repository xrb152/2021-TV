using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enermy"))
        {
            FindAnyObjectByType<LevelManager>().EnemyItsIN = true;
            FindAnyObjectByType<LevelManager>().Finish(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enermy"))
        {
            FindAnyObjectByType<LevelManager>().EnemyItsIN = true;
            FindAnyObjectByType<LevelManager>().Finish(false);
        }
    }
}
