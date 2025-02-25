using UnityEngine;

public class MoneyObject : ItemObject
{
    protected override void OnPickUp()
    {
        PlayerManager.Instance.AddMoney(ItemData.ItemPrice);
        Destroy(gameObject);
    }
}
