using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    [Header("Transfomer Controller")]
    public Transform[] ListPosition;
    public Transform[] EnemyPosition;
    public Transform[] EnemyPositionLeft;
    public Transform[] EnemyPositionRight;

    [Header("Floating manager")]
    public float StopDistance = 1f;
}
