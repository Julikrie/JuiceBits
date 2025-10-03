using UnityEngine;

namespace JuiceBits
{
    public class SpawnedObjects : MonoBehaviour
    {
        private float _speed;

        // Sets the speed of the coin party
        public void SetPartySpeed(float speed)
        {
            _speed = speed;
        }

        void Update()
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        // Destroys them
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }

            if (collision.gameObject.CompareTag("Destroyer"))
            {
                Destroy(gameObject);
            }
        }
    }
}