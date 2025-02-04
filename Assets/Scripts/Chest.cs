using System;
using System.Collections.Generic;
using UnityEngine;


public class Chest : InteractObject
{
    public SpriteAnimator SpriteAnimator;
    void Start()
    {
        SpriteAnimator = GetComponent<SpriteAnimator>();
    }
    
    protected override void Interact(GameObject player, GameObject target)
    {
        SpriteAnimator.Play("opening", false);
    }

    public void Toast()
    {
        Debug.Log("Toast");
    }
}
