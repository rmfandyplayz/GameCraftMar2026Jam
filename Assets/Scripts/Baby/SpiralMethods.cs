using Baby;
using UnityEngine;

namespace Baby
{
    public class SpiralMethods : MonoBehaviour
    {
        private BabyController controller;

        void OnEnable()
        {
            controller = FindAnyObjectByType<BabyController>();
        }

        public void MindControlCheck()
        {
            StartCoroutine(controller.McCheckEnumerator());
        }
        
        public void DisableMindControlCheck()
        {
            this.gameObject.SetActive(false);
        }
    }
}
