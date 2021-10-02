using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TestingTaskJunior
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text textTMP;
        private void OnEnable()
        {
            var ray = Camera.main.ViewportPointToRay(new Vector2(0, 1));
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 1);
            if (Physics.Raycast(ray, out var hit, 20))
                textTMP.text = hit.transform.name;
            else
                textTMP.text = "Empty";
        }
    }
}