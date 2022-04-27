using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    [NonSerialized]
    public Animator characterAnimator;
    public AnimatorOverrideController defaultAnimator;
    private InventoryManager inventory;

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
        characterAnimator = GetComponent<Animator>();

        inventory = GetComponent<InventoryManager>();
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
                characterAnimator.SetBool("isJumping", true);
                direction.y = jumpSpeed;
            }
        }

        if (Input.GetMouseButtonDown(0) && canMove)
        {
            Attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Block();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            canMove = true;
            characterAnimator.SetBool("isBlocking", false);
        }


        if (Input.GetKeyDown(KeyCode.E) && availableItem && canMove)
        {
            Pick(availableItem);
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

    private void Pick(GameObject itemToPick)
    {
        if (inventory.AddToInventory(itemToPick))
        {
            canMove = false;
            characterAnimator.SetBool("isPicking", true);

            StartCoroutine(Utility.TimedEvent(() =>
            {
                itemToPick.transform.parent = hand.transform;
                itemToPick.transform.localPosition = hand.transform.localPosition;
                itemToPick.transform.localRotation = hand.transform.localRotation;

                inventory.FirstEquip(itemToPick);
            }, 1.5f));

            StartCoroutine(Utility.TimedEvent(() =>
            {
                canMove = true;
            }, 1.7f));
        }
    }

    private void Drop()
    {
        characterAnimator.runtimeAnimatorController = defaultAnimator;
        inventory.RemoveFromInventory();
    }

    private void Attack()
    {
        canMove = false;
        characterAnimator.SetBool("isAttacking", true);

        StartCoroutine(Utility.TimedEvent(() =>
        {
            canMove = true;
        }, 1.2f));
    }

    private void Block()
    {
        canMove = false;
        characterAnimator.SetBool("isBlocking", true);
    } 

    private void AnimateMovement()
    {
        float directionX = Vector3.Dot(direction.normalized, transform.right);
        float directionZ = Vector3.Dot(direction.normalized, transform.forward);

        characterAnimator.SetFloat("directionX", directionX, 0.1f, Time.deltaTime);
        characterAnimator.SetFloat("directionZ", directionZ, 0.1f, Time.deltaTime);
    }
}
