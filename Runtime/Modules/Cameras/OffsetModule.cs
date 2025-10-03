using Unity.Cinemachine;
using System.Collections;
using UnityEngine;

namespace JuiceBits
{
    public class OffsetModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Offset";
        public float Duration = 0.5f;
        public float TimeBeforeReturn;
        public Vector3 Offset = new Vector3(-1f, 0f, 0f);
        public bool UseCurve;
        public bool StayAtOffset = false;
        public AnimationCurve AnimationCurve;
        public CinemachineCamera Camera;
        public EaseTypes EaseTypes = EaseTypes.Linear;

        private float _elapsedTime = 0f;
        private bool _isMoving;
        private Vector3 _originalCamera;
        private Vector3 _targetOffset;
        private Coroutine _coroutine;
        private Coroutine _repeatingCoroutine;
        private CinemachineCameraOffset _cinemachineOffset;

        public override void Initialize(GameObject targetObject)
        {
            _cinemachineOffset = Camera.GetComponentInChildren<CinemachineCameraOffset>();

            if (_cinemachineOffset == null)
            {
                _cinemachineOffset = Camera.gameObject.AddComponent<CinemachineCameraOffset>();
            }

            _originalCamera = _cinemachineOffset.Offset;
        }

        // Sets the default value of the animation curve to linear
        private void Reset()
        {
            AnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }

        public override void Update()
        {
            _cinemachineOffset = Camera.GetComponentInChildren<CinemachineCameraOffset>();
        }

        // Executes the offset module logic
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
                _coroutine = CoroutineManager.Instance.StartCoroutine(PlayOffset());
            }
        }

        // Stops the offset and resets the values to start again
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

            _isRepeating = false;
            _skipStartDelay = false;

            if (_runningUntilStopped)
            {
                CoroutineManager.Instance.StartCoroutine(PlayReturningOffset());
            }
        }

        // Enables the repetition of offset
        private IEnumerator RepeatModule()
        {
            for (int i = 1; i <= Repetitions; i++)
            {
                _skipStartDelay = i > 1;

                yield return CoroutineManager.Instance.StartCoroutine(PlayOffset());

                yield return new WaitForSeconds(RepeatDelay);
            }

            _isRepeating = false;

            // Checks if the repetition is done
            Finished();
        }

        // Offset logic
        private IEnumerator PlayOffset()
        {
            if (!_skipStartDelay && !IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            // Can't start more than one offset
            if (_isMoving)
                yield break;

            _isMoving = true;

            _originalCamera = _cinemachineOffset.Offset;
            _targetOffset = _originalCamera + Offset;

            _elapsedTime = 0f;

            // Lerp to the position with offset
            while (_elapsedTime < Duration)
            {
                _elapsedTime += Time.deltaTime;
                float time = _elapsedTime / Duration;
                float easedTime = UseCurve ? AnimationCurve.Evaluate(time) : EaseManger.Easings(EaseTypes, time);

                _cinemachineOffset.Offset = Vector3.Lerp(_originalCamera, _targetOffset, easedTime);

                yield return null;
            }

            // Loops the offset
            if (_runningUntilStopped)
            {
                StayAtOffset = true;
            }

            // The offset does not return
            if (StayAtOffset)
            {
                _isMoving = false;
                yield break;
            }

            yield return new WaitForSeconds(TimeBeforeReturn);

            yield return CoroutineManager.Instance.StartCoroutine(PlayReturningOffset());

            // Checks if the offset is done
            if (!_isRepeating)
            {
                Finished();
            }
        }

        // Returns the camera to the original position
        private IEnumerator PlayReturningOffset()
        {
            _isMoving = true;

            // current position
            Vector3 currentOffset = _cinemachineOffset.Offset; 
            _elapsedTime = 0f;

            while (_elapsedTime < Duration)
            {
                _elapsedTime += Time.deltaTime;
                float time = _elapsedTime / Duration;
                float easedTime = UseCurve ? AnimationCurve.Evaluate(time) : EaseManger.Easings(EaseTypes, time);

                _cinemachineOffset.Offset = Vector3.Lerp(currentOffset, _originalCamera, easedTime);
                yield return null;
            }

            _cinemachineOffset.Offset = _originalCamera;
            _isMoving = false;
        }
    }
}