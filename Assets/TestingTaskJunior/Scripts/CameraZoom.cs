using System;
using UnityEngine;

namespace TestingTaskJunior
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField]
        private Map map;
        [SerializeField]
        private float minZoom = 3;


        private Camera cam;
        private float zoom;
        private float maxZoom;
        private float startSmoothingZoom;
        private float amountSmoothingTime;
        private float smoothingTime;
        private bool isSmoothing;

        public bool IsSmoothing
        {
            get => isSmoothing;
            set
            {
                isSmoothing = value;
                if (value)
                    smoothingTime = 0;
            }
        }

        public event Action<float> OnInitZoom;
        public event Action OnZoomed;


        private void OnEnable()
        {
            map.OnGenerated += Init;
            TryGetComponent(out cam);
        }

        private void OnDisable()
        {
            map.OnGenerated -= Init;
        }

        private void Update()
        {
            if (IsSmoothing)
            {
                smoothingTime += Time.deltaTime / amountSmoothingTime;
                if (smoothingTime >= 1)
                    IsSmoothing = false;
                cam.orthographicSize = Mathf.Lerp(startSmoothingZoom, zoom, smoothingTime);
                OnZoomed?.Invoke();
            }
        }

        private void Init()
        {
            maxZoom = map.Size.y / 2;
            zoom = Mathf.Clamp(maxZoom, minZoom, cam.orthographicSize);
            var deltaZoom = maxZoom - minZoom;
            var initZoom = (maxZoom - zoom) / deltaZoom;
            OnInitZoom?.Invoke(initZoom);
        }

        public void Zoom(float value, float amountSmoothingTime)
        {
            IsSmoothing = true;
            startSmoothingZoom = cam.orthographicSize;
            zoom = Mathf.Lerp(maxZoom, minZoom, value);
            this.amountSmoothingTime = amountSmoothingTime;
        }
    }
}