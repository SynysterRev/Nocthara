using System;
using Unity.Cinemachine;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] 
    private Transform Spawn;

    [SerializeField] 
    private Vector2 SpawnDirection;

    public void PlayerEnterTrigger(PlayerController player)
    {
        if (player)
        {
            player.ChangeFacingDirection(SpawnDirection);
            player.transform.position = Spawn.position;
            CinemachineCamera virtualCam = Tools.GetCurrentVirtualCamera();
            if (virtualCam)
            {
                CinemachineConfiner2D confiner2D = virtualCam.GetComponent<CinemachineConfiner2D>();
                confiner2D.BoundingShape2D = GetComponent<BoxCollider2D>() ? GetComponent<BoxCollider2D>() : null;
            }
        }
    }
}
