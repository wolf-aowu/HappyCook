using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData() {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // 柜台上没有东西
            if (player.HasKitchenObject()) {
                // 玩家手上有东西
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    // 玩家带了可切割的东西，玩家可以把它放下，修改进度条
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    cuttingProgress = 0;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
            else {
                // 玩家没有携带东西
            }
        }
        else {
            // 柜台上有东西
            if (player.HasKitchenObject()) {
                // 玩家携带某些东西
                if (player.GetKitchenObject() is PlateKitchenObject) {
                    // 玩家拿的是盘子
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                        // 将柜台上的东西尝试放进玩家手中的盘子
                        if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                            // 东西可以被放入盘子，清空柜台
                            GetKitchenObject().DestroySelf();
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

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            // 柜台上有东西，东西可以被切
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            // 测试有几个对象注册了 OnAnyCut 事件
            //Debug.Log(OnAnyCut.GetInvocationList());

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                // 切的进度完成
                // 变成切片流程：销毁完整的东西，生成切片
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        }
        else {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
