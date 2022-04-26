using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform playerCamera;
    [SerializeField]
    private float horizontalSpeed = 2f;
    [SerializeField]
    private float verticalSpeed = 1f;
    [SerializeField]
    private float lookingLimit = 60f;

    private Vector2 rotation = Vector2.zero;

    void Start()
    {
        rotation.x = transform.eulerAngles.x;
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {
        rotation.x += -Input.GetAxis("Mouse Y") * verticalSpeed;
        rotation.y += Input.GetAxis("Mouse X") * horizontalSpeed;
        rotation.x = Mathf.Clamp(rotation.x, -lookingLimit, lookingLimit);

        playerCamera.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, rotation.y);
    }
}
