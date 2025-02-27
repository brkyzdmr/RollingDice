﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Brkyzdmr.Services.InputService
{
    public class UnityInputService : Service, IInputService
    {
        public bool isInputActive { get; set; }
        public int touchCount => Input.touchCount;


        public bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }

        public bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }

        public bool GetMouseButtonUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }

        public Vector2 GetMousePosition()
        {
            return Input.mousePosition;
        }

        public bool IsPointerOverUI(int fingerId = -1)
        {
            if (EventSystem.current != null)
            {
                if (EventSystem.current.IsPointerOverGameObject(fingerId))
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };
                    List<RaycastResult> raycastResults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, raycastResults);

                    foreach (var result in raycastResults)
                    {
                        if (result.gameObject.CompareTag("ExcludedUI"))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public Touch GetTouch(int index)
        {
            return Input.GetTouch(index);
        }
        
        public float GetMouseScrollDelta()
        {
            return -Input.mouseScrollDelta.y;
        }
    }
}