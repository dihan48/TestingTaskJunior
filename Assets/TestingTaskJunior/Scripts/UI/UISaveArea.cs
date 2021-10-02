using UnityEngine;

namespace TestingTaskJunior
{
    [RequireComponent(typeof(RectTransform))]
    public class UISaveArea : MonoBehaviour
    {
        private RectTransform rectTransform;

        private void OnEnable()
        {
            TryGetComponent(out rectTransform);
            StaySafeArea();
            ScreenResolution.OnChanged += StaySafeArea;
        }

        private void OnDisable()
        {
            ScreenResolution.OnChanged -= StaySafeArea;
        }

        private void StaySafeArea()
        {
            var saveArea = Screen.safeArea;
            if(Screen.safeArea.width > Screen.width || Screen.safeArea.height > Screen.height)
                return;
            var anchorMin = saveArea.position;
            var anchorMax = anchorMin + saveArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}