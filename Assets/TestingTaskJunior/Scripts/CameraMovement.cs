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
        private Vector2 startMousePos;
        private Vector3 camPos;
        private Vector3 mouseDelta;
        private Vector2 lerpMouseDelta;
        private float lerpTime = 1;
        private Vector3 speed;

        private void Start()
        {
            TryGetComponent(out cam);
            useBorder = TryGetComponent(out border);
        }

        public void OnStartDrag()
        {
            isDrag = true;
            startMousePos = mousePos;
            camPos = transform.position;
        }

        public void OnEndDrag()
        {
            isDrag = false;
            lerpTime = 0;
            camPos = transform.position;
        }

        public void OnDrag(InputValue input)
        {
            mousePos = input.Get<Vector2>();
        }

        public void Onhz(InputValue input)
        {
            Debug.Log(input.Get());
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
            if (isDrag == false && lerpTime < 1)
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