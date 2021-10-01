using UnityEngine;
using UnityEngine.InputSystem;

namespace TestingTaskJunior
{
    public class MouseScrollZoom : MonoBehaviour
    {
        [SerializeField]
        private int scrollMagnitude = 8;
        [SerializeField]
        private float amountSmoothingTime = 4;

        private CameraZoom cameraZoom;
        private float startZoom;

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
        }

        private void OnDisable()
        {
            if (cameraZoom != null)
                cameraZoom.OnInitZoom -= SetStartZoom;
        }

        public void OnZoom(InputValue input)
        {
            var scrollInput = input.Get<float>();
            if (scrollInput != 0)
            {
                var scrollDir = ((scrollInput > 0) ? 1f : -1f) / scrollMagnitude;
                var zoom = Mathf.Clamp(startZoom + scrollDir, 0, 1);
                startZoom = zoom;
                cameraZoom.Zoom(zoom, amountSmoothingTime);
            }
        }

        private void SetStartZoom(float zoom)
        {
            startZoom = zoom;
        }
    }
}
