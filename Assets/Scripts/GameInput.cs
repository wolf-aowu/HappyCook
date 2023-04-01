using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause,
    }

    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;

        // 由于 playerInputActions 对象监听了事件，所以不会被销毁，这些事件被再次触发时由于 GameInput 已经随着场景销毁了，所以会丢失引用，导致报错。
        // 所以需要退订事件，Unity 为 MonoBehaviour 销毁时提供了 OnDestroy 函数。在该函数中退订事件，也可以在该函数中调用 Dispose 释放 playerInputActions。
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        // 这里的 Player 是指 Action Map 的名字
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        // 如果交互事件为空会报错，所以此处要先判断 OnInteractAction 是否为空
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        // 向量归一化，也就是使向量的模恒等于 1
        // 避免移动距离一会等于 1，一会大于 1
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        // 键位修改需要先禁用 InputMap
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => {
            // 键位修改完成后会执行
            //Debug.Log(callback.action.bindings[1].path);
            //Debug.Log(callback.action.bindings[1].overridePath);
            callback.Dispose();
            // 重新启用 InputMap
            playerInputActions.Player.Enable();
            onActionRebound();
            // 保存键位修改
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}
