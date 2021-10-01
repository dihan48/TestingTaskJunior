using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TestingTaskJunior
{
    public class InputUIProxy : MonoBehaviour
    {
        [SerializeField]
        private GraphicRaycaster raycaster;
        [SerializeField]
        private EventSystem eventSystem;

        private Camera cam;
        private PlayerInput playerInput;
        private Vector2 dragDelta;
        private int touchCount;
        private bool isTouch0 = false;
        private bool isTouch1 = false;
        private bool isTouch0Changed = false;
        private bool isTouch1Changed = false;
        private Vector2 pos0;
        private Vector2 pos1;
        private bool isTouchUI;

        public delegate void Drag(Vector2 delta);
        public delegate void MultiDragStart();
        public delegate void MultiDrag(float deltaTouches);
        public delegate void DragStop(bool isOnlyTouch0);

        public event Drag OnDraged;
        public event MultiDragStart OnMultiDragStarted;
        public event MultiDrag OnMultiDraged;
        public event DragStop OnDragStoped;

        private void Start()
        {
            cam = Camera.main;
            TryGetComponent(out playerInput);
        }

        private void Update()
        {
            if (isTouch0Changed)
            {
                if (touchCount == 1)
                {
                    var pointerEventData = new PointerEventData(eventSystem);
                    pointerEventData.position = pos0;
                    var results = new List<RaycastResult>();
                    raycaster.Raycast(pointerEventData, results);
                    isTouchUI = results.Count > 0;
                    playerInput.uiInputModule.enabled = isTouchUI;
                }
                else if (touchCount == 0)
                {
                    playerInput.uiInputModule.enabled = true;
                }
            }

            if (isTouch1Changed && isTouch0 && isTouch1)
            {
                OnMultiDragStarted?.Invoke();
            }

            if (isTouchUI == false)
            {
                if (isTouch0Changed || isTouch1Changed)
                    OnDragStoped?.Invoke(touchCount == 0);

                switch (touchCount)
                {
                    case 1:
                        OnDraged?.Invoke(dragDelta);
                        break;
                    case 2:
                        var deltaTouches = cam.ScreenToViewportPoint(pos0 - pos1).magnitude;
                        OnMultiDraged?.Invoke(deltaTouches);
                        break;
                    default:
                        break;
                }
            }

            if (isTouchUI && touchCount == 0)
                isTouchUI = false;

            isTouch0Changed = false;
            isTouch1Changed = false;
        }

        public void OnTouch0(InputValue input)
        {
            isTouch0 = input.Get<float>() == 1;
            isTouch0Changed = true;
            if (isTouch0)
            {
                touchCount++;
            }
            else
            {
                touchCount--;
            }
        }

        public void OnTouch1(InputValue input)
        {
            isTouch1 = input.Get<float>() == 1;
            isTouch1Changed = true;
            if (isTouch1)
            {
                touchCount++;
            }
            else
            {
                touchCount--;
            }
        }

        public void OnDrag(InputValue input)
        {
            dragDelta = input.Get<Vector2>();
        }

        public void OnPosTouch0(InputValue input)
        {
            pos0 = input.Get<Vector2>();
        }

        public void OnPosTouch1(InputValue input)
        {
            pos1 = input.Get<Vector2>();
        }
    }
}