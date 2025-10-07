using UnityEngine;

namespace JuiceBits
{
    public class PlayerController2D : MonoBehaviour
    {
        public float _jumpForce = 20f;
        public float FallGravity;
        public AudioClip JumpSound;
        public AudioClip LandingSound;
        public AudioClip CoinCollect;

        // Reference to the effect containers
        public ModuleHandler JumpEffects;
        public ModuleHandler BoosterEffects;
        public ModuleHandler CollisionEffects;

        private float _originalGravity;
        private bool _isGrounded;
        private bool _hasIncreasedGravity = false;
        private Rigidbody2D _rb;
        private Animator _animator;
        private AudioSource _audioSoruce;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _originalGravity = _rb.gravityScale;
            _animator = GetComponent<Animator>();
            _audioSoruce = GetComponent<AudioSource>();
        }

        private void Update()
        {
            Jumping();
        }

        private void Jumping()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rb.gravityScale = _originalGravity;
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                _animator.SetBool("isJumping", true);
                _audioSoruce.PlayOneShot(JumpSound, 0.3f);

                if (JumpEffects != null)
                {
                    // Plays all JumpEffect modules when jumping
                    JumpEffects.PlayModules();
                }

                _isGrounded = false;
                _hasIncreasedGravity = false;
            }

            if (_rb.linearVelocity.y < 0f && !_hasIncreasedGravity)
            {
                _rb.gravityScale = _originalGravity * FallGravity;
                _hasIncreasedGravity = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true;
                _hasIncreasedGravity = false;
                _animator.SetBool("isJumping", false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Coin"))
            {
                if (CollisionEffects != null)
                {
                    // Plays all CollisionEffects after picking up coins
                    CollisionEffects.PlayModules();
                }

                FindFirstObjectByType<GameManager2D>().CollectCoins(10);
                _audioSoruce.PlayOneShot(CoinCollect, 0.3f);
            }

            if (collision.gameObject.CompareTag("Booster"))
            {
                // Plays all BoosterEffects after picking up a booster
                BoosterEffects.PlayModules();
                _audioSoruce.PlayOneShot(CoinCollect, 0.3f);
            }
        }
    }
}