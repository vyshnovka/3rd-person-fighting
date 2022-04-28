using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewController : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    [Range(0, 360)]
    private float angle;

    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private LayerMask environmentLayer;

    private void Start()
    {
        StartCoroutine(ViewRoutine());
    }

    private IEnumerator ViewRoutine()
    {
        while (true)
        {
            LookForEnemy();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void LookForEnemy()
    {
        Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        if (detectedEnemies.Length > 0)
        {
            GameObject enemy = ClothestEnemy(detectedEnemies);

            Vector3 direction = (enemy.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, direction) < angle / 2)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                Vector3 raycastPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

                //Debug.DrawRay(raycastPosition, enemy.transform.position, Color.blue);

                if (!Physics.Raycast(raycastPosition, direction, distance, environmentLayer))
                {
                    GetComponent<CombatController>().enemy = enemy;
                }
                else
                {
                    GetComponent<CombatController>().enemy = null;
                }
            }
            else
            {
                GetComponent<CombatController>().enemy = null;
            }
        }
        else if (GetComponent<CombatController>().enemy)
        {
            GetComponent<CombatController>().enemy = null;
        }
    }

    private GameObject ClothestEnemy(Collider[] detectedEnemies)
    {
        if (detectedEnemies.Length > 1)
        {
            var minimum = Vector3.Distance(transform.position, detectedEnemies[0].transform.position);
            int index = 0;

            for (int i = 1; i < detectedEnemies.Length; i++)
            {
                if (Vector3.Distance(transform.position, detectedEnemies[i].transform.position) < minimum)
                {
                    index = i;
                }
            }

            return detectedEnemies[index].gameObject;
        }

        return detectedEnemies[0].gameObject;
    }
}
