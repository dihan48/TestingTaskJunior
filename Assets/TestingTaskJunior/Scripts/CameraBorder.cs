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
        }

        private void Start()
        {
            TryGetComponent(out cam);
        }

        private void OnDisable()
        {
            map.OnGenerated -= SetBorderSize;
        }

        private void SetBorderSize()
        {
            var pos = map.transform.position;
            var size = map.Size;

            Debug.Log(map.Size);

            minX = pos.x - size.x / 2 + cam.orthographicSize * cam.aspect;
            maxX = pos.x + size.x / 2 - cam.orthographicSize * cam.aspect;

            minY = pos.y - size.y / 2 + cam.orthographicSize;
            maxY = pos.y + size.y / 2 - cam.orthographicSize;
        }

        public Vector3 StayBorder(Vector3 pos)
        {
            var newPos = pos;

            if (pos.x < MinX)
                newPos.Set(MinX, newPos.y, pos.z);
            if (pos.x > MaxX)
                newPos.Set(MaxX, newPos.y, pos.z);
            if (pos.y < MinY)
                newPos.Set(newPos.x, MinY, pos.z);
            if (pos.y > MaxY)
                newPos.Set(newPos.x, MaxY, pos.z);

            return newPos;
        }
    }
}