using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestingTaskJunior
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private float lerpScale = 5;

        private bool isDrag = false;
        private Camera cam;
        private CameraBorder border;
        private bool useBorder;
        private Vector2 mousePos;
        private Vector3 mouseDelta;
        private float lerpTime = 1;

        private void Start()
        {
            TryGetComponent(out cam);
            useBorder = TryGetComponent(out border);
        }

        public void OnStartDrag()
        {
            isDrag = true;
        }

        public void OnEndDrag()
        {
            isDrag = false;
            lerpTime = 0;
        }

        public void OnDrag(InputValue input)
        {
            mousePos = input.Get<Vector2>();
        }

        private void Update()
        {
            if (isDrag)
            {
                mouseDelta = ViewporToUnits(cam.ScreenToViewportPoint(mousePos));
                transform.Translate(-mouseDelta);
                if (useBorder)
                    transform.position = border.StayBorder(transform.position);
            }
            else if (lerpTime < 1)
            {
                lerpTime += Time.deltaTime / lerpScale;
                mouseDelta = Vector2.Lerp(mouseDelta, Vector2.zero, lerpTime);
                transform.Translate(-mouseDelta);
                if (useBorder)
                    transform.position = border.StayBorder(transform.position);
            }
        }

        private Vector3 ViewporToUnits(Vector3 viewPoint)
        {
            return new Vector3(viewPoint.x * cam.orthographicSize * 2 * cam.aspect, viewPoint.y * cam.orthographicSize * 2);
        }
    }
}