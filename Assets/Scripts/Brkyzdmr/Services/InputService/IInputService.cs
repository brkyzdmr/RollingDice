using UnityEngine;

namespace Brkyzdmr.Services.InputService
{
    public interface IInputService
    {
        bool isInputActive { get; set; }
        int touchCount => Input.touchCount;
        bool GetMouseButton(int button);
        bool GetMouseButtonDown(int button);
        bool GetMouseButtonUp(int button);
        Vector2 GetMousePosition();
        bool IsPointerOverUI(int fingerId = -1);

        Touch GetTouch(int index);
        float GetMouseScrollDelta();
    }
}