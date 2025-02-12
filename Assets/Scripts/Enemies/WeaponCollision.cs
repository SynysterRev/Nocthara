using System;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    private int _damage = -1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            if (_damage == -1)
            {
                _damage = GetComponentInParent<Enemy>().Damage;
            }

            player.TakeDamage(_damage);
        }
    }
}