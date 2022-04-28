using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
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

    [NonSerialized]
    public bool canMove = true;

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

            if (Input.GetKeyDown(KeyCode.Space) && canMove)
            {
                characterAnimator.SetBool("isJumping", true);
                direction.y = jumpSpeed;
            }
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
            availableItem = null;

            canMove = false;
            characterAnimator.SetBool("isPicking", true);

            StartCoroutine(Utility.TimedEvent(() =>
            {
                itemToPick.GetComponent<WeaponController>().ActivateUI();
                itemToPick.GetComponent<Collider>().enabled = false;

                itemToPick.transform.parent = hand.transform;
                itemToPick.transform.position = hand.transform.position;
                itemToPick.transform.rotation = hand.transform.rotation;

                inventory.FirstEquip(itemToPick);
            }, 1f));

            StartCoroutine(Utility.TimedEvent(() =>
            {
                canMove = true;
            }, 2f));
        }
    }

    private void Drop()
    {
        GetComponent<CombatController>().damage = GetComponent<CombatController>().defaultDamage;
        GetComponent<CombatController>().range = GetComponent<CombatController>().defaultRange;
        characterAnimator.runtimeAnimatorController = defaultAnimator;

        inventory.RemoveFromInventory();
    }

    private void AnimateMovement()
    {
        float directionX = Vector3.Dot(direction.normalized, transform.right);
        float directionZ = Vector3.Dot(direction.normalized, transform.forward);

        characterAnimator.SetFloat("directionX", directionX, 0.1f, Time.deltaTime);
        characterAnimator.SetFloat("directionZ", directionZ, 0.1f, Time.deltaTime);
    }
}
