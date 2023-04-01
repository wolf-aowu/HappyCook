using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        // 取消父级下的自己
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("Counter already has a KitchenObject!");
        }

        // 将自己传给要去的父级
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }

    public void DestroySelf() {
        // 在销毁自己之前需要先告诉父级，使父级清空自己的信息
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if (this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else {
            plateKitchenObject = null;
            return false;
        }
    }

    /// <summary>
    /// 在指定节点生成 KitchenObject
    /// </summary>
    /// <param name="kitchenObjectSO">需要生成的 KitchenObjectSO</param>
    /// <param name="kitchenObjectParent">生成的 KitchenObject 的父节点</param>
    /// <returns>生成的 KitchenObject</returns>
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        kitchenObjectTransform.localPosition = Vector3.zero;
        return kitchenObject;
    }
}
