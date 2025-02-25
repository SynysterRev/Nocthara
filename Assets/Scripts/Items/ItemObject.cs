using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField]
    protected Item ItemData;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickUp();
        }
    }

    protected virtual void OnPickUp()
    {
        
    }
}
