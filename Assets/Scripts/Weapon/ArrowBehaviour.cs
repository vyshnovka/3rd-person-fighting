using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position += transform.forward * 0.1f;
        transform.position -= transform.up * 0.01f;
    }
}
