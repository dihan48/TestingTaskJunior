using UnityEngine;
using UnityEngine.EventSystems;

namespace TestingTaskJunior
{
    public class MenuButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("MenuButton");
        }
    }
}