using UnityEngine;

namespace JuiceBits
{
    public class CharacterController3D : MonoBehaviour
    {
        public float Speed;
        public float JumpHeight;
        public float Gravity;
        public float NegateSkinWidth;
        public bool _isGrounded;

        private CharacterController _characterController;
        private Vector3 _velocity;

        void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            float movementX = Input.GetAxis("Horizontal");
            float movementY = Input.GetAxis("Vertical");

            Vector3 movement = transform.right * movementX + transform.forward * movementY;
            _characterController.Move(movement * Speed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = Mathf.Sqrt(JumpHeight * NegateSkinWidth * Gravity);
            }

            _velocity.y += Gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            _isGrounded = _characterController.isGrounded;

            // Negates the Skin width
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = NegateSkinWidth;
            }
        }
    }
}