using UnityEngine;
using UnityEngine.InputSystem;

namespace TestingTaskJunior
{
    [RequireComponent(typeof(Camera))]
    public class TouchZoom : MonoBehaviour
    {
        [SerializeField]
        private float deltaTouchesMultiplier = 3;
        [SerializeField]
        private float amountSmoothingTime = 6;

        private CameraZoom cameraZoom;
        private Camera cam;

        private bool press0 = false;
        private bool press1 = false;
        private Vector2 pos0;
        private Vector2 pos1;

        private float zoom;
        private float startZoom;
        private float startDeltaTouches;

        private void OnEnable()
        {
            if (TryGetComponent(out cameraZoom) == false)
            {
                Debug.LogError("CameraZoom not found!");
                enabled = false;
            }
            else
            {
                cameraZoom.OnInitZoom += SetStartZoom;
            }
            TryGetComponent(out cam);
        }

        private void OnDisable()
        {
            if(cameraZoom != null)
                cameraZoom.OnInitZoom -= SetStartZoom;
        }

        private void Update()
        {
            if (press0 && press1)
            {
                var deltaTouches = cam.ScreenToViewportPoint(pos0 - pos1).magnitude * deltaTouchesMultiplier;
                if (startDeltaTouches == 0)
                    startDeltaTouches = deltaTouches;
                if (cameraZoom != null)
                {
                    zoom = Mathf.Clamp(startZoom + deltaTouches - startDeltaTouches, 0, 1);
                    if (zoom == 1 || zoom == 0)
                    {
                        startDeltaTouches = deltaTouches;
                        startZoom = zoom;
                    }
                    cameraZoom.Zoom(zoom, amountSmoothingTime);
                }
            }
        }

        public void OnTouch0(InputValue input)
        {
            press0 = (input.Get<float>() == 1);
            if (press0 == false)
            {
                startDeltaTouches = 0;
                startZoom = zoom;
            }
        }

        public void OnTouch1(InputValue input)
        {
            press1 = (input.Get<float>() == 1);
            if (press1 == false)
            {
                startDeltaTouches = 0;
                startZoom = zoom;
            }
        }

        public void OnDragTouch0(InputValue input)
        {
            pos0 = input.Get<Vector2>();
        }

        public void OnDragTouch1(InputValue input)
        {
            pos1 = input.Get<Vector2>();
        }

        private void SetStartZoom(float zoom)
        {
            this.zoom = zoom;
            startZoom = zoom;
        }
    }
}