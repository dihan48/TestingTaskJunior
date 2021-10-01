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
        private Vector2 startSmoothingVelocity;
        private float smoothingTime;
        private bool isSmoothing;
        private InputUIProxy inputHandler;

        public bool UseSmoothing { 
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
            if(TryGetComponent(out inputHandler))
            {
                inputHandler.OnDraged += Drag;
                inputHandler.OnDragStoped += Smoothing;
            }
            if(TryGetComponent(out border))
                border.OnChanged += StayBorder;
        }

        private void OnDisable()
        {
            if(border != null)
                border.OnChanged -= StayBorder;
            if(inputHandler)
            {
                inputHandler.OnDraged -= Drag;
                inputHandler.OnDragStoped -= Smoothing;
            }
        }

        private void Update()
        {
            if (UseSmoothing)
            {
                smoothingTime += Time.deltaTime / amountSmoothingTime;
                if (smoothingTime >= 1)
                    UseSmoothing = false;
                var smoothingVelocity = Vector2.Lerp(startSmoothingVelocity, Vector2.zero, smoothingTime);
                Move(smoothingVelocity * Time.deltaTime);
            }
        }

        public void Drag(Vector2 dragDelta)
        {
            var offset = cam.ViewportToWorldPoint(new Vector2(0, 0)) - cam.ScreenToWorldPoint(dragDelta);
            startSmoothingVelocity = Vector2.ClampMagnitude(offset, maxSmoothingVelocity) / Time.deltaTime;
            UseSmoothing = true;
            Move(offset);
        }

        public void Smoothing(bool value) => UseSmoothing = value;

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