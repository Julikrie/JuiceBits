using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

namespace JuiceBits
{
    public class ShakeModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Shake";
        public float ShakeDuration = 0.5f;
        public Vector3 ShakeDirection = new Vector3(0, 0.2f, 0);
        public AnimationCurve AnimationCurve;
        public ShakeType ShakeType = ShakeType.Bump;
        public CinemachineCamera CinemachineCamera;

        private CinemachineImpulseListener _impulseListener;
        private CinemachineImpulseSource _impulseSource;
        private Coroutine _coroutine;
        private Coroutine _repeatingCoroutine;

        // Prepare camera components to use shake
        public override void Initialize(GameObject targetObject)
        {
            _impulseListener = CinemachineCamera.GetComponent<CinemachineImpulseListener>();
            _impulseSource = targetObject.GetComponent<CinemachineImpulseSource>();

            if (_impulseListener == null)
            {
                _impulseListener = CinemachineCamera.gameObject.AddComponent<CinemachineImpulseListener>();
                _impulseListener.ApplyAfter = CinemachineCore.Stage.Noise;    // default value
                _impulseListener.ChannelMask = 1;                             // default value
                _impulseListener.Gain = 1f;                                   // default value
                _impulseListener.UseCameraSpace = false;
                _impulseListener.ReactionSettings.AmplitudeGain = 1.0f;     // default value
                _impulseListener.ReactionSettings.FrequencyGain = 1.0f;     // default value
                _impulseListener.ReactionSettings.Duration = 1.0f;          // default value
            }

            if (_impulseSource == null)
            {
                _impulseSource = targetObject.AddComponent<CinemachineImpulseSource>();
            }

            _impulseSource.ImpulseDefinition.ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform;   // default value
        }

        // Sets the default value of the animation curve to linear
        private void Reset()
        {
            AnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }

        public override void Update()
        {
            _impulseSource.DefaultVelocity = ShakeDirection;
            _impulseSource.ImpulseDefinition.ImpulseDuration = ShakeDuration;

            SelectShake();
        }

        // Executes the shake logic
        public override void Play()
        {
            bool isPlaying = true;

            if (Probability > 0f)
            {
                float randomValue = Random.Range(0f, 100f);

                if (randomValue > Probability)
                {
                    isPlaying = false;
                }
            }

            if (!isPlaying)
                return;

            if (Repetitions > 0)
            {
                if (!_isRepeating)
                {
                    _isRepeating = true;
                    _repeatingCoroutine = CoroutineManager.Instance.StartCoroutine(RepeatModule());
                }
            }
            else
            {
                _coroutine = CoroutineManager.Instance.StartCoroutine(PlayShake());
            }
        }

        // Stops the shake
        public override void Stop()
        {
            if (_coroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (_repeatingCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_repeatingCoroutine);
                _repeatingCoroutine = null;
            }

            // Stops the shake abruptly
            _impulseListener.Gain = 0f;
        }

        // Shake logic
        private IEnumerator PlayShake()
        {
            _impulseListener.Gain = 1f;

            if (!_skipStartDelay && !IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            _impulseSource.GenerateImpulse(ShakeDirection);

            if (!_isRepeating)
            {
                Finished();
            }
        }

        // Enables the repetition of shake
        private IEnumerator RepeatModule()
        {
            {
                for (int i = 1; i <= Repetitions; i++)
                {
                    _skipStartDelay = i > 1;

                    yield return CoroutineManager.Instance.StartCoroutine(PlayShake());

                    yield return new WaitForSeconds(RepeatDelay);
                }
            }

            _isRepeating = false;

            // Checks if the shake repetition is finished
            Finished();
        }

        // Sets the different shake types to the correct Enum
        public void SelectShake()
        {
            switch (ShakeType)
            {
                case ShakeType.Custom:
                    ShakeCustom();
                    break;
                case ShakeType.Recoil:
                    ShakeRecoil();
                    break;
                case ShakeType.Bump:
                    ShakeBump();
                    break;
                case ShakeType.Explosion:
                    ShakeExplosion();
                    break;
                case ShakeType.Rumble:
                    ShakeRumble();
                    break;
            }
        }

        // Assigns shake presets to the Enums
        private void ShakeCustom()
        {
            _impulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Custom;
            _impulseSource.ImpulseDefinition.CustomImpulseShape = AnimationCurve;
        }

        private void ShakeRecoil()
        {
            _impulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil;
        }

        private void ShakeBump()
        {
            _impulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Bump;
        }

        private void ShakeExplosion()
        {
            _impulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
        }

        private void ShakeRumble()
        {
            _impulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;
        }
    }
}