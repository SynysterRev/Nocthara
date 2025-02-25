using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] 
    private House ConnectedHouse;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (ConnectedHouse)
            {
                ConnectedHouse.PlayerEnterTrigger(player);
            }
        }
    }
}
