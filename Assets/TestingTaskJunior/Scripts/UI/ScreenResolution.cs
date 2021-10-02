using System;
using UnityEngine;

namespace TestingTaskJunior
{
    [RequireComponent(typeof(Canvas))]
    public class ScreenResolution : MonoBehaviour
    {
        public static Action OnChanged;

        private void OnRectTransformDimensionsChange()
        {
            OnChanged?.Invoke();
        }
    }
}