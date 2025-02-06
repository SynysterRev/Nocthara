using System.Collections.Generic;
using UnityEngine;

public class ConditionListAttribute : PropertyAttribute { }
public class InteractObject : MonoBehaviour, IInteractable
{
    [SerializeReference, SerializeReferenceDropdown]
    private List<InteractionCondition> Conditions = new List<InteractionCondition>();

    public void TryInteract(GameObject player, GameObject target)
    {
        if (CanInteract(player, target))
        {
            Interact(player, target);
        }
    }
    protected virtual void Interact(GameObject player, GameObject target)
    {

    }

    protected bool CanInteract(GameObject player, GameObject target)
    {
        foreach (InteractionCondition condition in Conditions)
        {
            if (!condition.IsMet(player, target)) return false;
        }
        return true;
    }
}
