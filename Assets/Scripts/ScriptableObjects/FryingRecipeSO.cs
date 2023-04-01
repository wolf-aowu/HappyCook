using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject {
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int fryingTimerMax;
}
