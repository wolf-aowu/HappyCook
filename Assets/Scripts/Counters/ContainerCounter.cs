using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // 柜台上没有物品
            if (!player.HasKitchenObject()) {
                // 玩家没有携带物品，生成一个物品交给玩家
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
