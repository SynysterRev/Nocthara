using System;
using Unity.Cinemachine;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] 
    private Transform Spawn;

    [SerializeField] 
    private Vector2 SpawnDirection;
    
    private PlayerController _player;

    public void PlayerEnterTrigger(PlayerController player)
    {
        _player = player;
        if (_player)
        {
            _player.EnablePlayerInput(false);
            _player.EnteringDoor(SpawnDirection, 1.0f);
            LoadingScreenManager.Instance.ShowLoadingScreen();
            LoadingScreenManager.Instance.OnFadeInOver += FadeInOver;
        }
    }

    private void FadeInOver()
    {
        _player.ChangeFacingDirection(SpawnDirection);
        _player.transform.position = Spawn.position;
        _player.StopMoving();
        CinemachineCamera virtualCam = Tools.GetCurrentVirtualCamera();
        if (virtualCam)
        {
            CinemachineConfiner2D confiner2D = virtualCam.GetComponent<CinemachineConfiner2D>();
            confiner2D.BoundingShape2D = GetComponent<BoxCollider2D>() ? GetComponent<BoxCollider2D>() : null;
        }
        LoadingScreenManager.Instance.HideLoadingScreen();
        LoadingScreenManager.Instance.OnFadeInOver -= FadeInOver;
        LoadingScreenManager.Instance.OnFadeOutOver += FadeOutOver;

    }

    private void FadeOutOver()
    {
        _player.EnablePlayerInput(true);
        _player.ResetMoveSpeed();
        LoadingScreenManager.Instance.OnFadeOutOver -= FadeInOver;
    }
}
