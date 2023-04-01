using UnityEngine;

/// <summary>
/// 存放相关信息，如对象名，贴图，预制体，一般为菜品相关信息
/// </summary>
[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject {
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
