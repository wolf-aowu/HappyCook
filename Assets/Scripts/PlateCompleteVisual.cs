using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {
    [Serializable]
    public struct KitchenObjectSO_GameOject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameOject> kitchenObjectSOGameOjectList;


    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectSO_GameOject kitchenObjectSOGameOject in kitchenObjectSOGameOjectList) {
            kitchenObjectSOGameOject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        foreach (KitchenObjectSO_GameOject kitchenObjectSOGameOject in kitchenObjectSOGameOjectList) {
            if (kitchenObjectSOGameOject.kitchenObjectSO == e.kitchenObjectSO) {
                kitchenObjectSOGameOject.gameObject.SetActive(true);
            }
        }
    }
}
