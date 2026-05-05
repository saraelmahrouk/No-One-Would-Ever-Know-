using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTrigger : MonoBehaviour
{
    public float damagePerSecond = 1f;
    private Player player;
    private bool playerInside = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the trigger: " + other.name); // This tells you IF a collision happened
        if (other.CompareTag("Player"))
        {
            Debug.Log("The Player entered!"); // This tells you if the TAG is correct
            player = other.GetComponent<Player>();

            // Only set playerInside to true if we actually found the Player script
            if (player != null)
            {
                playerInside = true;
            }
            else
            {
                Debug.LogError("Object tagged 'Player' entered, but it is missing the Player.cs script!");
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }

    void Update()
    {
        if (playerInside && player != null)
        {
            player.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
