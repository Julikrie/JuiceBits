using UnityEngine;
using TMPro;

namespace JuiceBits
{
    public class GameManager3D : MonoBehaviour
    {
        public TMP_Text StartText;

        private void Update()
        {
            StartDemo();
        }

        // Deactivates the start text after pressing space
        private void StartDemo()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartText.enabled = false;
            }
        }
    }
}