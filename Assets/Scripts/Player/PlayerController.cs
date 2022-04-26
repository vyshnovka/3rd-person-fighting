using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator chararterAnimator;
    //private InventoryManager inventory;

    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float jumpSpeed = 5f;
    [SerializeField]
    private float gravity = 10f;

    private Vector3 direction = Vector3.zero;

    private float inputX = 0;
    private float inputZ = 0;

    private bool canMove = true;

    [SerializeField]
    private GameObject hand;
    [NonSerialized]
    public GameObject availableItem = null;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        chararterAnimator = GetComponent<Animator>();

        //inventory = GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            inputX = Input.GetAxis("Vertical");
            inputZ = Input.GetAxis("Horizontal");
            direction = (forward * inputX * movementSpeed) + (right * inputZ * movementSpeed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                chararterAnimator.SetBool("isJumping", true);
                direction.y = jumpSpeed;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("attack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            chararterAnimator.SetBool("isBlocking", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            chararterAnimator.SetBool("isBlocking", false);
        }

        if (Input.GetKeyDown(KeyCode.E) && availableItem)
        {
            StartCoroutine(Pick(availableItem));
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }

        direction.y -= gravity * Time.deltaTime;

        if (canMove)
        {
            characterController.Move(direction * Time.deltaTime);
            AnimateMovement();
        }
    }

    private IEnumerator Pick(GameObject itemToPick)
    {
        canMove = false;
        chararterAnimator.SetBool("isPicking", true);

        yield return new WaitForSeconds(1.5f);

        itemToPick.transform.parent = hand.transform;
        itemToPick.transform.localPosition = hand.transform.localPosition;
        itemToPick.transform.localRotation = hand.transform.localRotation;

        canMove = true;
    }

    private void Drop()
    {
        Debug.Log("dropped");
    }

    private void AnimateMovement()
    {
        float directionX = Vector3.Dot(direction.normalized, transform.right);
        float directionZ = Vector3.Dot(direction.normalized, transform.forward);

        chararterAnimator.SetFloat("directionX", directionX, 0.1f, Time.deltaTime);
        chararterAnimator.SetFloat("directionZ", directionZ, 0.1f, Time.deltaTime);
    }
}
