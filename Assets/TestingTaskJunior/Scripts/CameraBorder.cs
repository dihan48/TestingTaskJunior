using System;
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

        public event Action OnChanged;

        private void OnEnable()
        {
            TryGetComponent(out cam);
            map.OnGenerated += SetBorderSize;
            if (TryGetComponent(out cameraZoom))
                cameraZoom.OnZoomed += SetBorderSize;
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

            OnChanged?.Invoke();
        }

        public Vector3 StayBorder(Vector3 pos)
        {
            pos.Set(
                Mathf.Clamp(pos.x, minX, maxX),
                Mathf.Clamp(pos.y, minY, maxY),
                pos.z);
            return pos;
        }
    }
}