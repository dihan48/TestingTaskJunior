using UnityEngine;
using UnityEngine.EventSystems;

namespace TestingTaskJunior
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            InputHandler.OnPointerEnterUI();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InputHandler.OnPointerExitUI();
        }
    }
}