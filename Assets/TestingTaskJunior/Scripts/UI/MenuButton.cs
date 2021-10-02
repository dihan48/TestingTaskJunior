using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace TestingTaskJunior
{
    public class MenuButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private PlayerInput playerInput;
        [SerializeField]
        private Menu menu;

        public void OnPointerClick(PointerEventData eventData)
        {
            menu.gameObject.SetActive(playerInput.currentActionMap.enabled);
            if(playerInput.currentActionMap.enabled)
                playerInput.currentActionMap.Disable();
            else
                playerInput.currentActionMap.Enable();
        }
    }
}