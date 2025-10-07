using Unity.Cinemachine;
using System.Collections;
using UnityEngine;

namespace JuiceBits
{
    public class ZoomModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Zoom";
        public float CameraStartSize;
        public float TargetValue = 9f;
        public float ZoomBuffer;
        public float ZoomOutDuration = 0.5f;
        public float ZoomInDuration = 0.3f;
        public float Delay;
        public bool UseCurve;
        public AnimationCurve AnimationCurve;
        public EaseTypes EaseTypes = EaseTypes.Linear;
        public CinemachineCamera CinemachineCamera;

        private float _elapsedTime = 0f;
        private float _damping = 0f;
        private float _time;
        private float _easedTime;
        private float _zoomValue;
        private bool _isZooming;
        private bool _isOrthographic;
        private Coroutine _coroutine;
        private Coroutine _repeatingCoroutine;

        // Set the default value of the animation curve to linear
        private void Reset()
        {
            AnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }

        // Executes the zoom logic
        public override void Play()
        {
            if (_isZooming) return;

            if (Probability > 0f)
            {
                float randomValue = Random.Range(0f, 100f);

                if (randomValue > Probability)
                {
                    return;
                }
            }

            if (_coroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutine);
                _coroutine = null;
            }

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
                _coroutine = CoroutineManager.Instance.RunTrackedCoroutine(PlayZoom());
            }
        }

        // Stops the zoom and resets the values for the next use
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
            _isZooming = false;

            if (_isOrthographic)
            {
                CameraStartSize = CinemachineCamera.Lens.OrthographicSize;
            }
            else
            {
                CameraStartSize = CinemachineCamera.Lens.FieldOfView;
            }
        }

        // Enables the repetition of the zoom
        private IEnumerator RepeatModule()
        {
            {
                for (int i = 1; i <= Repetitions; i++)
                {
                    _skipStartDelay = i > 1;

                    yield return CoroutineManager.Instance.StartCoroutine(PlayZoom());

                    yield return new WaitForSeconds(RepeatDelay);
                }
            }

            _isRepeating = false;

            // Checks if the zoom repetition is finished
            Finished();
        }

        // Zoom logic
        private IEnumerator PlayZoom()
        {
            _isZooming = true;
            _elapsedTime = 0f;
            _isOrthographic = CinemachineCamera.Lens.Orthographic;

            // Looks if the scene is using an orthographic or perspective camera view
            if (_isOrthographic)
            {
                CameraStartSize = CinemachineCamera.Lens.OrthographicSize;
            }
            else
            {
                CameraStartSize = CinemachineCamera.Lens.FieldOfView;
            }

            float startValue = CameraStartSize;
            float targetValue = TargetValue;

            if (!_skipStartDelay && IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            // Zoom Out
            while (_elapsedTime < ZoomOutDuration)
            {
                _elapsedTime += Time.deltaTime;
                _time = _elapsedTime / ZoomOutDuration;
                _easedTime = EaseManger.Easings(EaseTypes, _time);
                _damping = UseCurve ? AnimationCurve.Evaluate(_time) : Mathf.Lerp(0f, 1f, _easedTime);
                _zoomValue = Mathf.Lerp(CameraStartSize, TargetValue, _damping);

                if (_isOrthographic)
                {
                    CinemachineCamera.Lens.OrthographicSize = _zoomValue;
                }
                else
                {
                    CinemachineCamera.Lens.FieldOfView = _zoomValue;
                }

                yield return null;
            }

            // Checks for the values of orthographic or perspective
            if (_isOrthographic)
            {
                CinemachineCamera.Lens.OrthographicSize = targetValue;
            }
            else
            {
                CinemachineCamera.Lens.FieldOfView = targetValue;
            }

            yield return new WaitForSeconds(ZoomBuffer);

            _elapsedTime = 0f;

            // Zoom in
            while (_elapsedTime < ZoomInDuration)
            {
                _elapsedTime += Time.deltaTime;
                _time = _elapsedTime / ZoomInDuration;
                _easedTime = EaseManger.Easings(EaseTypes, _time);
                _damping = UseCurve ? AnimationCurve.Evaluate(_time) : Mathf.Lerp(0f, 1f, _easedTime);
                _zoomValue = Mathf.Lerp(TargetValue, CameraStartSize, _damping);

                // Checks for the values of orthographic or perspective
                if (_isOrthographic)
                {
                    CinemachineCamera.Lens.OrthographicSize = _zoomValue;
                }
                else
                {
                    CinemachineCamera.Lens.FieldOfView = _zoomValue;
                }

                yield return null;
            }

            if (_isOrthographic)
            {
                CinemachineCamera.Lens.OrthographicSize = startValue;
            }
            else
            {
                CinemachineCamera.Lens.FieldOfView = startValue;
            }

            _isZooming = false;
            _coroutine = null;

            // Checks if the zoom is finished
            if (!_isRepeating)
            {
                Finished();
            }
        }
    }
}