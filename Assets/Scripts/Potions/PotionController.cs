using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionController : MonoBehaviour
{
    [SerializeField]
    private int value = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<CombatController>();

            if (player.playerStats.health != player.healthBar.maxValue)
            {
                player.playerStats.health += value;
                player.healthBar.value += value;

                Destroy(gameObject);
            }
        }
    }
}
