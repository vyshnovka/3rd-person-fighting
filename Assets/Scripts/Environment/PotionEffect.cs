using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 10f)]
    private float rotationSpeed;

    void FixedUpdate()
    {
        transform.eulerAngles += Vector3.up * rotationSpeed;
    }
}
