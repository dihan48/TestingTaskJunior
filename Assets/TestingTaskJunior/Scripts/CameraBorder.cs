using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestingTaskJunior
{
    [RequireComponent(typeof(Camera))]
    public class CameraBorder : MonoBehaviour
    {
        [SerializeField]
        private Map map;

        private Camera cam;
        private CameraZoom cameraZoom;

        private float minX, maxX, minY, maxY;

        public float MinX => minX;
        public float MaxX => maxX;
        public float MinY => minY;
        public float MaxY => maxY;

        private void OnEnable()
        {
            if (map == null)
                throw new NullReferenceException("map");

            map.OnGenerated += SetBorderSize;

            if (TryGetComponent(out cameraZoom))
                cameraZoom.OnZoomed += SetBorderSize;
        }

        private void Start()
        {
            TryGetComponent(out cam);
        }

        private void OnDisable()
        {
            map.OnGenerated -= SetBorderSize;
            if(cameraZoom != null)
                cameraZoom.OnZoomed -= SetBorderSize;
        }

        private void SetBorderSize()
        {
            var pos = map.transform.position;
            var size = map.Size;
            var orthographicSize = cam.orthographicSize;
            var aspect = cam.aspect;

            minX = pos.x - size.x / 2 + orthographicSize * aspect;
            maxX = pos.x + size.x / 2 - orthographicSize * aspect;

            minY = pos.y - size.y / 2 + orthographicSize;
            maxY = pos.y + size.y / 2 - orthographicSize;
        }

        public Vector3 StayBorder(Vector3 pos)
        {
            pos.Set(
                Mathf.Clamp(pos.x, MinX, maxX),
                Mathf.Clamp(pos.y, MinY, maxY),
                pos.z);
            return pos;
        }
    }
}