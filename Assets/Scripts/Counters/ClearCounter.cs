using UnityEngine;

// 继承的类只能有一个，但是继承的接口可以有多个
public class ClearCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // 柜台上没有东西
            if (player.HasKitchenObject()) {
                // 玩家携带某些东西，将玩家携带东西放到柜台上
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else {
                // 玩家没有携带东西
            }
        }
        else {
            // 柜台上有东西
            if (player.HasKitchenObject()) {
                // 玩家携带某些东西
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // 玩家拿的是盘子
                    // 将柜台上的东西尝试放进玩家手中的盘子
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        // 东西可以被放入盘子，清空柜台
                        GetKitchenObject().DestroySelf();
                    }
                }
                else {
                    // 玩家拿的不是盘子
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        // 柜台上的是盘子
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            // 东西可以被放入盘子，清空玩家手上的东西
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else {
                // 玩家没有携带某些东西，将柜台上的东西交给玩家
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
