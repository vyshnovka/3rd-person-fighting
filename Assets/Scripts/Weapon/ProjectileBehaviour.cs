using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position += transform.forward * 0.2f;
        transform.position -= transform.up * 0.01f;
    }
}
