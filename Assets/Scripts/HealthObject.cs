using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObject : MonoBehaviour
{
    [Header("Floating Controller")]
    internal float HealthCurrentObject = 100;

    [Header("Outside Factor")]
    public float MovingFactor = 20f;
    private void Start()
    {
        transform.Find("Spawn_unit").gameObject.GetComponent<ParticleSystem>().Play();
    }
    private void Update()
    {
        if(HealthCurrentObject <= 0) { Destroy(this.gameObject); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy"))
        {
            HealthCurrentObject -= MovingFactor;
            StartCoroutine(HealthMovingBack());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy"))
        {
            HealthCurrentObject -= MovingFactor;
            StopCoroutine(HealthMovingBack());
        }
    }
    IEnumerator HealthMovingBack()
    {
        yield return new WaitForSeconds(1f);
        HealthCurrentObject -= MovingFactor;
        StartCoroutine(HealthMovingBack());
    }
}
