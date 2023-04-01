using UnityEngine;

public class PlateIconUI : MonoBehaviour {
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTamplate;

    private void Awake() {
        iconTamplate.gameObject.SetActive(false);
    }

    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        // 销毁除了模板 icon 以外的 icon
        foreach (Transform child in transform) {
            if (child != iconTamplate) {
                Destroy(child.gameObject);
            }
        }
        // 根据盘子里的 kitchenObjectSOList 生成 icon
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTamplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
