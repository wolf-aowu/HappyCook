using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;

    private void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
                // 以前的教程中说尽量不要使用 Camera.main.transform 是因为 Unity 没有对 Camera.main 缓存，每一次调用都要去找到它，
                // 现在 Unity 已经对它进行缓存过了，所以可以使用
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                // 向量坐标等于终点坐标减去起始坐标
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
            default:
                break;
        }

    }
}
