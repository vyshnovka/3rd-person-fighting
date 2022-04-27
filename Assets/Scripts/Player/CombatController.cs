using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private MovementController movementController;

    void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetComponent<MovementController>().canMove)
        {
            Attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Block();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            movementController.canMove = true;
            movementController.characterAnimator.SetBool("isBlocking", false);
        }
    }

    private void Attack()
    {
        movementController.canMove = false;
        movementController.characterAnimator.SetBool("isAttacking", true);

        StartCoroutine(Utility.TimedEvent(() =>
        {
            GetComponent<MovementController>().canMove = true;
        }, 1.2f));
    }

    private void Block()
    {
        movementController.canMove = false;
        movementController.characterAnimator.SetBool("isBlocking", true);
    }
}
