using UnityEngine;

namespace JuiceBits
{
    public class RigidBodyController : MonoBehaviour
    {
        public float MoveSpeed = 20f;
        public float JumpHeight = 10f;
        public ModuleHandler JumpEffects;

        private Rigidbody _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();

        }

        void Update()
        {
            float horizotanlInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizotanlInput, 0f, verticalInput).normalized;

            _rb.linearVelocity = new Vector3(movement.x * MoveSpeed, _rb.linearVelocity.y, movement.z * MoveSpeed);

            Jump();
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, JumpHeight, _rb.linearVelocity.z);
                JumpEffects.PlayModules();
            }
        }
    }
}