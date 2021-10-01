using UnityEngine;
using UnityEngine.InputSystem;

namespace TestingTaskJunior
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private float amountSmoothingTime = 2;
        [SerializeField]
        private float maxSmoothingVelocity = 0.2f;

        private Camera cam;
        private CameraBorder border;
        private int touchCount = 0;
        private Vector2 dragDelta;
        private Vector2 startSmoothingVelocity;
        private bool isDrag = false;
        private float smoothingTime;
        private bool isSmoothing;

        public bool IsSmoothing { 
            get => isSmoothing;
            set 
            {
                isSmoothing = value;
                if(value)
                    smoothingTime = 0;
            }
        }

        private void OnEnable()
        {
            TryGetComponent(out cam);
            if(TryGetComponent(out border))
                border.OnChanged += StayBorder;
        }

        private void OnDisable()
        {
            if(border != null)
                border.OnChanged -= StayBorder;
        }

        private void Update()
        {
            if (isDrag)
            {
                var offset = cam.ViewportToWorldPoint(new Vector2(0, 0)) - cam.ScreenToWorldPoint(dragDelta);
                startSmoothingVelocity = Vector2.ClampMagnitude(offset, maxSmoothingVelocity) / Time.deltaTime;
                IsSmoothing = true;
                Move(offset);
            }
            else if (IsSmoothing)
            {
                smoothingTime += Time.deltaTime / amountSmoothingTime;
                if (smoothingTime >= 1)
                    IsSmoothing = false;
                var smoothingVelocity = Vector2.Lerp(startSmoothingVelocity, Vector2.zero, smoothingTime);
                Move(smoothingVelocity * Time.deltaTime);
            }
        }

        public void OnTouch0(InputValue input)
        {
            if (input.Get<float>() == 1)
            {
                touchCount++;
                isDrag = (touchCount == 1);
                IsSmoothing = false;
            }
            else
            {
                touchCount--;
                isDrag = false;
                IsSmoothing = (touchCount == 0);
            }
        }

        public void OnTouch1(InputValue input)
        {
            if (input.Get<float>() == 1)
                touchCount++;
            else
                touchCount--;
            isDrag = false;
            IsSmoothing = false;
        }

        public void OnDrag(InputValue input)
        {
            dragDelta = input.Get<Vector2>();
        }

        private void Move(Vector3 offset)
        {
            if (border != null)
                transform.position = border.StayBorder(transform.position + offset);
            else
                transform.Translate(offset);
        }

        private void StayBorder()
        {
            transform.position = border.StayBorder(transform.position);
        }
    }
}