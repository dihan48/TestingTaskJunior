using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestingTaskJunior
{
    public class Map : MonoBehaviour
    {
        private const float PixelsPerUnit = 100;

        [SerializeField]
        private TextAsset mapJson;
        [SerializeField]
        private Vector2 spritePivot = new Vector2(0, 1);


        private Vector2 size;

        public Vector2 Size => size;

        public event Action OnGenerated;

        private void Start()
        {
            Genarate();
        }

        private void Genarate()
        {
            if (mapJson == null)
                throw new NullReferenceException("mapJson");
            var mapInfo = JsonUtility.FromJson<MapInfo>(mapJson.text);
            if (mapInfo.List == null)
                Debug.LogError("Map format error");
            size = GetSize(mapInfo);
            var offset = new Vector3(-size.x / 2, size.y / 2);

            foreach (var cellInfo in mapInfo.List)
            {
                if (cellInfo.Id == null)
                    Debug.LogError("Map cell format error");
                var texture = Resources.Load<Texture2D>(cellInfo.Id);
                if (texture == null)
                    Debug.LogError($"Texture {cellInfo.Id} not found");
                var sprite = Sprite.Create(texture, new Rect(0, 0, cellInfo.Width * PixelsPerUnit, cellInfo.Height * PixelsPerUnit), spritePivot, PixelsPerUnit);

                var cell = new GameObject(cellInfo.Id);
                cell.transform.parent = transform;
                cell.transform.position = new Vector3(cellInfo.X, cellInfo.Y) + offset;

                var spriteRenderer = cell.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
            }

            OnGenerated?.Invoke();
        }

        private Vector2 GetSize(MapInfo mapInfo)
        {
            float x = 0;
            float y = 0;

            foreach (var cell in mapInfo.List)
            {
                if (x < cell.X + cell.Width)
                    x = cell.X + cell.Width;
                if (y < -cell.Y + cell.Height)
                    y = -cell.Y + cell.Height;
            }

            return new Vector2(x, y);
        }
    }

    [Serializable]
    public struct MapInfo
    {
        public MapCellInfo[] List;
    }

    [Serializable]
    public struct MapCellInfo
    {
        public string Id;
        public string Type;
        public float X;
        public float Y;
        public float Width;
        public float Height;
    }
}
