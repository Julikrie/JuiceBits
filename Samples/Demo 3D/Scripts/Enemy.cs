using UnityEngine;

namespace JuiceBits
{
    public class Enemy : MonoBehaviour
    {
        public AudioClip Explosion;

        // Reference to the effect containers
        public ModuleHandler HitEffects;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                // Plays the HitEffects on collision with the projectiles
                HitEffects.PlayModules();
                _audioSource.PlayOneShot(Explosion, 0.7f);
            }
        }
    }
}