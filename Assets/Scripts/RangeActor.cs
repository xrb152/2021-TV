using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeActor : MonoBehaviour
{
    [Header("Manager Controller")]
    private SkeletonAnimation animationController;

    [Header("Objects Controller")]
    public GameObject bulletPrefab;

    [Header("Position Manager")]
    public Transform firePoint;

    [Header("float Manager")]
    internal float bulletSpeed = 20f;
    internal float shootingDelay = 1.5f;
    internal float CurrentHealth = 100f;

    [Header("Boolean Manager")]
    private bool isShooting = false;
    private void Awake()
    {
        animationController = GetComponentInChildren<SkeletonAnimation>();
        animationController.AnimationState.SetAnimation(0, "idle", true);
    }
    private void Update()
    {
        if(CurrentHealth <= 0) { Destroy(this.gameObject);}
        if (isShooting) return;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enermy");
        if (enemies.Length > 0 && FindAnyObjectByType<LevelManager>().EnemyEnterd == true)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null && FindAnyObjectByType<LevelManager>().EnemyEnterd == true)
        {
            // Start the shooting delay coroutine
            StartCoroutine(ShootingDelay(closestEnemy));
        }
    }
    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enermy");

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector2 characterPosition = firePoint.position;

        foreach (GameObject enemy in enemies)
        {
            Vector2 enemyPosition = enemy.transform.position;
            float distance = Vector2.Distance(characterPosition, enemyPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }


    private IEnumerator ShootingDelay(GameObject closestEnemy)
    {
        isShooting = true;
        animationController.AnimationState.SetAnimation(0, "attack_range_6_7", true);
        firePoint.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
        // Wait for the shooting delay

        yield return new WaitForSeconds(0.02f);
        if(closestEnemy != null)
        {
            Vector2 direction = closestEnemy.transform.position - firePoint.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Instantiate a bullet at the fire point position with the calculated rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
            bullet.transform.SetParent(FindAnyObjectByType<PoolBullet>().transform);
            // Set the bullet's velocity to move in the calculated direction
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = direction * bulletSpeed;

            // Destroy the bullet after a specified duration
            Destroy(bullet, 4);
        }
        yield return new WaitForSeconds(shootingDelay);

        // Reset shooting flag and play idle animation
        isShooting = false;
        animationController.AnimationState.SetAnimation(0 ,"idle", true);
      //  animator.SetTrigger("Idle");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy"))
        {
            StartCoroutine(LoadingDestroy());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy"))
        {
            StopCoroutine(LoadingDestroy());
        }
    }
    IEnumerator LoadingDestroy()
    {
        yield return new WaitForSeconds(0.3f);
        CurrentHealth -= 20;
        StartCoroutine(LoadingDestroy());
    }
}
