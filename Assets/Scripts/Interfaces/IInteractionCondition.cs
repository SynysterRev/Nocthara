using UnityEngine;

[System.Serializable]
public abstract class InteractionCondition
{
    public abstract bool IsMet(GameObject player, GameObject target);
}

[System.Serializable]
public class DistanceCondition : InteractionCondition
{
    [SerializeField]
    private float Distance;
    public override bool IsMet(GameObject player, GameObject target)
    {
        return Vector3.Distance(player.transform.position, target.transform.position) < Distance;
    }
}
//
// [System.Serializable]
// public class AngleCondition : InteractionCondition
// {
//     [SerializeField]
//     private float MaxAngle;
//
//     [SerializeField] 
//     private Vector2 TargetDirection;
//     
//     public override bool IsMet(GameObject player, GameObject target)
//     {
//         PlayerController playerController = player.GetComponent<PlayerController>();
//         float angle = Vector3.Dot(playerController.LastFacedDirection, TargetDirection);
//         return angle / 2.0f < MaxAngle;
//     }
// }

[System.Serializable]
public class FacingCondition : InteractionCondition
{
    [SerializeField] 
    private Vector2 TargetDirection;
    
    public override bool IsMet(GameObject player, GameObject target)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        float dot = Vector3.Dot(playerController.LastFacedDirection, TargetDirection);
        return dot > 0.0f;
    }
}
