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
        private InputUIProxy inputHandler;

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
            if (TryGetComponent(out inputHandler))
            {
                inputHandler.OnMultiDragStarted += StartZoom;
                inputHandler.OnMultiDraged += Zoom;
            }
        }

        private void OnDisable()
        {
            if (cameraZoom != null)
            {
                inputHandler.OnMultiDragStarted -= StartZoom;
                cameraZoom.OnInitZoom -= SetStartZoom;
            }
        }

        private void StartZoom()
        {
            startDeltaTouches = 0;
            startZoom = zoom;
        }

        private void Zoom(float delta)
        {
            var deltaTouches = delta * deltaTouchesMultiplier;
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

        private void SetStartZoom(float zoom)
        {
            this.zoom = zoom;
            startZoom = zoom;
        }
    }
}