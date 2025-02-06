using System;
using System.Collections.Generic;
using UnityEngine;


public class Chest : InteractObject
{
    public SpriteAnimator SpriteAnimator;
    private bool _isOpen;
    void Start()
    {
        SpriteAnimator = GetComponent<SpriteAnimator>();
    }
    
    protected override void Interact(GameObject player, GameObject target)
    {
        if (!_isOpen)
        {
            _isOpen = true;
            SpriteAnimator.Play("opening", false);
        }
    }
}
