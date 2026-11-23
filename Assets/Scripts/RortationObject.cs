using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RortationObject : MonoBehaviour
{
    public Vector3 Rotation;
    void Update()
    {
        transform.Rotate(Rotation);
    }
}
