using UnityEngine;

namespace JuiceBits
{
    public class TankShooting : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public Transform FirePoint;
        public float ProjectileSpeed = 25f;
        public float FireRate = 0.5f;
        public AudioClip ShotSound;

        // Reference to the effect containers
        public ModuleHandler ShotEffects;

        private float _fireCooldown = 0f;
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            _fireCooldown += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && _fireCooldown > FireRate)
            {
                Fire();
            }
        }

        private void Fire()
        {
            GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
            Rigidbody projecticleRigidbody = projectile.GetComponent<Rigidbody>();

            // Starts the ShotEffects when the tank is shooting with space
            ShotEffects.PlayModules();
            _audioSource.PlayOneShot(ShotSound, 0.7f);

            projecticleRigidbody.linearVelocity = FirePoint.forward * ProjectileSpeed;

            _fireCooldown = 0f;
        }
    }
}