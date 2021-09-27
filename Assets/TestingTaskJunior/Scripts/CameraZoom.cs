using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestingTaskJunior
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField]
        private Map map;
        [SerializeField]
        private float maxZoom = 3;

        private Camera cam;
        private float zoomDelta;
        private Vector2 mapSize;

        public event Action OnZoomed;

        private void Start()
        {
            if (map == null)
                throw new NullReferenceException("map");
            mapSize = map.Size;

            TryGetComponent(out cam);
        }

        public void OnZoom(InputValue input)
        {
            zoomDelta = Mathf.Clamp(input.Get<float>(), -1, 1);
        }

        private void Update()
        {
            if (zoomDelta != 0)
            {
                var orthographicSize = cam.orthographicSize + zoomDelta;
                cam.orthographicSize = Mathf.Clamp(orthographicSize, maxZoom, mapSize.y / 2);
                OnZoomed?.Invoke();
            }
        }
    }
}