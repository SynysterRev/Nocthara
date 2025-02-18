using System;
using System.Collections.Generic;
using UnityEngine;


public class Chest : InteractObject
{
    [SerializeField] private Item Item;
    private SpriteAnimator _spriteAnimator;
    private bool _isOpen;

    void Start()
    {
        _spriteAnimator = GetComponent<SpriteAnimator>();
    }

    protected override void Interact(GameObject player, GameObject target)
    {
        if (!_isOpen)
        {
            _isOpen = true;
            _spriteAnimator.Play("opening", false);
            _spriteAnimator.onAnimationFinished += OnAnimationFinished;
        }
    }

    private void OnAnimationFinished()
    {
        if (Item)
        {
            //play player animation get object
            DialogueManager.Instance.OnTypeWriterEnds += OnTypeWriterEnds;
            DialogueManager.Instance.StartOneDialogue($"GG tu as gagné {Item.ItemName}. {Item.ItemDescription}.");
        }
    }

    private void OnTypeWriterEnds()
    {
        if (Item.ItemType == ItemType.Money)
        {
            //reset player anim
            PlayerManager.Instance.AddMoney(Item.ItemPrice);
        }
    }
}