using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void TryInteract(GameObject player, GameObject target);
}
