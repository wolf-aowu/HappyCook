using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {
    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interact(); 不应该被调用");
    }

    public virtual void InteractAlternate(Player player) {
        //Debug.LogError("BaseCounter.InteractAlternate(); 不应该被调用");
    }

    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 获取当前对象上的 KitchenObject，一般为放在柜台上的菜品
    /// </summary>
    /// <returns></returns>
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return (kitchenObject != null);
    }
}
