using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Spine.Unity;
using Spine;
using static UnityEngine.GraphicsBuffer;

public class MeeleActor : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform; // Reference to the object's sprite transform
    [SerializeField] private float flipThreshold = 0.05f;

    [Header("Controller Manager")]
    private AIDestinationSetter destinationSetter;
    private CharManager managerPosition;
    private AIPath pathFinding;
    private SkeletonAnimation animations;

    [Header("Target Controller")]
    private GameObject[] listActiveEnemy;

    [Header("Internal Integer")]
    internal int currentTarget = 0;

    [Header("Boolean Manager")]
    internal bool itsIdle = true;
    internal bool itsAttack = true;
    internal bool itsRun = true;
    internal bool followRandomly = false;
    internal bool LeftChecker = true;
    internal bool RightChecker = true;

    private GameObject followingObject;
    private GameObject hittingObject;

    private void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        managerPosition = FindAnyObjectByType<CharManager>();
        animations = transform.GetComponentInChildren<SkeletonAnimation>();
        pathFinding = GetComponent<AIPath>();
        spriteTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        pathFinding.endReachedDistance = managerPosition.StopDistance;
        listActiveEnemy = GameObject.FindGameObjectsWithTag("Enermy");
    }

    private void Update()
    {
        if (listActiveEnemy.Length > 0)
        {
            LevelManager Manager = FindAnyObjectByType<LevelManager>();
            if (followingObject == null)
            {
                currentTarget = Random.Range(0, listActiveEnemy.Length);
                followingObject = listActiveEnemy[currentTarget];
                followRandomly = false; // Added to prevent random following immediately
            }
            else if (followingObject != null && followRandomly == false)
            {
                Vector3 direction = followingObject.transform.position - transform.position;

                // Flip the character's sprite based on the direction
                if (direction.x > 0 && LeftChecker)
                {
                    spriteTransform.localScale = new Vector3(-1f, 1f, 1f); LeftChecker = false;
                }
                else if (direction.x < 0 && RightChecker)
                {
                    spriteTransform.localScale = new Vector3(1f, 1f, 1f);RightChecker = false;
                }
                /////////////////
                if (followingObject != null && followingObject.GetComponent<Enemy>() != null && followingObject.GetComponent<Enemy>().CurrentAttack < 3)
                {
                    if (Manager.EnemyEnterd)
                    {
                        animations.AnimationState.SetAnimation(0, "run", true);
                        followingObject.GetComponent<Enemy>().CurrentAttack += 1;

                        destinationSetter.target = followingObject.transform;
                        destinationSetter.enabled = true;

                        itsIdle = true;
                        itsRun = false;
                    }
                }
                else
                {
                    followingObject = null;
                }

                bool allObjectsMeetCondition = true;
                foreach (GameObject obj in listActiveEnemy)
                {
                    if (obj == null || obj.GetComponent<Enemy>() == null || obj.GetComponent<Enemy>().CurrentAttack < 3)
                    {
                        allObjectsMeetCondition = false;
                        break;
                    }
                }

                if (allObjectsMeetCondition)
                {
                    // All objects meet the condition
                    followRandomly = true;
                }
                else if (followingObject != null && followingObject.GetComponent<Enemy>() != null && followingObject.GetComponent<Enemy>().CurrentAttack > 3)
                {
                    // At least one object does not meet the condition
                    followingObject = null;
                    followRandomly = false;
                }
            }
            else if (followRandomly == true)
            {
                if (followingObject != null)
                {
                    destinationSetter.target = followingObject.transform;
                    destinationSetter.enabled = true;
                }
            }
        }
        else if (itsIdle)
        {
            animations.AnimationState.SetAnimation(0, "idle", true);
            itsAttack = true;
            itsRun = true;
            itsIdle = false;
        }

        listActiveEnemy = GameObject.FindGameObjectsWithTag("Enermy"); // Moved outside the else if (itsIdle == false) block
    }

    private void OnDestroy()
    {
        if (followingObject != null && followingObject.GetComponent<Enemy>() != null && followingObject.GetComponent<Enemy>().CurrentAttack > 0)
        {
            followingObject.GetComponent<Enemy>().CurrentAttack -= 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy"))
        {
            GetComponent<AudioSource>().Play();
            if (itsAttack)
            {
                hittingObject = collision.gameObject;
                StartCoroutine(HittingObjects(hittingObject));
                animations.AnimationState.SetAnimation(0, "attack_melee_1_4", true);
                itsRun = true;
                itsIdle = true;
                itsAttack = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy"))
        {
            itsAttack = true;
            GetComponent<AudioSource>().Play();
            StopCoroutine(HittingObjects(hittingObject));
            if (itsAttack)
            {
                animations.AnimationState.SetAnimation(0, "attack_melee_2_3_5_6", true);
                itsRun = true;
                itsIdle = true;
                itsAttack = false;
            }
        }
    }

    IEnumerator HittingObjects(GameObject obj)
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1));
        if (obj != null && obj.GetComponent<Enemy>() != null)
        {
            obj.GetComponent<Enemy>().HealthValue -= 25f;
        }
        StartCoroutine(HittingObjects(hittingObject));
    }
}
