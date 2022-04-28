using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewController : MonoBehaviour
{
    private Animator enemyAnimator;

    [SerializeField]
    private float radius;
    [SerializeField]
    [Range(0, 360)]
    private float angle;

    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask environmentLayer;

    [NonSerialized]
    public bool isSeeing;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();

        StartCoroutine(ViewRoutine());
    }

    private IEnumerator ViewRoutine()
    {
        while (true)
        {
            LookForPlayer();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void LookForPlayer()
    {
        Collider[] detectedPlayers = Physics.OverlapSphere(transform.position, radius, playerLayer);

        if (detectedPlayers.Length > 0)
        {
            GameObject player = detectedPlayers[0].gameObject;

            Vector3 direction = (player.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, direction) < angle / 2)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

                //Debug.DrawRay(raycastPosition, player.transform.position, Color.red);

                if (!Physics.Raycast(raycastPosition, direction, distance, environmentLayer))
                {
                    enemyAnimator.SetBool("isSeeing", true);
                    isSeeing = true;
                } 
                else
                {
                    enemyAnimator.SetBool("isSeeing", false);
                    isSeeing = false;
                }
            }
            else
            {
                enemyAnimator.SetBool("isSeeing", false);
                isSeeing = false;
            }
        }
        else if (isSeeing)
        {
            enemyAnimator.SetBool("isSeeing", false);
            isSeeing = false;
        }
    }
}
