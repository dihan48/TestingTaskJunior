using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestingTaskJunior
{
    public class InputHandler : MonoBehaviour
    {
        private Camera cam;
        private Vector2 dragDelta;
        private int touchCount;
        private bool isOnlyTouch0;
        private Vector2 pos0;
        private Vector2 pos1;

        public delegate void DragStart(Vector2 delta);
        public delegate void MultiDragStart(float deltaTouches);
        public delegate void DragStop(bool isOnlyTouch0);

        public event DragStart OnDragStarted;
        public event MultiDragStart OnMultiDragStarted;
        public event DragStop OnDragStoped;

        private static bool isInteractUI = false;
        private static bool isIgnoreUI = false;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (isInteractUI && isIgnoreUI == false)
            {
                Debug.Log("isInteractUI");
            }
            else
            {
                switch (touchCount)
                {
                    case 0:
                        if (isIgnoreUI)
                        {
                            isIgnoreUI = false;
                            OnDragStoped?.Invoke(isOnlyTouch0);
                        }
                        break;
                    case 1:
                        isIgnoreUI = true;
                        OnDragStarted?.Invoke(dragDelta);
                        break;
                    case 2:
                        isIgnoreUI = true;
                        var deltaTouches = cam.ScreenToViewportPoint(pos0 - pos1).magnitude;
                        OnMultiDragStarted?.Invoke(deltaTouches);
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnTouch0(InputValue input)
        {
            if (input.Get<float>() == 1)
            {
                touchCount++;
                isOnlyTouch0 = true;
            }
            else
            {
                touchCount--;
                isOnlyTouch0 = touchCount == 0;
            }
        }

        public void OnTouch1(InputValue input)
        {
            if (input.Get<float>() == 1)
                touchCount++;
            else
                touchCount--;

            isOnlyTouch0 = false;
        }

        public void OnDrag(InputValue input)
        {
            dragDelta = input.Get<Vector2>();
        }

        public void OnDragTouch0(InputValue input)
        {
            pos0 = input.Get<Vector2>();
        }

        public void OnDragTouch1(InputValue input)
        {
            pos1 = input.Get<Vector2>();
        }

        public static void OnPointerEnterUI()
        {
            isInteractUI = true;
        }

        public static void OnPointerExitUI()
        {
            isInteractUI = false;
        }
    }
}