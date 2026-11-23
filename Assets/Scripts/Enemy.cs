using Pathfinding;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Controller Manager")]
    private AIDestinationSetter destinationSetter;
    private CharManager managerPosition;
    private AIPath pathFinding;
    private SkeletonAnimation animationController;

    [Header("GameObjects")]
    public GameObject EffectEnergy;

    [Header("Boolean Manager")]
    internal bool moveHome = true;
    internal bool idle = true;
    internal bool walk = true;
    internal bool dead = true;
    internal bool attack = true;
    internal bool foundClosestObject = false; // Add a flag to track if the closest object is found

    [Header("Strings manager")]
    internal string[] tagsToSearch = {"Barel" ,"Player" };

    [Header("Floating manager")]
    internal float minDistance = 3f;
    internal float HealthValue = 70f;

    [Header("Integer Controller")]
    internal int CurrentAttack = 0;

    private void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        managerPosition = FindAnyObjectByType<CharManager>();
        animationController = GetComponentInChildren<SkeletonAnimation>();
        pathFinding = GetComponent<AIPath>();
    }

    private void Start()
    {
        animationController.AnimationState.SetAnimation(0, "walk", true);
        pathFinding.endReachedDistance = managerPosition.StopDistance;
        PositionFollow = managerPosition.ListPosition[Random.Range(0, managerPosition.ListPosition.Length)].gameObject;
    }

    private void Update()
    {
        ManagerLocation();

        if (moveHome)
        {
            if (walk)
            {
                animationController.AnimationState.SetAnimation(0, "walk", true);
                dead = true;
                attack = true;
                idle = true;
                walk = false;
            }

            destinationSetter.target = PositionFollow.transform;
            destinationSetter.enabled = true;
        }
        else
        {
            if (TargetClose != null)
            {
                destinationSetter.target = TargetClose.transform;            
            }
        }

        if (HealthValue <= 0)
        {
            LevelManager Manager = FindAnyObjectByType<LevelManager>();
            if (Manager.TotalRound < 130)
            {
                (Instantiate(EffectEnergy, transform.position, EffectEnergy.transform.rotation) as GameObject).transform.SetParent(FindAnyObjectByType<PoolEffect>().transform);
                Manager.TotalRound += 1;
            }
            Destroy(gameObject);
        }
    }

    void ManagerLocation()
    {
        closestObject = null;
        float closestDistance = minDistance;

        foreach (string tag in tagsToSearch)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject taggedObject in taggedObjects)
            {
                float distance = Vector2.Distance(transform.position, taggedObject.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = taggedObject;
                }
            }
        }

        if (closestObject != null)
        {
            TargetClose = closestObject;
            Debug.DrawLine(transform.position, closestObject.transform.position, Color.red);
            foundClosestObject = true; // Set the flag to true when the closest object is found
            moveHome = false;
        }
        else
        {
            foundClosestObject = false;
            moveHome = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (attack)
            {
                LevelManager Manager = FindAnyObjectByType<LevelManager>();
                if (Manager.TotalRound < 130)
                {
                    (Instantiate(EffectEnergy, transform.position, EffectEnergy.transform.rotation) as GameObject).transform.SetParent(FindAnyObjectByType<PoolEffect>().transform);
                    Manager.TotalRound += 1;
                }
                HealthValue -= 10;
                animationController.AnimationState.SetAnimation(0, "attack", true);
                walk = true;
                idle = true;
                dead = true;
                attack = false;
            }
        }
        if (collision.CompareTag("Barel"))
        {
            if (attack)
            {
                HealthValue -= 10;
                animationController.AnimationState.SetAnimation(0, "attack", true);
                walk = true;
                idle = true;
                dead = true;
                attack = false;
            }
        }
        if (collision.CompareTag("Bullet"))
        {
            GetComponent<AudioSource>().Play();
            HealthValue -= 5f;
            LevelManager Manager = FindAnyObjectByType<LevelManager>();
            if(Manager.TotalRound < 130)
            {
                (Instantiate(EffectEnergy, transform.position, EffectEnergy.transform.rotation) as GameObject).transform.SetParent(FindAnyObjectByType<PoolEffect>().transform);
                Manager.TotalRound += 1;
            }
            GetComponentInChildren<Animator>().Play("AnimationMaterial");
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CurrentAttack -= 1;
            if (attack)
            {
                HealthValue -= 10;
                animationController.AnimationState.SetAnimation(0, "attack", true);
                walk = true;
                idle = true;
                dead = true;
                attack = false;
            }
        }
        if (collision.CompareTag("Barel"))
        {
            if (attack)
            {
                HealthValue -= 10;
                animationController.AnimationState.SetAnimation(0, "attack", true);
                walk = true;
                idle = true;
                dead = true;
                attack = false;
            }
        }
    }

    [Header("Position Manager")]
    private GameObject PositionFollow;
    private GameObject TargetClose;
    private GameObject closestObject;
}
